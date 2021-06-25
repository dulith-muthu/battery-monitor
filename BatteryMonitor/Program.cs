using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace BatteryMonitor
{
	public class Program
	{
		public static void Main(string[] args)
		{
			Log.Logger = new LoggerConfiguration()
				.MinimumLevel.Debug()
				.MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
				.Enrich.FromLogContext()
				.WriteTo.File(@"C:\Users\dulit\Desktop\BatteryMonitor.txt")
				.CreateLogger();

			try
			{
				Log.Information("Starting up the service");
				CreateHostBuilder(args).Build().Run();
				return;
			}
			catch (System.Exception ex)
			{
				Log.Fatal(ex, "There was a problem starting the service");
				return;
			}
			finally
			{
				Log.CloseAndFlush();
			}
		}

		public static IHostBuilder CreateHostBuilder(string[] args) =>
			Host.CreateDefaultBuilder(args)
				.UseWindowsService()
				.ConfigureServices((hostContext, services) =>
				{
					services.AddHostedService<Worker>();
				})
				.UseSerilog();
	}
}
