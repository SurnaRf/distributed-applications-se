using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Data.Entities
{
	public class Project : BaseEntity
	{
		public string Name { get; set; }

		public string Notes { get; set; }

		public int Progress { get; set; }

		public ProjectStatus Status { get; set; }

		public DateTime StartDate { get; set; }

		public DateTime? FinishDate { get; set; }

		public decimal EstimatedCost { get; set; }

		public bool IsFavorite { get; set; }

		public int UserId { get; set; }

		public int PatternId { get; set; }
	}
}
