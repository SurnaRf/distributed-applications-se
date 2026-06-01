using Amigurumemi.Contracts.Messaging.Requests.Users;
using Amigurumemi.Contracts.Messaging.Responses.Shared;
using Amigurumemi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Responses.Users
{
	public class UsersGetResponse : BaseGetResponse<UserResponse>
	{
		public UserFilterRequest Filter { get; set; }

	}

}
