using System.Text.Json.Serialization;

namespace PSXhub.Application.Model
{
	public class GameList
	{
		[JsonPropertyName("id")]
		public int Id { get; set; }
		[JsonPropertyName("title")]
		public string? Title { get; set; }
		[JsonPropertyName("image")]
		public string Image { get; set; }
		[JsonPropertyName("region")]
		public string Region { get; set; }
		[JsonPropertyName("gameTitle")]
		public string GameTitle { get; set; }
		[JsonPropertyName("consoleTitle")]
		public string ConsoleTitle { get; set; }
	}
}
