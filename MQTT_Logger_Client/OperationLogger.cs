using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MQTT_Logger_Client
{
	internal class OperationLogger
	{
		private DateTime operationTime;
		private string savePath;
		private StreamWriter _sw;

		public OperationLogger()
		{
			operationTime = DateTime.Now;
			savePath = AppDomain.CurrentDomain.BaseDirectory + "Logfile_" +
					   DateTime.Now.ToShortDateString().Replace('.', '_') + '_' +
					   DateTime.Now.ToShortTimeString().Replace(':', '_') + ".log";
			if (File.Exists(savePath))
			{
				File.Move(savePath, Path.ChangeExtension(savePath, null) + ".old.log");
			}
			// Create a file to write to.
			using StreamWriter sw = File.CreateText(savePath);
			sw.WriteLine("Hello Log File...");
			sw.WriteLine($"Logging started: {DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}");
			_sw = File.AppendText(savePath);
		}


		public void LogMessage(string message)
		{
			_sw.WriteLine($"--> {(DateTime.Now - operationTime).TotalSeconds}, at: {DateTime.Now.ToShortTimeString()}: {message}");
		}

		public void LogFromTopic(string topic, string payload)
		{
			StringBuilder sb = new StringBuilder();
			sb.Append($"--> {(DateTime.Now - operationTime).TotalSeconds}, at: {DateTime.Now.ToShortTimeString()}:");
			sb.AppendLine($"> From {topic}:");
			sb.AppendLine($"> ContentL {payload}");
			
			_sw.WriteLine(sb.ToString());
			Console.WriteLine(sb.ToString());
		}
	}
}
