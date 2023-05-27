using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Logger_Client
{
	public class OperationLogger
	{
		private DateTime _operationTime;
		private string _savePath;
		private string _path;
		private StreamWriter _sw;

		public OperationLogger()
		{
			_operationTime = DateTime.Now;
			_path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "logs");
			_savePath = Path.Combine(_path,
				"Logfile_" 
				+ DateTime.Now.ToShortDateString().Replace('.', '_') + '_' 
				+ DateTime.Now.ToShortTimeString().Replace(':', '_') + ".log");

			if (!Directory.Exists(_path)) Directory.CreateDirectory(_path);

			if (File.Exists(_savePath))
				File.Move(_savePath, Path.ChangeExtension(_savePath, null) + ".old.log");

			using (StreamWriter sw = File.CreateText(_savePath))
			{
				sw.WriteLine("Hello Log File...");
				sw.WriteLine($"Logging started: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}");
			};

			_sw = File.AppendText(_savePath);
			_sw.AutoFlush = true;
		}


		public void LogMessage(string message)
		{
			string str =
				$"> {(int)(DateTime.Now - _operationTime).TotalSeconds}, at: {DateTime.Now.ToShortTimeString()}: {message}";

			_sw.WriteLine(str);
			Console.WriteLine(str);
		}

		public void LogFromTopic(string topic, string payload)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"--> {(int)(DateTime.Now - _operationTime).TotalSeconds}s, at: {DateTime.Now.ToShortTimeString()}:");
			sb.AppendLine($"--> From: {topic}:");
			if (payload.Length <= 500) sb.AppendLine($"> Content {payload}");
			else sb.Append($"--> Payload exceeded max length printing first 100 characters: " +
						   $"--> {payload.AsSpan(0, 100)}");
			_sw.WriteLine(sb.ToString());
			Console.WriteLine(sb.ToString());
		}
	}
}
