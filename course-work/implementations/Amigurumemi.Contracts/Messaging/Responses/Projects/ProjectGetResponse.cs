using Amigurumemi.Contracts.Messaging.Requests.Projects;
using Amigurumemi.Contracts.Messaging.Responses.Shared;
using Amigurumemi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Responses.Projects
{
	public class ProjectGetResponse : BaseGetResponse<ProjectResponse>
	{
		public ProjectFilterRequest Filter { get; set; }
	}
}
