using Amigurumemi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Responses.Projects
{
	public class ProjectResponse
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string? Notes { get; set; }

		public ProjectStatus Status { get; set; }

		public int Progress { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		public decimal EstimatedCost { get; set; }

		public bool IsFavorite { get; set; }

		public int UserId { get; set; }

		public int PatternId { get; set; }

		public string PatternName { get; set; }

		public List<string> YarnNames { get; set; } = new List<string>();

		public List<int> YarnIds { get; set; } = new List<int>();
	}
}
