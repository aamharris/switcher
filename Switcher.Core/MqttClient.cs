using System;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client.Options;
using MQTTnet.Extensions.ManagedClient;

namespace Switcher.Core
{
    public class MqttClient
    {
        public IManagedMqttClient client;
        public event EventHandler<string> OnTopicRecieved;

        public MqttClient()
        {
        }

        public async Task ConnectAsync()
        {
            string clientId = Guid.NewGuid().ToString();
            string mqttURI = "YOUR_MQTT_URI_HERE";
            string mqttUser = "YOUR_MQTT_USER_HERE";
            string mqttPassword = "YOUR_PASSWORD_HERE";
            int mqttPort = 000000000;
            bool mqttSecure = false;
            var messageBuilder = new MqttClientOptionsBuilder()
                    .WithClientId(clientId)
                    .WithCredentials(mqttUser, mqttPassword)
                    .WithTcpServer(mqttURI, mqttPort)
                    .WithCleanSession();

            var options = mqttSecure ? messageBuilder.WithTls().Build()
              : messageBuilder.Build();

            var managedOptions = new ManagedMqttClientOptionsBuilder()
              .WithAutoReconnectDelay(TimeSpan.FromSeconds(5))
              .WithClientOptions(options)
              .Build();

            client = new MqttFactory().CreateManagedMqttClient();
            await client.StartAsync(managedOptions);
        }


        /// <summary>
        /// Publish Message.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="payload">Payload.</param>
        /// <param name="retainFlag">Retain flag.</param>
        /// <param name="qos">Quality of Service.</param>
        /// <returns>Task.</returns>
        public async Task PublishAsync(string topic, string payload, bool retainFlag = true, int qos = 1)
        {
            await client.PublishAsync(new MqttApplicationMessageBuilder()
                      .WithTopic(topic)
                      .WithPayload(payload)
                      .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
                      .WithRetainFlag(retainFlag)
                      .Build());
        }

        /// <summary>
        /// Subscribe topic.
        /// </summary>
        /// <param name="topic">Topic.</param>
        /// <param name="qos">Quality of Service.</param>
        /// <returns>Task.</returns>
        public async Task SubscribeAsync(string topic, int qos = 1)
        {
            await client.SubscribeAsync(new TopicFilterBuilder()
                      .WithTopic(topic)
                      .WithQualityOfServiceLevel((MQTTnet.Protocol.MqttQualityOfServiceLevel)qos)
                      .Build());

            client.UseApplicationMessageReceivedHandler(e =>
            {
                OnTopicRecieved.Invoke(
                    this,
                    Encoding.UTF8.GetString(e.ApplicationMessage.Payload)
                    );
            });
        }
    }
}
