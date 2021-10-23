using BaseProtocol;
using BaseProtocol.Extenstion;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DynamicAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var controllerBuilder = services.AddControllers();
            
            controllerBuilder.ConfigureApplicationPartManager(apm =>
            {
                foreach (var dll in System.IO.Directory.GetFiles(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Dll")))
                {
                    var assembly = Assembly.LoadFile(dll);
                    foreach (var type in assembly.GetExportedTypes().Where(t => t.IsClass && t.IsSubclassOf(typeof(ServiceBase))))
                    {
                        services.AddDynamicService(type);
                    }
                    var part = new AssemblyPart(assembly);
                    apm.ApplicationParts.Add(part);
                }
            });
            foreach (var type in Assembly.GetExecutingAssembly().GetExportedTypes().Where(t => t.IsClass && t.IsSubclassOf(typeof(ServiceBase))))
            {
                services.AddDynamicService(type);
            }
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DynamicAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "DynamicAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
