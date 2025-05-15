using System.ComponentModel.DataAnnotations;

namespace PSXhub.Application.Entity.Game
{
	public class GamePath
	{
		public int Id { get; set; }
		[Required]
		public string GameId { get; set; }
		[Required]
		public string Title { get; set; }
		[Required]
		public string Path { get; set; }
		public string? Console { get; set; } = null;
	}
}
