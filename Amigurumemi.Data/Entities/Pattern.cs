using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Data.Entities
{
	public class Pattern : BaseEntity
	{
		public string Name { get; set; }
		
		public string Category { get; set; }

		public DifficultyLevel Difficulty { get; set; }

		public string Description { get; set; }

		public decimal Price { get; set; }

		public int EstimatedHours { get; set; }

		public DateTime PublishedOn { get; set; }

		public bool IsPremium { get; set; }

		public int UserId { get; set; }

	}
}
