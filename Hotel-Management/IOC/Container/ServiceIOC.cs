using Microsoft.Extensions.DependencyInjection;
using Service;

namespace IOC.Container
{
    public class ServiceIOC
    {
        public static void ServiceConfigure(IServiceCollection services)
        {
            services.AddScoped<RoomService>();
        }
    }
}
