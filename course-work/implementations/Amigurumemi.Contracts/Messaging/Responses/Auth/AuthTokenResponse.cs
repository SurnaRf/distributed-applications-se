using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Responses.Auth
{
	public class AuthTokenResponse
	{
		public string Token { get; set; }
		public DateTime Expiration { get; set; }
		public int UserId { get; set; }
		public string Username { get; set; }
		public string Role { get; set; }
	}
}
