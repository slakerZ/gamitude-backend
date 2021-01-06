

using System;
using AspNetCore.Identity.Mongo;
using AspNetCore.Identity.Mongo.Model;
using gamitude_backend.Models;
using gamitude_backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace gamitude_backend.Extensions
{
    public static class IdentityExtension
    {
        public static void AddCustomIdentity(this IServiceCollection services, string connectionString, string dbName)
        {
            services.Configure<DataProtectionTokenProviderOptions>(o =>
               o.TokenLifespan = TimeSpan.FromHours(3));
            services.AddIdentityMongoDbProvider<User, MongoRole>(identityOptions =>
            {
                identityOptions.Tokens.ProviderMap.Add(
                    "GamitudeEmailConfirmation",
                    new TokenProviderDescriptor(typeof(GamitudeEmailConfirmationTokenProvider<User>)));
                identityOptions.Password.RequiredLength = 8;
                identityOptions.Password.RequireLowercase = true;
                identityOptions.Password.RequireUppercase = true;
                identityOptions.Password.RequireNonAlphanumeric = false;
                identityOptions.Password.RequireDigit = true;
                identityOptions.User.RequireUniqueEmail = true;
            }, mongoIdentityOptions =>
            {
                mongoIdentityOptions.ConnectionString = connectionString;
            });
            services.AddTransient<GamitudeEmailConfirmationTokenProvider<User>>();
            

        }
    }
}