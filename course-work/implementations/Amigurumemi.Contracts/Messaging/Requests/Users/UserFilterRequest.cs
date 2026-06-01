using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Users
{
	public class UserFilterRequest
	{
		public string? Username { get; set; }

		public string? FirstName { get; set; }

		public string? LastName { get; set; }

		public string? Email { get; set; }

	}
}
