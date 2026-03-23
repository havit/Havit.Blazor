using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

[TestClass]
public class HxFilterFormTests : BunitTestBase
{
	/// <summary>
	/// Verifies that HxFilterForm renders child content (filter inputs) inside a form element.
	/// </summary>
	[TestMethod]
	public void HxFilterForm_Render_DisplaysFilterInputs()
	{
		// Arrange
		var model = new FilterModel { Name = "Test" };

		// Act
		var cut = RenderComponent<HxFilterForm<FilterModel>>(parameters => parameters
			.Add(p => p.Model, model)
			.Add(p => p.ChildContent, (FilterModel m) => (RenderTreeBuilder builder) =>
			{
				builder.OpenElement(0, "input");
				builder.AddAttribute(1, "type", "text");
				builder.AddAttribute(2, "value", m.Name);
				builder.CloseElement();
			})
		);

		// Assert
		Assert.IsNotNull(cut.Find("form"), "Form element should be rendered.");
		Assert.AreEqual("Test", cut.Find("input").GetAttribute("value"), "Input should display the model value.");
	}

	/// <summary>
	/// Verifies that submitting the filter form invokes ModelChanged with the current filter model values.
	/// </summary>
	[TestMethod]
	public async Task HxFilterForm_Submit_UpdatesFilterModel()
	{
		// Arrange
		var model = new FilterModel { Name = "Initial" };
		FilterModel updatedModel = null;

		var cut = RenderComponent<HxFilterForm<FilterModel>>(parameters => parameters
			.Add(p => p.Model, model)
			.Add(p => p.ModelChanged, (FilterModel m) => updatedModel = m)
			.Add(p => p.ChildContent, (FilterModel m) => (RenderTreeBuilder builder) =>
			{
				builder.OpenElement(0, "input");
				builder.AddAttribute(1, "type", "text");
				builder.AddAttribute(2, "value", m.Name);
				builder.CloseElement();
			})
		);

		// Act — simulate a valid form submission
		await cut.InvokeAsync(() => cut.Instance.UpdateModelAsync());

		// Assert
		Assert.IsNotNull(updatedModel, "ModelChanged should have been invoked after submission.");
		Assert.AreEqual("Initial", updatedModel.Name, "Updated model should reflect the current filter values.");
	}

	/// <summary>
	/// Verifies that providing a new default model resets the filter form to show the default values.
	/// </summary>
	[TestMethod]
	public void HxFilterForm_Reset_ClearsToDefaults()
	{
		// Arrange
		var initialModel = new FilterModel { Name = "ActiveFilter" };
		var defaultModel = new FilterModel { Name = null };

		var cut = RenderComponent<HxFilterForm<FilterModel>>(parameters => parameters
			.Add(p => p.Model, initialModel)
			.Add(p => p.ChildContent, (FilterModel m) => (RenderTreeBuilder builder) =>
			{
				builder.OpenElement(0, "span");
				builder.AddAttribute(1, "id", "filter-name");
				builder.AddContent(2, m.Name);
				builder.CloseElement();
			})
		);

		// Verify initial state
		Assert.AreEqual("ActiveFilter", cut.Find("#filter-name").TextContent, "Initial model value should be displayed.");

		// Act — reset by providing a new default model from outside
		cut.SetParametersAndRender(parameters => parameters
			.Add(p => p.Model, defaultModel)
		);

		// Assert
		Assert.AreEqual(string.Empty, cut.Find("#filter-name").TextContent, "After reset, the form should display the default (empty) values.");
	}

	private class FilterModel
	{
		public string Name { get; set; }
	}
}
