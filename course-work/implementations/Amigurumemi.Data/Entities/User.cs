using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Data.Entities
{
	public class User : BaseEntity
	{
		public string Username { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string PasswordHash { get; set; }

		public DateTime RegistrationDate { get; set; }

		public string Role { get; set; }

	}
}
