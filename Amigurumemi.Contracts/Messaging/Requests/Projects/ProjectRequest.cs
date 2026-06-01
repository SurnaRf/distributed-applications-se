using Amigurumemi.Data.Entities;

public class ProjectRequest
{
	public string Name { get; set; }

	public int PatternId { get; set; }

	public int UserId { get; set; }

	public ProjectStatus Status { get; set; }

	public int Progress { get; set; }

	public DateTime StartDate { get; set; }

	public string Notes { get; set; }

	public decimal EstimatedCost { get; set; }

	public bool IsFavorite { get; set; }

	public List<int> SelectedYarnIds { get; set; } = new List<int>();
}