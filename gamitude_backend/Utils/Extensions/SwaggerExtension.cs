using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using gamitude_backend.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace gamitude_backend.Extensions
{
    public static class SwaggerExtension
    {
        public static void UseCustomSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelsExpandDepth(0);
                c.SwaggerEndpoint("api/swagger/v2/swagger.json", "Gamitude V2");
                c.RoutePrefix = "api/swagger";
            });
        }
        public static void AddCustomSwaggerConfig(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.UseAllOfToExtendReferenceSchemas();
                c.OperationFilter<AuthResponsesOperationFilter>();
                c.SwaggerDoc("v2", new OpenApiInfo { Title = "Gamitude API", Version = "v2" });
                c.SchemaFilter<SwaggerSchemaFilter>();
                c.DocumentFilter<CustomModelDocumentFilter<ControllerErrorResponse>>();

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme.
                      Enter 'Bearer' [space] and then your token in the text input below.
                      Example: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {{new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        },
                        Scheme = "oauth2",
                        Name = "Bearer",
                        In = ParameterLocation.Header,

                        },
                        new List<string>()
                }});

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }
    }

    public class AuthResponsesOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var schema = new KeyValuePair<string, OpenApiMediaType>
            (
                "application/json",
                new OpenApiMediaType
                {
                    Schema = new OpenApiSchema
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.Schema,
                            Id = nameof(ControllerErrorResponse)
                        }
                    }
                }
            );

            var authAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<AuthorizeAttribute>();
            var getAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<HttpGetAttribute>();
            var postAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<HttpPostAttribute>();
            var putAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<HttpPutAttribute>();
            var deleteAttributes = context.MethodInfo.DeclaringType.GetCustomAttributes(true)
                .Union(context.MethodInfo.GetCustomAttributes(true))
                .OfType<HttpDeleteAttribute>();
            var apiParameters = context.ApiDescription.ParameterDescriptions;

            // Log.Information(context.MethodInfo.Name); TODO add new return codes 
            if (getAttributes.Any())
            {
                if(apiParameters.Count > 0)
                operation.Responses.Add("404", new OpenApiResponse
                {
                    Description = "Not Found",
                    Content = { schema }
                });
            }

            if (postAttributes.Any())
            {
                operation.Responses.Add("201", new OpenApiResponse
                {
                    Description = "Created",
                    Content = { schema }
                });
            }

            if (deleteAttributes.Any())
            {
                operation.Responses.Add("204", new OpenApiResponse
                {
                    Description = "Success No Content Deleted",
                    Content = { schema }
                });
            }

            if (authAttributes.Any())
            {
                operation.Responses.Add("401", new OpenApiResponse
                {
                    Description = "Unauthorized",
                    Content = { schema }
                });
            }

            if (apiParameters.Count > 0)
            {
                operation.Responses.Add("400", new OpenApiResponse
                {
                    Description = "Bad Request",
                    Content = { schema }
                });
            }

        }
    }
    public class CustomModelDocumentFilter<T> : IDocumentFilter where T : class
    {
        public void Apply(OpenApiDocument openapiDoc, DocumentFilterContext context)
        {
            context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
        }
    }
    public class SwaggerSchemaFilter : ISchemaFilter
    {
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            // var type = context.Type;
            // if (type.IsEnum)
            // {
            //     schema.Extensions.Add(
            //         "x-ms-enum",
            //         new OpenApiObject
            //         {
            //             ["name"] = new OpenApiString(type.Name),
            //             ["modelAsString"] = new OpenApiBoolean(true)
            //         }
            //     );
            // };

            // var prefix = "Dto";
            // foreach (var key in context.SchemaRepository.Schemas.Keys)
            // {
            //     if (!key.EndsWith(prefix))
            //     {
            //         context.SchemaRepository.Schemas.Remove(key);
            //     }
            // }
        }
    }
}
