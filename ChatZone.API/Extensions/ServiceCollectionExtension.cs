using ChatZone.Core.Application.Services;
using ChatZone.Core.Domain;
using ChatZone.Core.Domain.Interfaces.Producer;
using ChatZone.Core.Domain.Interfaces.Repositories;
using ChatZone.Core.Domain.Interfaces.Services;
using ChatZone.Core.Domain.Options;
using ChatZone.Infrastructure.Producer;
using ChatZone.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ChatZone.API.Extensions {
    public static class ServiceCollectionExtension {

        public static IServiceCollection AddConfiguration(this IServiceCollection services, IConfiguration configuration) {
            AddCustomAuthentication(services, configuration);

            var connectionString = configuration.GetValue<string>("ConnectionString");
            services.AddDbContext<ChatDbContext>(options =>
                options.UseSqlServer(connectionString));
            return services;
        }

        public static IServiceCollection AddOptions(this IServiceCollection services, IConfiguration configuration) {
            services.Configure<JwtSettingOption>(options => configuration.GetSection(nameof(JwtSettingOption)).Bind(options));
            services.Configure<KafkaOption>(options => configuration.GetSection(nameof(KafkaOption)).Bind(options));
            return services;
        }

        private static void AddCustomAuthentication(IServiceCollection services, IConfiguration configuration) {
            var jwtSettings = configuration.GetSection(nameof(JwtSettingOption)).Get<JwtSettingOption>();

            if (jwtSettings == null || string.IsNullOrEmpty(jwtSettings.SecretKey))
                throw new ArgumentException("Secret Key is empty");
            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);

            services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.SaveToken = true;
                options.TokenValidationParameters = GetTokenValidationParams(key);
                options.Events = GetEvents();
            });
        }

        private static JwtBearerEvents GetEvents() {
            return new JwtBearerEvents {
                OnMessageReceived = context => {
                    var accessToken = context.Request.Query["token"];
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/messageHub")) {
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
        }

        private static TokenValidationParameters GetTokenValidationParams(byte[] key) {
            return new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(120)
            };
        }

        public static IServiceCollection AddServices(this IServiceCollection services) {
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IChatRepository, ChatRepository>();
            services.AddTransient<IAuthService, AuthService>();
            services.AddTransient<IJwtService, JwtService>();
            services.AddTransient<IChatService, ChatService>();
            services.AddSingleton(new UserConnectionService());
            services.AddTransient<IKafkaProducer, KafkaProducer>();
            services.AddSignalR();
            return services;
        }
    }
}
