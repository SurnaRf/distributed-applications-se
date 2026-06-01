using Amigurumemi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Patterns
{
	public class PatternFilterRequest
	{
		public string? Name { get; set; }

		public DifficultyLevel? Difficulty { get; set; }

		public string? Category { get; set; }

		public int? UserId { get; set; }
	}
}
