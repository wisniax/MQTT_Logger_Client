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
		private IMqttClient mqttClient;
		private CurrentTelemetry _telem = new();

		public async Task SubscribeOnTopics()
		{
			var mqttFactory = new MqttFactory();
			mqttClient = mqttFactory.CreateMqttClient();
			var mqttClientOptions = new MqttClientOptionsBuilder().WithTcpServer(ServerIp).Build();
			mqttClient.ApplicationMessageReceivedAsync += HandleApplicationMessageReceived;
			mqttClientOptions.CleanSession = false;
			await mqttClient.ConnectAsync(mqttClientOptions, CancellationToken.None);

			var mqttSubscribeOptionsBuilder = mqttFactory.CreateSubscribeOptionsBuilder();
			foreach (var topic in Topics)
			{
				mqttSubscribeOptionsBuilder.WithTopicFilter(f => f.WithTopic(topic));
			}
			var mqttSubscribeOptions = mqttSubscribeOptionsBuilder.Build();

			var response = await mqttClient.SubscribeAsync(mqttSubscribeOptions, CancellationToken.None);
			mqttClient.DisconnectedAsync += HandleDisconnection;

			Program.Logger.LogMessage("MQTT client subscribed to topics.");
			Program.Logger.LogMessage("Press enter to exit.");
			Console.ReadLine();
		}

		private async Task HandleDisconnection(MqttClientDisconnectedEventArgs arg)
		{
			if (mqttClient.IsConnected) return;
			await mqttClient.ReconnectAsync();
			//throw new NotImplementedException();
		}

		private async Task HandleApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
		{
			Program.Logger.LogFromTopic(e.ApplicationMessage.Topic, e.ApplicationMessage.ConvertPayloadToString());
			await _telem.UpdateData(e.ApplicationMessage);
			//Console.WriteLine($"{DateTime.Now}: On \"{e.ApplicationMessage.Topic}\": \t {e.ApplicationMessage.ConvertPayloadToString()}");
			//return Task.CompletedTask;
		}
	}
}
