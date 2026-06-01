using Amigurumemi.Contracts.Messaging.Requests.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Projects
{
	public class ProjectGetRequest : BaseGetRequest
	{
		public ProjectFilterRequest? Filter { get; set; }
	}
}
