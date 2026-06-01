using Amigurumemi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Projects
{
	public class ProjectFilterRequest
	{
		public string? Name { get; set; }

		public ProjectStatus? Status { get; set; }

		public int? PatternId { get; set; }

		public int? UserId { get; set; }
	}
}
