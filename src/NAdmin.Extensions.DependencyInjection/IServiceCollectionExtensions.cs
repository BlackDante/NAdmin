using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NAdmin.Configuration;
using NAdmin.Persistence;

namespace NAdmin.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNAdmin(this IServiceCollection services, Action<NAdminConfigurationBuilder> configuration)
        {
            var configurationBuilder = new NAdminConfigurationBuilder();

            configuration(configurationBuilder);

            var config = configurationBuilder.Build();
            services.AddSingleton(config);

            services.AddDbContext<NAdminDbContext>(opt =>
            {
                opt.UseSqlServer(config.SqlServerConnectionString, conf =>
                {
                    conf.MigrationsAssembly(typeof(NAdminConfiguration).Assembly.FullName);
                });
            });

            services.AddIdentityCore<ApplicationUser>()
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<NAdminDbContext>()
                .AddSignInManager()
                .AddDefaultTokenProviders();

            services.AddAuthentication(o =>
            {
                o.DefaultScheme = IdentityConstants.ApplicationScheme;
                o.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
                .AddIdentityCookies(o => {});
            
            return services;
        }
    }
    
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseNAdmin(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            return app;
        }
    }
}