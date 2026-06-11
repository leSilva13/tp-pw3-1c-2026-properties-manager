using Microsoft.Extensions.DependencyInjection;
using ProgramacionWeb3.PropertiesManager.Web.Services;

namespace ProgramacionWeb3.PropertiesManager.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWeb(this IServiceCollection services)
        {
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}
