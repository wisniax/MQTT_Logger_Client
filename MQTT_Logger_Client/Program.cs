namespace MQTT_Logger_Client
{
	internal class Program
	{
		public static OperationLogger Logger {get; private set; }
		public static LocalSettings Settings { get; private set; }


		static async Task Main(string[] args)
		{
			Logger = new OperationLogger();
			Settings = new LocalSettings();

			var cl = new Client()
			{
				Topics = Settings?.Settings?.Topics ?? new List<string>(),
				ServerIp = Settings?.Settings?.ServerIp ?? "127.0.0.1",
			};
			await cl.Subscribe_Multiple_Topics();
		}
	}
}