using ChatZone.Core.Application.Services;
using ChatZone.Core.Domain;
using ChatZone.Core.Domain.Interfaces.Repositories;
using ChatZone.Core.Domain.Interfaces.Services;
using ChatZone.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.API.Extensions {
    public static class ServiceCollectionExtension {

        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration) {
            var connectionString = configuration.GetValue<string>("ConnectionString");
            services.AddDbContext<ChatDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services) {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthService, AuthService>();
            return services;
        }
    }
}
