using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Responses.Users
{
	public class UserResponse
	{
		public int Id { get; set; }

		public string Username { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Email { get; set; }

		public string Role { get; set; }

		public DateTime RegistrationDate { get; set; }
	}
}
