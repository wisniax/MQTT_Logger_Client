namespace MQTT_Logger_Client
{
	internal class Program
	{
		public static OperationLogger Logger;

		static async Task Main(string[] args)
		{
			Logger = new OperationLogger();
			var cl = new Client();

			await cl.Subscribe_Multiple_Topics();
		}
	}
}