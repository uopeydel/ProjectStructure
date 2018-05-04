using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pjs1.Main.Data;
using Pjs1.Main.Models;
using Pjs1.Main.Services;
using Pjs1.Common.DAL;
using Pjs1.Main.PubSub;
using Pjs1.Main.PubSubHub;

namespace Pjs1.Main
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _env = env;
            Configuration = configuration;
        }

        private readonly IHostingEnvironment _env;
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add-Migration CreateMigrationAgentAuth -Context ApplicationDbContext
            // Update-Database CreateMigrationAgentAuth -Context ApplicationDbContext
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("AuthConnection")));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<Postgre1DbContext>(options =>
                // This line for UseNpgsql add Can add migration
                options.UseNpgsql(Configuration.GetConnectionString("Db1Connection"))
            );

            services.AddIoc(Configuration, _env);
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }





            #region UseWebSocketsOptions
            //var webSocketOptions = new WebSocketOptions()
            //{
            //    KeepAliveInterval = TimeSpan.FromSeconds(120),
            //    ReceiveBufferSize = 4 * 1024
            //};
             
            app.UseWebSockets(/*webSocketOptions*/);
            #endregion
            #region UsePubSubProvider
            try
            {
                app.UsePubSubProvider(conf =>
                {
                    conf.MapChannel<Chanel1Hub>("Chanel1HubSlug");
                });
            }
            catch (Exception e)
            {
                var a = e;  
            }
            #endregion









            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
