using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Protocol;

namespace MQTT_Logger_Client
{
	internal class Client
	{
		public string ServerIp { get; set; } = "127.0.0.1";
		public List<string> Topics { get; set; } = new List<string>();



		public async Task Subscribe_Multiple_Topics()
		{
			/*
			 * This sample subscribes to several topics in a single request.
			 */

			var mqttFactory = new MqttFactory();

			using var mqttClient = mqttFactory.CreateMqttClient();
			//var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("broker.hivemq.com").Build();
			var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(ServerIp).Build();

			mqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceived;

			await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

			// Create the subscribe options including several topics with different options.
			// It is also possible to all of these topics using a dedicated call of _SubscribeAsync_ per topic.

			var mqttSubscribeOptionsBuilder = mqttFactory.CreateSubscribeOptionsBuilder();

			foreach (var topic in Topics)
			{
				mqttSubscribeOptionsBuilder.WithTopicFilter(f => f.WithTopic(topic));
			}

			var mqttSubscribeOptions = mqttSubscribeOptionsBuilder.Build();
			
			var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

			Program.Logger.LogMessage("MQTT client subscribed to topics.");

			Program.Logger.LogMessage("Press enter to exit.");
			Console.ReadLine();

			//response.DumpToConsole();
		}

		private Task HandleApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
		{
			Program.Logger.LogFromTopic(e.ApplicationMessage.Topic, e.ApplicationMessage.ConvertPayloadToString());
			//Console.WriteLine($"{DateTime.Now}: On \"{e.ApplicationMessage.Topic}\": \t {e.ApplicationMessage.ConvertPayloadToString()}");
			return Task.CompletedTask;
		}
	}
}
