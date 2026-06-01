using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Responses.Users
{
	public class UserStatsResponse
	{
		public int UserId { get; set; }

		public int TotalProjects { get; set; }

		public int ActiveProjects { get; set; }

		public int CompletedProjects { get; set; }

		public int PatternsCreated { get; set; }
	}
}
