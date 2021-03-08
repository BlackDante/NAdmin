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
        public static Microsoft.Extensions.DependencyInjection.IMvcBuilder AddNAdmin(this Microsoft.Extensions.DependencyInjection.IMvcBuilder builder)
        {
            builder.AddApplicationPart(typeof(NAdminConfiguration).Assembly)
                .AddControllersAsServices();

            return builder;
        }
        
        public static IServiceCollection AddNAdmin(this IServiceCollection services,
            Func<NAdminConfigurationBuilder, NAdminConfigurationBuilder> configuration)
        {
            var configurationBuilder = new NAdminConfigurationBuilder();
            
            var config = configuration(configurationBuilder).Build();
            
            services.AddSingleton(config);

            services.Scan(scan => scan.FromAssemblies(config.EntitiesAssemblies)
                .AddClasses(@class => @class.AssignableTo<INAdminEntity>())
                .AsImplementedInterfaces());

            services.AddDbContext<NAdminDbContext>(opt =>
            {
                opt.UseSqlServer(config.SqlServerConnectionString, conf =>
                {
                    conf.MigrationsAssembly(config.MigrationAssembly);
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