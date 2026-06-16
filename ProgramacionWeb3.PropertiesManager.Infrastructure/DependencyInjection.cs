using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Repositories;
using ProgramacionWeb3.PropertiesManager.Core.Interfaces.Services;
using ProgramacionWeb3.PropertiesManager.Infrastructure.Repositories;
using ProgramacionWeb3.PropertiesManager.Infrastructure.Services;

namespace ProgramacionWeb3.PropertiesManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString, string filesBasePath)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddScoped<IBuildingRepository, BuildingRepository>();
            services.AddScoped<IExpenseDistributionRepository, ExpenseDistributionRepository>();
            services.AddScoped<IExpenseRepository, ExpenseRepository>();
            services.AddScoped<IExpenseTypeRepository, ExpenseTypeRepository>();
            services.AddScoped<IMultipurposeRoomRepository, MultipurposeRoomRepository>();
            services.AddScoped<IMultipurposeRoomReservationRepository, MultipurposeRoomReservationRepository>();
            services.AddScoped<IOwnerRepository, OwnerRepository>();
            services.AddScoped<IProvinceRepository, ProvinceRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IUnitRepository, UnitRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            services.AddScoped<IEmailService, SmtpEmailService>();
            services.AddScoped<IFileService>(_ => new FileService(filesBasePath));

            return services;
        }
    }
}
