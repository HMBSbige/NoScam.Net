using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NoScam.Net;

namespace WebApi
{
	public class Program
	{
		public static SpamDetector filter=new SpamDetector();
		public static void Main(string[] args)
		{
			var spamPath = @"D:\Cloud\Git\NoScam.Net\NoScam.Net\Resources\spam.txt";
			var hamPath = @"D:\Cloud\Git\NoScam.Net\NoScam.Net\Resources\ham.txt";

			filter.Train(spamPath, hamPath);
			CreateWebHostBuilder(args).Build().Run();
		}

		public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
			WebHost.CreateDefaultBuilder(args)
				.UseStartup<Startup>();
	}
}
