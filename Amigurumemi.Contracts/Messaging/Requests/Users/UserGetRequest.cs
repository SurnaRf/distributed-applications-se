using Amigurumemi.Contracts.Messaging.Requests.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Users
{
	public class UserGetRequest : BaseGetRequest
	{
		public UserFilterRequest? Filter { get; set; }
	}
}
