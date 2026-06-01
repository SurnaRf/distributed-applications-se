using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Auth
{
	public class AuthTokenRequest
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}
