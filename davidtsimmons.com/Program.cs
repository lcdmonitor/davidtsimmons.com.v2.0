using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.DataProtection;
using Services.Repositories;
using Services;
using Microsoft.AspNetCore.Identity;
using davidtsimmons.com.Models;
using davidtsimmons.com.Services;
using davidtsimmons.com.CustomLogging;
using davidtsimmons.com.Authentication;
using Contracts.Authentication;
using TanvirArjel.Extensions.Microsoft.DependencyInjection;
using Microsoft.AspNetCore.HttpOverrides;

namespace davidtsimmons.com
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add Services to the container.
            builder.Services.AddControllersWithViews();

            // setup DI, etc.
            ConfigureServices(builder.Services);

            var redisConfigurationOptions = ConfigurationOptions.Parse("redis:6379");

            builder.Services.AddStackExchangeRedisCache(redisCacheConfig =>
            {
                redisCacheConfig.ConfigurationOptions = redisConfigurationOptions;
            });

            var redis = ConnectionMultiplexer.Connect("redis:6379");

            builder.Services.AddDataProtection()
            .PersistKeysToStackExchangeRedis(redis, "DataProtection-Keys");

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "davidtsimmons.com_session";
                options.IdleTimeout = TimeSpan.FromMinutes(60 * 24);
            });

            #region Logging
            builder.Services.AddLogging(opt =>
            {
                opt.AddSimpleConsole(c =>
                {
                    c.TimestampFormat = "yyyy-MM-dd [HH:mm:ss] ";
                });
            });
            #endregion

            #region reverse proxy setup
            //https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-7.0
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders = ForwardedHeaders.All;
            });
            #endregion

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");

                //https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-7.0
                app.UseForwardedHeaders();

                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                //https://learn.microsoft.com/en-us/aspnet/core/host-and-deploy/proxy-load-balancer?view=aspnetcore-7.0
                app.UseForwardedHeaders();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSession();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }

        public static void ConfigureServices(IServiceCollection services)
        {
            //Dependency Injection Setup
            //https://github.com/TanvirArjel/TanvirArjel.Extensions.Microsoft.DependencyInjection
            services.AddServicesOfAllTypes();

            #region Setup Authentication/Authorization
            services.AddTransient<IUserStore<ApplicationUser>, UserStore>();
            services.AddTransient<IRoleStore<ApplicationRole>, RoleStore>();

            services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = $"/Account/Login";
                options.LogoutPath = $"/Account/Logout";
                options.AccessDeniedPath = $"/Account/AccessDenied";
            });

            #endregion

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddDefaultTokenProviders()
                .AddRoles<ApplicationRole>();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}
