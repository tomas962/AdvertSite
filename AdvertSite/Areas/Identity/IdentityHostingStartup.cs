using Microsoft.AspNetCore.Hosting;

[assembly: HostingStartup(typeof(AdvertSite.Areas.Identity.IdentityHostingStartup))]
namespace AdvertSite.Areas.Identity
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