namespace Havit.Blazor.Components.Web.Bootstrap.Internal;

public partial class HxAutosuggestInputInternal
{
	[Parameter] public string Value { get; set; }

	[Parameter] public string Placeholder { get; set; }

	[Parameter] public EventCallback<string> OnInput { get; set; }

	[Parameter] public EventCallback OnFocus { get; set; }

	[Parameter] public EventCallback OnBlur { get; set; }

	[Parameter] public EventCallback OnMouseDown { get; set; }

	[Parameter] public EventCallback OnClick { get; set; }

	[Parameter] public string InputId { get; set; }

	[Parameter] public string CssClass { get; set; }

	[Parameter] public bool EnabledEffective { get; set; }

	[Parameter] public bool? SpellcheckEffective { get; set; }

	/// <summary>
	/// Offset between the dropdown and the input.
	/// <see href="https://popper.js.org/docs/v2/modifiers/offset/#options"/>
	/// </summary>
	[Parameter] public (int Skidding, int Distance) DropdownOffset { get; set; }

	[Parameter] public string NameAttributeValue { get; set; }

	/// <summary>
	/// Additional attributes to be splatted onto an underlying HTML element.
	/// </summary>
	[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

	internal ElementReference InputElement { get; set; }

	private async Task HandleInput(ChangeEventArgs changeEventArgs)
	{
		await OnInput.InvokeAsync((string)changeEventArgs.Value);
	}

	public async ValueTask FocusAsync()
	{
		await InputElement.FocusAsync();
	}
}
