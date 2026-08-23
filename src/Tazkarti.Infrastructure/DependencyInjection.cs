using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Helper;
using System.Text;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;
using Tazkarti.Domain.Interfaces;
using Tazkarti.Infrastructure.Data;
using Tazkarti.Infrastructure.ImplmentationService;
using Tazkarti.Infrastructure.Repositories;
using TokenOptions = Shared.Helper.TokenOptions;

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

		services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	    .AddJwtBearer(option =>

	    option.TokenValidationParameters = new TokenValidationParameters
	    {
		   ValidateIssuerSigningKey = true,
		   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Token:Key"])),
		   ValidateIssuer = true,
		   ValidIssuer = configuration["Token:Issuer"],
		   ValidateAudience = false,
		   ValidateLifetime = true,
	    });

		services.Configure<TokenOptions>(configuration.GetSection("Token"));

		services.Configure<ApiBehaviorOptions>(option =>
		{
			option.InvalidModelStateResponseFactory = (actionContext) =>
			{
				var error = actionContext.ModelState.Where(p => p.Value?.Errors.Count > 0)
												.SelectMany(e => e.Value.Errors)
												.Select(m => m.ErrorMessage)
												.ToList();
				var response = new BaseResult<string>()
				{
					IsSuccess = false,
					Errors = error
				};

				return new BadRequestObjectResult(response);
			};
		});

		services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ITokenService, TokenService>();
		services.AddScoped<IAuthService, AuthService>();
		services.AddScoped<IProfileService, ProfileService>();
		

		return services;
    }
}
