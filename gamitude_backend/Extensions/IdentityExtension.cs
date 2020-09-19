

using System;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using gamitude_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace gamitude_backend.Extensions
{
    public static class IdentityExtension
    {
        public static void AddCustomIdentity(this IServiceCollection services,String connectionString,String dbName)
        {

            services.AddIdentityMongoDbProvider<User, MongoRole>(identityOptions =>
            {
                identityOptions.Password.RequiredLength = 6;
                identityOptions.Password.RequireLowercase = false;
                identityOptions.Password.RequireUppercase = false;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireDigit = false;
            }, mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString = $"{connectionString}/{dbName}";
            }); 

        }
    }
}