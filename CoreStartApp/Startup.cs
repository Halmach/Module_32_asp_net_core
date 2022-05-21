using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreStartApp
{
    public class Startup
    {
        static IWebHostEnvironment Env;
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            Env = env;
            if (env.IsDevelopment() || env.IsStaging())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.Map("/about", About);
            app.Use(async (context, next) =>
            {
                // Для логирования данных о запросе используем свойста объекта HttpContext
                Console.WriteLine($"[{DateTime.Now}]: New request to http://{context.Request.Host.Value + context.Request.Path}");
                await next.Invoke();
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context => { await context.Response.WriteAsync($"App name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}"); });
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/config", async context =>
                {
                    await context.Response.WriteAsync($"ConfApp name: {env.ApplicationName}. App running configuration: {env.EnvironmentName}");
                });
            });

            //Добавляем компонент для логирования запросов с использованием метода Use.
            

            app.Run(async (context) =>
            {
                await context.Response.WriteAsync($"Welcome to the {env.ApplicationName}!");
            });

           
        }

        /// <summary>
        ///  Обработчик для страницы About
        /// </summary>
        private static void About(IApplicationBuilder app)
        {
            app.Run(async context =>
            {
                await context.Response.WriteAsync($"{Env.EnvironmentName} - ASP.Net Core tutorial project");
            });
        }
    }
}
