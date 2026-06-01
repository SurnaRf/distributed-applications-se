using Amigurumemi.Data.Entities;

namespace Amigurumemi.Contracts.Messaging.Requests.Pattern
{
	public class PatternRequest
	{
		public string Name { get; set; }

		public string Description { get; set; }

		public DifficultyLevel Difficulty { get; set; }

		public string Category { get; set; }

		public decimal Price { get; set; }

		public int EstimatedHours { get; set; }

		public int UserId { get; set; }
	}
}