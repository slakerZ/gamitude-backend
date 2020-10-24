using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using gamitude_backend.Extensions;
using gamitude_backend.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace gamitude_backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public IConfiguration configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ReadSettings(configuration);
            var dbSettings = configuration.GetSection(nameof(DatabaseSettings)).Get<DatabaseSettings>();

            Log.Debug(dbSettings.connectionString);

            services.addDatabase(dbSettings);
            services.AddCustomIdentity(dbSettings.connectionString,
                                                dbSettings.databaseName);

            var key = Encoding.ASCII.GetBytes(configuration.GetSection(nameof(JwtSettings)).Get<JwtSettings>().secret);
            services.AddCustomAuthenticationConfiguration(key);

            services.AddServices();
            services.AddCustomControllersConfiguration();
            services.AddCustomLocalizationConfiguration();
            services.AddCustomSwaggerConfig();
            services.AddAutoMapper(typeof(Startup));
            services.AddHttpContextAccessor();
            services.AddRouting(c => c.LowercaseUrls = true); // all routing to lowerCase
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            // app.UseCustomLocalization();
            app.UseStaticFiles();
            // app.UseHttpsRedirection();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseCustomSwagger();
            app.UseAuthentication();
            app.UseRouting();
            app.UseCustomExceptionMiddleware();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
