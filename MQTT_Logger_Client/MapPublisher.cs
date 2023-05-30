using MQTTnet.Client;
using MQTTnet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Logger_Client
{
	public class MapPublisher
	{
		public string ServerIp { get; set; } = "127.0.0.1";

		private IMqttClient? _mqttClient;

		public MapPublisher()
		{
			var mqttFactory = new MqttFactory();
			_mqttClient = mqttFactory.CreateMqttClient();
		}

		public async Task ConnectAsync()
		{
			var mqttClientOptions = new MqttClientOptionsBuilder()
				.WithTcpServer(ServerIp)
				.Build();
			_mqttClient?.ConnectAsync(mqttClientOptions, CancellationToken.None);
		}

		public async Task PublishCurrentPosAsync(string serialized)
		{
			if (_mqttClient is not { IsConnected: true }) return;
			var applicationMessage = new MqttApplicationMessageBuilder()
				.WithTopic(Program.Settings.Settings?.HexCurrentPositionTopic)
				.WithPayload(serialized)
				.Build();

			await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
		}

		public async Task PublishOnShootAsync(string serialized)
		{
			if (_mqttClient is not { IsConnected: true }) return;
			var applicationMessage = new MqttApplicationMessageBuilder()
				.WithTopic(Program.Settings.Settings?.HexOnShootTopic)
				.WithPayload(serialized)
				.Build();

			await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);
		}

		[Obsolete]
		private async Task Publish_Application_Message()
		{
			/*
			 * This sample pushes multiple simple application message including a topic and a payload.
			 *
			 * See sample _Publish_Application_Message_ for more details.
			 */
			if (_mqttClient is not { IsConnected: true }) return;

			

			var applicationMessage = new MqttApplicationMessageBuilder()
				.WithTopic("samples/temperature/living_room")
				.WithPayload("20.0")
				.Build();

			await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

			applicationMessage = new MqttApplicationMessageBuilder()
				.WithTopic("samples/temperature/living_room")
				.WithPayload("21.0")
				.Build();

			await _mqttClient.PublishAsync(applicationMessage, CancellationToken.None);

			await _mqttClient.DisconnectAsync();

			Console.WriteLine("MQTT application message is published.");
		}
	}
}
