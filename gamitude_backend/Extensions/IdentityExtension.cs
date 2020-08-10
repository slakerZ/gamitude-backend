

using gamitude_backend.Data;
using gamitude_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace gamitude_backend.Extensions
{
    public static class IdentityExtension
    {
        public static void AddCustomIdentity(this IServiceCollection services)
        {
            services.AddIdentity<User, IdentityRole>(o =>
                {
                    o.Password.RequireUppercase = false;
                    o.Password.RequireNonAlphanumeric = false;
                    // o.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<DataContext>();
        }
    }
}