namespace Havit.Blazor.Documentation.Pages.Showcase.Data;

public class ShowcaseDataService : IShowcaseDataService
{
	private readonly List<ShowcaseModel> _data = new()
	{
		new()
		{
			Id = "goran",
			Title = "Goran",
			Description = "SaaS ERP for small businesses",
			ImageUrl = "images/showcase/Goran.png",
			Author = "HAVIT",
			AuthorUrl = "https://www.havit.eu",
			ProjectUrl = "https://havit-g3.goran.cz",
			SourceCodeTitle = "GitHub (Premium)",
			SourceCodeUrl = "premium/access-content?url=https%3A%2F%2Fgithub.com%2Fhavit%2FGoranG3"
		},
		new()
		{
			Id = "bety",
			Title = "Bety",
			Description = "Broker pool CRM",
			ImageUrl = "images/showcase/Bety.png",
			Author = "HAVIT",
			AuthorUrl = "https://www.havit.eu",
			ProjectUrl = "https://bety2.brokertrust.cz",
		},
		new()
		{
			Id = "autronic",
			Title = "Autronic",
			Description = "B2B furniture e-shop",
			ImageUrl = "images/showcase/Autronic.png",
			Author = "HAVIT",
			AuthorUrl = "https://www.havit.eu",
			ProjectUrl = "https://www.autronic.cz",
		},
		new()
		{
			Id = "patria-web-trader",
			Title = "Web Trader",
			Description = "Trading platform",
			ImageUrl = "images/showcase/Patria.png",
			Author = "Patria + HAVIT",
		},
		new()
		{
			Id = "edenred",
			Title = "Edenred",
			Description = "Fintech card management",
			ImageUrl = "images/showcase/Edenred.png",
			Author = "HAVIT",
			AuthorUrl = "https://www.havit.eu",
		},
		new()
		{
			Id = "zachranka",
			Title = "NG-SOS",
			Description = "Emergency communication platform",
			ImageUrl = "images/showcase/Ngsos.jpg",
			Author = "NG-SOS",
			AuthorUrl = "https://ng-sos.com",
		},
		new()
		{
			Id = "skdoatube",
			Title = "SkodaTube",
			Description = "Digital media platform",
			ImageUrl = "images/showcase/SkodaTube.png",
			Author = "HAVIT",
			AuthorUrl = "https://www.havit.eu",
		},
		new()
		{
			Id = "tulipHeron",
			Title = "TULIP Heron",
			Description = "Attendance system",
			ImageUrl = "images/showcase/TulipHeron.png",
			Author = "Tulipize + HAVIT",
			AuthorUrl = "https://tulipize.com/heron-attendance/",
		},

	};

	public ShowcaseModel GetShowcase(string key)
	{
		var showcase = _data.FirstOrDefault(s => s.Id == key);
		if (showcase == null)
		{
			throw new ArgumentException($"Showcase with key '{key}' not found.");
		}
		return showcase;
	}

	public IEnumerable<ShowcaseModel> GetShowcases()
	{
		return _data;
	}

	public ShowcaseModel GetPreviousShowcase(string currentKey)
	{
		var currentIndex = _data.FindIndex(s => s.Id == currentKey);
		if (currentIndex == -1)
		{
			throw new ArgumentException($"Showcase with key '{currentKey}' not found.");
		}
		var previousIndex = (currentIndex - 1 + _data.Count) % _data.Count;
		return _data[previousIndex];
	}

	public ShowcaseModel GetNextShowcase(string currentKey)
	{
		var currentIndex = _data.FindIndex(s => s.Id == currentKey);
		if (currentIndex == -1)
		{
			throw new ArgumentException($"Showcase with key '{currentKey}' not found.");
		}
		var nextIndex = (currentIndex + 1) % _data.Count;
		return _data[nextIndex];
	}
}
