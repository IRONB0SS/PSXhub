namespace PSXhub.Application.Model
{
	public class Language
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string Symbol { get; set; }

		public static List<Language> DefaultLanguages { get; } = new List<Language>
		{
			new Language { Id = 1, Title = "English", Symbol = "en" },
			new Language { Id = 2, Title = "فارسی", Symbol = "fa" }
		};

		public static Language GetById(int id)
		{
			return DefaultLanguages.SingleOrDefault(lang => lang.Id == id)!;
		}
	}
}
