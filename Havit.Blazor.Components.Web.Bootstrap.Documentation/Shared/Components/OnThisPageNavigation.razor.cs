namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

public partial class OnThisPageNavigation
{
	[Parameter] public IEnumerable<SectionTitle> Sections { get; set; }

	protected RenderFragment GenerateSectionTree() => builder =>
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
			if (currentSection.Level > currentLevel)
			{
				builder.OpenElement(sequence++, "ul");
			}
			else if (currentSection.Level < currentLevel)
			{
				builder.CloseElement();
			}
			currentLevel = currentSection.Level;

			// Render the list item and a link to the section.
			builder.OpenElement(sequence++, "li");

			builder.OpenElement(sequence++, "a");
			builder.AddAttribute(sequence++, "href", currentSection.GetSectionUrl());
			builder.AddContent(sequence++, currentSection.TitleEffective);
			builder.AddContent(sequence++, currentSection.ChildContent);
			builder.CloseElement();

			builder.CloseElement();
		}
	};
}
