using Application.Abstractions;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(opt =>
                opt.UseInMemoryDatabase("vaccination-db"));
            services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());
            return services;
        }
    }
}
