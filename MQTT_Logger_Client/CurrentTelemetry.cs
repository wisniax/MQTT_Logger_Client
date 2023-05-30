using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MQTTnet;

namespace MQTT_Logger_Client
{
	public class CurrentTelemetry
	{
		public ConnectionState HexConnection { get; private set; }
		public JsonTemplates.CurrPos HexCurrentPos { get; private set; }
		public JsonTemplates.OnShoot OnShoot {get; private set; }

		private MapPublisher _publisher = new();

		public async Task UpdateData(MqttApplicationMessage msg)
		{
			if (msg.Topic == Program.Settings.Settings?.HexDataTopic)
			{
				var deserialized = JsonSerializer.Deserialize<JsonTemplates.HexData>(msg.ConvertPayloadToString());
				HexCurrentPos = deserialized!;
				await _publisher.PublishCurrentPosAsync(JsonSerializer.Serialize(HexCurrentPos));
			}
			else if (msg.Topic == Program.Settings.Settings?.HexShootTopic)
			{
				var deserialized = JsonSerializer.Deserialize<JsonTemplates.HexShoot>(msg.ConvertPayloadToString());
				OnShoot.shoot = deserialized?.data ?? 0;
				OnShoot.timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
				OnShoot.curr_pos = HexCurrentPos;
				await _publisher.PublishOnShootAsync(JsonSerializer.Serialize(HexCurrentPos));
			}
			else if (msg.Topic == Program.Settings.Settings?.HexStatusTopic)
			{
				var deserialized = JsonSerializer.Deserialize<JsonTemplates.HexStatus>(msg.ConvertPayloadToString());
				switch (deserialized?.status)
				{
					case "CONNECTED":
						HexConnection = ConnectionState.Open;
						break;
					case "DISCONNECTED":
						HexConnection = ConnectionState.Closed;
						break;
					case "LOST_CONNECTION":
						HexConnection = ConnectionState.Broken;
						break;
				}
			}
			else if (msg.Topic == Program.Settings.Settings?.HexWaypointTopic)
			{
				//var deserialized = JsonSerializer.Deserialize<JsonTemplates.HexWaypoint>(msg.ConvertPayloadToString());
			}
		}
	}
}
