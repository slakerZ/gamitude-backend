using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace gamitude_backend.Extensions
{
    public static class AuthorizationExtension
    {
        public static void AddCustomAuthorizationConfiguration(this IServiceCollection services)
        {
            // services.AddAuthorization(c =>
            // {
            //     c.AddPolicy(
            //         "DbTokenPolicy",
            //         policy => policy.Requirements.Add(new DbTokenRequirement { }));
            //     c.AddPolicy(
            //         "RbacPolicy",
            //         policy => policy.Requirements.Add(new RbacRequirement { }));
            // });
            // services.AddScoped<IAuthorizationHandler, DbTokenHandler>();
            // services.AddScoped<IAuthorizationHandler, RbacHandler>();
        }
    }
}