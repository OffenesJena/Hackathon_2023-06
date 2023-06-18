using MQTTnet;
using MQTTnet.Client;
using Newtonsoft.Json.Linq;
using System.Security.Authentication;
using System.Text;

public class MqttClientService
{

    private IMqttClient?       client;
    private MqttClientOptions? options;

    public async Task ConnectAsync(String  hostname,
                                   Int16   port,
                                   String  username,
                                   String  password,
                                   String  topic)
    {

        var factory = new MqttFactory();
        client = factory.CreateMqttClient();

        // Optionen für die MQTT-Verbindung
        options = new MqttClientOptionsBuilder().
                      WithTcpServer   (hostname, port).
                      WithCredentials (username, password).
                      WithClientId    (Guid.NewGuid().ToString()).
                      WithCleanSession().
                      WithTls         (o => {
                                                o.CertificateValidationHandler = _ => true;
                                                o.SslProtocol = SslProtocols.Tls12;
                                            }).
                      Build           ();

        // Handle event when mqtt messages are received.
        client.ApplicationMessageReceivedAsync += async e => {

            Console.WriteLine($"Topic: {e.ApplicationMessage.Topic}");
            Console.WriteLine($"Payload: {Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment)}\n");

            var json = JObject.Parse(Encoding.UTF8.GetString(e.ApplicationMessage.PayloadSegment));

            File.AppendAllText("hackathonJena4.log", e.ApplicationMessage.Topic);
            File.AppendAllText("hackathonJena4.log", json.ToString() + Environment.NewLine + Environment.NewLine);

        };

        await client.ConnectAsync(options);

        await client.SubscribeAsync(new MqttTopicFilterBuilder().
                                        WithTopic(topic).
                                        Build    ());

    }

}

