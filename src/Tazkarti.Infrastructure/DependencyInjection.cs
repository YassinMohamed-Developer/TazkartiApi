using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;
using Tazkarti.Domain.Interfaces;
using Tazkarti.Infrastructure.Data;
using Tazkarti.Infrastructure.Repositories;

namespace Tazkarti.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") 
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' was not found.");

        services.AddDbContext<TazkartiDbContext>(options =>
            options.UseSqlServer(connectionString, b => b.MigrationsAssembly(typeof(TazkartiDbContext).Assembly.FullName)));

        services.AddDataProtection();

        services.AddIdentityCore<AppUser>().AddRoles<IdentityRole>()
        .AddEntityFrameworkStores<TazkartiDbContext>()
        .AddSignInManager<SignInManager<AppUser>>()
        .AddRoleManager<RoleManager<IdentityRole>>()
        .AddDefaultTokenProviders();

        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}
