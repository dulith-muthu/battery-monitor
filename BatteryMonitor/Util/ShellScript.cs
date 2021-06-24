using System;
using System.Diagnostics;

namespace BatteryMonitor.Util
{
    class ShellScript
    {
        public string Run(string command) {
			var process = new Process
			{
				StartInfo = new ProcessStartInfo(@"C:\Windows\System32\WindowsPowerShell\v1.0\powershell.exe",
					command)
				{
					WorkingDirectory = Environment.CurrentDirectory,
					RedirectStandardOutput = true,
					CreateNoWindow = true,
				}
			};
			process.Start();

			var reader = process.StandardOutput;
			return reader.ReadToEnd();
		}
    }
}
