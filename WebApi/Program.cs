using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using NoScam.Net;
using System.IO;
using System.Reflection;

namespace WebApi
{
	public static class Program
	{
		public static readonly SpamDetector filter = new SpamDetector();

		public static void Main(string[] args)
		{
			var currentPath = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
			var spamPath = Path.Combine(currentPath, @"Resources", @"spam.txt");
			var hamPath = Path.Combine(currentPath, @"Resources", @"ham.txt");

			filter.Train(spamPath, hamPath);
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args).UseStartup<Startup>();
	}
}
