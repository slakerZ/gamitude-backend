

using System;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;
using gamitude_backend.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace gamitude_backend.Extensions
{
    public static class ControllersExtension
    {
        public static void AddCustomControllersConfiguration(this IServiceCollection services)
        {
            services
                .AddControllers(o =>
                {
                    o.Filters.Add(new ProducesAttribute("application/json"));
                    o.Filters.Add(new ConsumesAttribute("application/json"));
                })
                .AddDataAnnotationsLocalization()
                .AddJsonOptions(c =>
                    {
                        c.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    })
                .ConfigureApiBehaviorOptions(options => // custom model validation error
                {
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var result = new ObjectResult(new ControllerErrorResponse
                        {
                            message = string.Join(' ', context.ModelState.Values.SelectMany(x => x.Errors.Select(x => x.ErrorMessage)).ToList())
                        });
                        result.StatusCode = (int)HttpStatusCode.BadRequest;
                        return result;
                    };
                });
        }
    }
}