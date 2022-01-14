namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxAutosuggestInput
	{
		[Parameter] public string Value { get; set; }

		[Parameter] public string Placeholder { get; set; }

		[Parameter] public EventCallback<string> OnInputInput { get; set; }

		[Parameter] public EventCallback OnInputFocus { get; set; }

		[Parameter] public EventCallback OnInputBlur { get; set; }

		[Parameter] public EventCallback OnInputMouseDown { get; set; }

		[Parameter] public string InputId { get; set; }

		[Parameter] public string CssClass { get; set; }

		[Parameter] public bool EnabledEffective { get; set; }

		/// <summary>
		/// Offset between the dropdown and the input.
		/// <see href="https://popper.js.org/docs/v2/modifiers/offset/#options"/>
		/// </summary>
		[Parameter] public (int Skidding, int Distance) DropdownOffset { get; set; }

		internal ElementReference InputElement { get; set; }

		private async Task HandleInput(ChangeEventArgs changeEventArgs)
		{
			await OnInputInput.InvokeAsync((string)changeEventArgs.Value);
		}

		public async ValueTask FocusAsync()
		{
			await InputElement.FocusAsync();
		}
	}
}
