using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using gamitude_backend.Data;
using gamitude_backend.Dto;
using gamitude_backend.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace gamitude_backend.Extensions
{
    public static class DatabaseExtension
    {
       public static void addDatabase(this IServiceCollection services, IDatabaseSettings databaseSettings)
       {
           services.AddSingleton<IDatabaseCollections>(new MongoCollections(databaseSettings));
       } 
    }
}