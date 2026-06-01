using Amigurumemi.Data;
using Amigurumemi.Data.Entities;
using Amigurumemi.ApplicationServices.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Amigurumemi.API.Extensions
{
	public static class DbContextSeed
	{
		public static void SeedAdminUser(this IServiceProvider services)
		{
			using var scope = services.CreateScope();

			var db = scope.ServiceProvider.GetRequiredService<AmigurumemiDbContext>();
			var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

			if (!db.Users.Any(u => u.Username == "admin"))
			{
				var admin = new User
				{
					Username = "admin",
					Email = "admin@amigurumemi.com",
					FirstName = "System",
					LastName = "Admin",
					Role = "Admin",
					RegistrationDate = DateTime.UtcNow
				};

				admin.PasswordHash = passwordService.HashPassword(admin, "Admin123!");

				db.Users.Add(admin);
				db.SaveChanges();
			}
		}
	}
}
