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
		string _serverIp;

		public Client(string serverIp = "broker.hivemq.com")
		{
			_serverIp = serverIp;
		}



		public async Task Subscribe_Multiple_Topics()
		{
			/*
			 * This sample subscribes to several topics in a single request.
			 */

			var mqttFactory = new MqttFactory();

			using var mqttClient = mqttFactory.CreateMqttClient();
			//var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer("broker.hivemq.com").Build();
			var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(_serverIp).Build();

			mqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceived;

			await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

			// Create the subscribe options including several topics with different options.
			// It is also possible to all of these topics using a dedicated call of _SubscribeAsync_ per topic.
			var mqttSubscribeOptions = mqttFactory.CreateSubscribeOptionsBuilder()
				.WithTopicFilter(
					f =>
					{
						f.WithTopic("RAPTORS/test/#");
					})
				.WithTopicFilter(
					f =>
					{
						f.WithTopic("MScrRouter/image/check");
					})
				.WithTopicFilter(
					f =>
					{
						f.WithTopic("RAPTORS/hex");
					})
				.WithTopicFilter(
					f =>
					{
						f.WithTopic("RAPTORS/mavic"); // .WithRetainHandling(MqttRetainHandling.SendAtSubscribe)
					})
				.Build();


			var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);

			Console.WriteLine("MQTT client subscribed to topics.");

			Console.WriteLine("Press enter to exit.");
			Console.ReadLine();

			//response.DumpToConsole();
		}

		private Task HandleApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
		{
			//Program.Logger.LogMessage();
			Console.WriteLine($"{DateTime.Now}: On \"{e.ApplicationMessage.Topic}\": \t {e.ApplicationMessage.ConvertPayloadToString()}");
			return Task.CompletedTask;
		}
	}
}
