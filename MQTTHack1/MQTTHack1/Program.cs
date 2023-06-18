
var mqttService = new MqttClientService();

// wss://mqttbr.jena.de:8081
// mqtt://mqttbr.jena.de:8883
await mqttService.ConnectAsync(hostname:  "mqttbr.jena.de",
                               port:       8883,
                               username:  "login",
                               password:  "password",
                               topic:     "#");

while (true) {
    Thread.Sleep(1000);
}
