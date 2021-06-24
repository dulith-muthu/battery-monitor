using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using WorkerService1.ConfigurationSettings;
using WorkerService1.Util;

namespace WorkerService1
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IConfiguration _config;
		readonly string imagePath = String.Format("{0}\\Static\\{1}", Environment.CurrentDirectory, "error.png");

		public Worker(ILogger<Worker> logger, IConfiguration configuration)
		{
            _logger = logger;
			_config = configuration;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			var _appSettings = new AppSettings();
			_config.GetSection(AppSettings.Position).Bind(_appSettings);
			var _toaster = new WindowsToast();
			// var shellScript = new ShellScript();
			var ojectQuery = new ObjectQueryUtil();
			int prevBattryPer = 0;

			while (!stoppingToken.IsCancellationRequested)
			{
				string currBattryPerStr = "0";
				var battryDetail = ojectQuery.ExecuteObjectQuerry(_appSettings.BatteryProperties, _appSettings.BatteryClass);
				battryDetail.TryGetValue(_appSettings.BatteryProperties[0], out currBattryPerStr);
				int currBattryPer = Int32.Parse(currBattryPerStr);

				string battryStatusStr;
				battryDetail.TryGetValue(_appSettings.BatteryProperties[1], out battryStatusStr);
				bool isPluggedIn = battryStatusStr == "2";

				//_logger.LogInformation("Battry per: {0} \n      Is plugged: {1}", currBattryPer, isPluggedIn);

				// old method
				// var shellOut = shellScript.Run(_appSettings.CheckPluggedIn);
				// bool isPluggedIn = shellOut.Split("\n")[1] == "2";

				if (currBattryPer < prevBattryPer && isPluggedIn)
				{
					_toaster.GenerateToast(_appSettings.AppId, imagePath, _appSettings.ToastMessage, "", "");
					_logger.LogInformation("Plugged-in Not Charging, Battery: {0}", currBattryPer);
				}


				// _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(_appSettings.CheckDelay, stoppingToken);
				prevBattryPer = currBattryPer;
			}
		}

		
	}
}
