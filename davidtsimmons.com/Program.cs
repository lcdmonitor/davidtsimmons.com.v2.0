using StackExchange.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.DataProtection;
using Services.Repositories;

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

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
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

        public static void ConfigureServices(IServiceCollection Services)
        {
            Services.AddSingleton<ITestRepository, TestRepository>();
        }
    }
}
