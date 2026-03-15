using Havit.Blazor.Documentation.Pages.Showcase.Data;

namespace Havit.Blazor.Documentation.Pages.Showcase;

public partial class ShowcaseDetail(
	IShowcaseDataService showcaseDataService)
{
	[Parameter] public string Id { get; set; }

	private readonly IShowcaseDataService _showcaseDataService = showcaseDataService;

	private ShowcaseModel _showcase;
	private ShowcaseModel _previousShowcase;
	private ShowcaseModel _nextShowcase;

	protected override void OnParametersSet()
	{
		if (_showcase?.Id != Id)
		{
			_showcase = _showcaseDataService.GetShowcase(Id);
			_previousShowcase = _showcaseDataService.GetPreviousShowcase(Id);
			_nextShowcase = _showcaseDataService.GetNextShowcase(Id);
		}
	}
}