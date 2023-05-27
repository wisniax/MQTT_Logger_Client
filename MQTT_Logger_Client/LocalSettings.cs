using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MQTT_Logger_Client
{
	public class SettingsType
	{
		public string ServerIp { get; set; } = "broker.hivemq.com";
		public List<string> Topics { get; set; } = new List<string>()
		{
			"RAPTORS/test/#", "MScrRouter/image/check", "RAPTORS/hex", "RAPTORS/mavic"
		};
	}

	public class LocalSettings
	{
		private string _path;
		public SettingsType? Settings { get; set; }

		public LocalSettings()
		{
			_path = AppDomain.CurrentDomain.BaseDirectory + "settings.cfg";
			if (!Load()) Save();

		}

		public bool Save()
		{
			try
			{
				//if (File.Exists(_path)) { File.Delete(_path); }
				var serialized = JsonSerializer.Serialize(Settings);
				using var sw = File.CreateText(_path);
				sw.Write(serialized);
			}
			catch (Exception e)
			{
				Settings = new SettingsType();
				return false;
			}

			return true;
		}

		public bool Load()
		{
			try
			{
				using StreamReader sr = new StreamReader(_path);
				var serialized = sr.ReadToEnd();
				sr.Close();
				Settings = JsonSerializer.Deserialize<SettingsType>(serialized);
			}
			catch (Exception e)
			{
				Settings = new SettingsType();
				return false;
			}

			return true;
		}
	}
}
