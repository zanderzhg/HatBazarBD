using DAL.Repository;
using DAL.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DAL.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.Builder;

namespace HatBazarBD.Services
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection InjectApplicationServices(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }

        public static IServiceCollection AddCustomizedDataStore(this IServiceCollection services,IConfiguration configuration)
        {
            services.AddDbContextPool<ApplicationDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DeafultConnection"), b => b.MigrationsAssembly("HatBazar")));
            return services;
        }

        public static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication();
            return services;
        }

        public static IServiceCollection RegisterGzip(this IServiceCollection services)
        {
            services.Configure<GzipCompressionProviderOptions>(options =>
                options.Level = System.IO.Compression.CompressionLevel.Optimal);
            services.AddResponseCompression(options =>
            {
                options.MimeTypes = new[]
                {
                    // Default
                    "text/plain",
                    "text/css",
                    "application/javascript",
                    "text/html",
                    "application/xml",
                    "text/xml",
                    "application/json",
                    "text/json",
                    // Custom
                    "image/svg+xml",
                    "font/woff2",
                    "application/font-woff",
                    "application/font-ttf",
                    "application/font-eot",
                    "image/jpeg",
                    "image/png"
                };
            });

            return services;
        }
    }
}
