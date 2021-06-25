using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using BatteryMonitor.ConfigurationSettings;
using BatteryMonitor.Util;

namespace BatteryMonitor
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly IConfiguration _config;
		readonly string imagePath = String.Format("{0}\\Static\\{1}", Environment.CurrentDirectory, "error.png");
		private AppSettings _appSettings;
		private WindowsToast _toaster;
		// private ShellScript _shellScript;
		private ObjectQueryUtil _ojectQuery;

		public Worker(ILogger<Worker> logger, IConfiguration configuration)
		{
            _logger = logger;
			_config = configuration;
		}

        public override Task StartAsync(CancellationToken cancellationToken)
		{
			_appSettings = new AppSettings();
			_config.GetSection(AppSettings.Position).Bind(_appSettings);
			//_toaster = new WindowsToast();
			_ojectQuery = new ObjectQueryUtil();
			//_shellScript = new ShellScript();
			return base.StartAsync(cancellationToken);
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
			_logger.LogInformation("Service Stopped!");
			return base.StopAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			int prevBattryPer = 0;

			while (!stoppingToken.IsCancellationRequested)
			{
				string currBattryPerStr = "0";
				var battryDetail = _ojectQuery.ExecuteObjectQuerry(_appSettings.BatteryProperties, _appSettings.BatteryClass);
				battryDetail.TryGetValue(_appSettings.BatteryProperties[0], out currBattryPerStr);
				int currBattryPer = Int32.Parse(currBattryPerStr);

				string battryStatusStr;
				battryDetail.TryGetValue(_appSettings.BatteryProperties[1], out battryStatusStr);
				bool isPluggedIn = battryStatusStr == "2";

				_logger.LogInformation("Battery percentage: {0}\tIs plugged: {1}", currBattryPer, isPluggedIn);

				// old method
				// var shellOut = shellScript.Run(_appSettings.CheckPluggedIn);
				// bool isPluggedIn = shellOut.Split("\n")[1] == "2";

				if (currBattryPer < prevBattryPer && isPluggedIn)
				{
					// cannot show toast messages from a windows service, Sad ;-(
					//_toaster.GenerateToast(_appSettings.AppId, imagePath, _appSettings.ToastMessage, "", "");
					_logger.LogError("Plugged-in Not Charging, Battery: {0}", currBattryPer);
				}


				// _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
				await Task.Delay(_appSettings.CheckDelay, stoppingToken);
				prevBattryPer = currBattryPer;
			}
		}

		
	}
}
