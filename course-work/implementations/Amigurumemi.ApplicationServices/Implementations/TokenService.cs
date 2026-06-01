using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Amigurumemi.ApplicationServices.Interfaces;
using Amigurumemi.Data.Entities;
using Microsoft.IdentityModel.Tokens;

namespace Amigurumemi.ApplicationServices.Implementations
{
	public class TokenService : ITokenService
	{
		public string CreateToken(User user)
		{
			var claims = new[]
			{
				new Claim("userId", user.Id.ToString()),
				new Claim(ClaimTypes.Role, user.Role),
				new Claim(ClaimTypes.Name, user.Username)
			};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SUPER_SECRET_KEY_CHANGE_ME_123456"));

			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: "Amigurumemi",
				audience: "AmigurumemiClient",
				claims: claims,
				expires: DateTime.UtcNow.AddHours(1),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}