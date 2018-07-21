using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pjs1.BLL.Implementations;
using Pjs1.BLL.Interfaces;
using Pjs1.DAL.Implementations;
using Pjs1.DAL.Interfaces;
using Pjs1.Main.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Pjs1.Common.GenericDbContext;
using Pjs1.Main.Service.Implement;
using Pjs1.Main.Service.Interface;

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
            services.AddScoped<ITestGenericIdentityService, TestGenericIdentityService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<IProjectHelper, ProjectHelper>();
            #endregion


            #region Singleton

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<UrlRedirectRule>();

            #endregion

        }
    }
}
