using AspNetCoreAuthenAndAuthoWithJwt.Context;
using AspNetCoreAuthenAndAuthoWithJwt.Services.Implements;
using AspNetCoreAuthenAndAuthoWithJwt.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AspNetCoreAuthenAndAuthoWithJwt.Extensions
{
    public static class ApplicationServiceExtenions
    {

        public static IServiceCollection AddApplicatioinServices(this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<ITokenService, TokenService>();

            return services;
        }

    }
}
