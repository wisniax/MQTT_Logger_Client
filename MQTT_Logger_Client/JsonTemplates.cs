using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Logger_Client
{
	public class JsonTemplates
	{
		public class CurrPos
		{
			public double latitude { get; set; } = 0;
			public double longitude { get; set; } = 0;
			public double altitude { get; set; } = 0;
		}

		public class OnShoot
		{
			public CurrPos curr_pos { get; set; } = new();
			public long timestamp { get; set; } = 0;
			public int shoot { get; set; } = 0;
		}

		public class HexData : CurrPos
		{
			public double pose_x { get; set; }
			public double pose_y { get; set; }
			public double pose_z { get; set; }
			public int hour { get; set; }
			public int minut { get; set; }
			public int sekun { get; set; }
			public int mqtt_sygnal_up { get; set; }
			public int accept { get; set; }
			public double velocity_linear_x { get; set; }
			public double velocity_linear_y { get; set; }
			public double velocity_linear_z { get; set; }
			public double velocity_angular_x { get; set; }
			public double velocity_angular_y { get; set; }
			public double velocity_angular_z { get; set; }
		}

		public class HexWaypoint
		{
			public double longitude_mqtt { get; set; }
			public double latitude_mqtt { get; set; }
			public int shoot_mqtt { get; set; }
			public int check_id { get; set; }
		}

		public class HexShoot
		{
			public int data { get; set; }
		}

		public class HexStatus
		{
			public string status { get; set; }
		}
	}
}
