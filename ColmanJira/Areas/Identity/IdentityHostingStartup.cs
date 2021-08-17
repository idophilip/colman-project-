using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(ColmanJira.Areas.Identity.IdentityHostingStartup))]
namespace ColmanJira.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
            });
        }
    }
}