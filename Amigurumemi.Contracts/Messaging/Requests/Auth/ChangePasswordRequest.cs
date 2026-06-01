using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Auth
{
	public class ChangePasswordRequest
	{
		public int UserId { get; set; }
		public string CurrentPassword { get; set; } = string.Empty;
		public string NewPassword { get; set; } = string.Empty;
	}
}
