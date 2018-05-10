using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pjs1.BLL.Implementations;
using Pjs1.BLL.Interfaces;
using Pjs1.DAL.PostgreSQL.Implementations;
using Pjs1.DAL.PostgreSQL.Interfaces;
using Pjs1.Main.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pjs1.Main.Services
{
    public static class ServiceExtensions
    {
        public static void AddIoc(this IServiceCollection services, IConfiguration configuration,
            IHostingEnvironment hostingEnvironment)
        {
            var ioc = new IocConfigurationService();
            ioc.Configure(services, configuration, hostingEnvironment);
        }
    }

    public class IocConfigurationService
    {
        public void Configure(IServiceCollection services, IConfiguration configuration,
            IHostingEnvironment hostingEnvironment)
        {
            #region Transient

            services.AddTransient<IEmailSender, EmailSender>();

            #endregion


            #region Scoped

            services.AddScoped(typeof(IEntityFrameworkRepository<,>), typeof(EntityFrameworkRepository<,>));
            services.AddScoped<IUserService, UserService>();

            #endregion


            #region Singleton

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<UrlRedirectRule>();

            #endregion

        }
    }
}
