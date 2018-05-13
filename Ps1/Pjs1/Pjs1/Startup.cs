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
using Swashbuckle.AspNetCore.Swagger;
using Pjs1.Main.Routing;
using Microsoft.AspNetCore.Rewrite;

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

            services.AddDbContext<Postgre1DbContext>((sp, options) =>
                // This line for UseNpgsql add Can add migration
                options.UseNpgsql(Configuration.GetConnectionString("Db1ConnectionNpgsql"))
            );

            services.AddDbContext<MsSql1DbContext>((sp, options) =>
                // This line for UseSqlServer add Can add migration
                options.UseSqlServer(Configuration.GetConnectionString("Db1ConnectionMsSql")) 
            );

            services.AddIoc(Configuration, _env);
            services.AddMvc();
            #region Swagger
            services.AddSwaggerGen(config =>
            {
                config.SwaggerDoc("v1", new Info { Title = "ProjectStructure API", Version = "v1" });
            });
            #endregion

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseBrowserLink();
            if (env.IsDevelopment())
            {
                app.UseTrapMiddleware();

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //app.UseExceptionHandler("/Error");
            }
            else
            {
                //for error  404
                app.UseStatusCodePagesWithReExecute("/Error/{0}");
                //for error another code
                app.UseExceptionHandler("/Error");
            }
            app.UseMvc();

            #region Swagger
            //https://localhost:44372/swagger/#!/swagger/v1/swagger.json
            app.UseSwagger();
            app.UseSwaggerUI(config =>
            {
                config.SwaggerEndpoint("/swagger/v1/swagger.json", "Structure API V1");
            });
            #endregion


            app.UseCors("AllowAll");




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

            var options = new RewriteOptions();
            var redirectionRule = app.ApplicationServices.GetService<UrlRedirectRule>();
            options.Add(redirectionRule);
            app.UseRewriter(options);

             
            app.UseDefaultFiles();
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
