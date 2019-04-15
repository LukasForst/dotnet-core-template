using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace BandEr.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args)
        {
            var builder = WebHost.CreateDefaultBuilder(args);
            if (args.Any(x => string.Equals("memory", x, System.StringComparison.OrdinalIgnoreCase)))
                builder.UseStartup<InMemoryStartup>();
            else
                builder.UseStartup<Startup>();
            return builder;
        }
    }
}
