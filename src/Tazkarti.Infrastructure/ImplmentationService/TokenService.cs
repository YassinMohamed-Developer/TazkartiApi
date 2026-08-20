using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Helper;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tazkarti.Application.Interfaces;
using Tazkarti.Domain.Entities;

namespace Tazkarti.Infrastructure.ImplmentationService
{
	public class TokenService : ITokenService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IOptions<Shared.Helper.TokenOptions> _options;
		private readonly SymmetricSecurityKey _Key;
		public TokenService(UserManager<AppUser> userManager,IOptions<Shared.Helper.TokenOptions> options)
		{
			_userManager = userManager;
			_options = options;
			_Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.Key));
		}
		public async Task<string> GenerateToken(AppUser appUser)
		{
			var roles = await _userManager.GetRolesAsync(appUser);

			var claims = new List<Claim>
			{
				new Claim("UserId",appUser.Id),
				new Claim("UserName",appUser.UserName),
			};

			foreach (var role in roles)
			{
				claims.Add(new Claim(ClaimTypes.Role, role));
			}

			var credential = new SigningCredentials(_Key, SecurityAlgorithms.HmacSha256);


			var TokenDescribe = new SecurityTokenDescriptor
			{
				SigningCredentials = credential,
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddHours(1),
				Issuer = _options.Value.Issuer,
				IssuedAt = DateTime.Now,
			};

			var handler = new JwtSecurityTokenHandler();

			var token = handler.CreateToken(TokenDescribe);

			return handler.WriteToken(token);
		}
	}
}
