using Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;
using Microsoft.AspNetCore.Components.Routing;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

public partial class OnThisPageNavigation
{
	[Inject] public ISectionTitleHolder SectionTitleHolder { get; set; }
	[Inject] public NavigationManager NavigationManager { get; set; }

	[Parameter] public string CssClass { get; set; }

	[Parameter] public RenderFragment ChildContent { get; set; }

	private IEnumerable<SectionTitle> Sections { get; set; }

	protected override void OnInitialized()
	{
		NavigationManager.LocationChanged += LoadSections;
	}

	protected override void OnAfterRender(bool firstRender)
	{
		if (firstRender)
		{
			Sections = SectionTitleHolder.RetrieveAll(NavigationManager.Uri);
			StateHasChanged();
		}
	}

	private void LoadSections(object sender, LocationChangedEventArgs eventArgs)
	{
		Sections = SectionTitleHolder.RetrieveAll(eventArgs.Location);
		StateHasChanged();
	}

	private RenderFragment GenerateSectionTree() => builder =>
	{
		if (!Sections.Any())
		{
			return;
		}

		int currentLevel = 0;

		int sequence = 1;
		for (int i = 0; i < Sections.Count(); i++)
		{
			SectionTitle currentSection = Sections.ElementAt(i);

			// Handle level adjustments - nested lists.
			int levelDifference = Math.Abs(currentSection.Level - currentLevel);
			if (currentSection.Level > currentLevel)
			{
				for (int j = 0; j < levelDifference; j++)
				{
					builder.OpenElement(sequence++, "ul");
				}
			}
			else if (currentSection.Level < currentLevel)
			{
				for (int j = 0; j < levelDifference; j++)
				{
					builder.CloseElement();
				}
			}
			currentLevel = currentSection.Level;

			// Render the list item and a link to the section.
			builder.OpenElement(sequence++, "li");

			builder.OpenElement(sequence++, "a");
			builder.AddAttribute(sequence++, "href", currentSection.GetSectionUrl());
			builder.AddAttribute(sequence++, "class", "text-secondary");
			builder.AddContent(sequence++, currentSection.TitleEffective);
			builder.AddContent(sequence++, currentSection.ChildContent);
			builder.CloseElement();

			builder.CloseElement();
		}
	};
}
