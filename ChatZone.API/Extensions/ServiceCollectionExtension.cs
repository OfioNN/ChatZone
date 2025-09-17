using ChatZone.Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace ChatZone.API.Extensions {
    public static class ServiceCollectionExtension {

        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration) {
            var connectionString = configuration.GetValue<string>("ConnectionString");
            services.AddDbContext<ChatDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }
    }
}
