using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace gamitude_backend.Extensions
{
    public static class LocalizationExtension
    {
        public static void AddCustomLocalizationConfiguration(this IServiceCollection services)
        {
            services.AddLocalization(c => { c.ResourcesPath = "Resources"; });
            services.Configure<RequestLocalizationOptions>(c =>
            {
                var supportedCultures = new List<CultureInfo>
                {
                    new CultureInfo("en"),
                    new CultureInfo("pl"),
                };
                c.DefaultRequestCulture = new RequestCulture("pl");
                // Formatting numbers, dates, etc.
                c.SupportedCultures = supportedCultures;
                // UI strings that we have localized.
                c.SupportedUICultures = supportedCultures;
            });
        }
        public static void UseCustomLocalization(this IApplicationBuilder app)
        {
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);
        }
    }
}