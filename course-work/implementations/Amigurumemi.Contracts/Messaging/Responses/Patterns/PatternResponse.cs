using Amigurumemi.Data.Entities;

namespace Amigurumemi.Contracts.Messaging.Responses.Patterns
{
	public class PatternResponse
	{
		public int Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public DifficultyLevel Difficulty { get; set; }

		public string Category { get; set; }

		public decimal Price { get; set; }

		public bool IsPremium { get; set; }

		public int EstimatedHours { get; set; }

		public DateTime PublishedOn { get; set; }

		public int UserId { get; set; }
	}
}