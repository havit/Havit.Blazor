namespace Havit.Blazor.Components.Web.Bootstrap.Internal
{
	public partial class HxInputTagsAutosuggestInput
	{
		[Parameter] public string Value { get; set; }

		[Parameter] public string Placeholder { get; set; }

		[Parameter] public EventCallback<string> OnInputInput { get; set; }

		[Parameter] public EventCallback OnInputFocus { get; set; }

		[Parameter] public EventCallback OnInputBlur { get; set; }

		[Parameter] public EventCallback OnInputMouseDown { get; set; }

		[Parameter] public EventCallback<KeyboardEventArgs> OnInputKeyDown { get; set; }

		[Parameter] public string InputId { get; set; }

		[Parameter] public string CssClass { get; set; }

		[Parameter] public bool EnabledEffective { get; set; }

		/// <summary>
		/// Offset between dropdown input and dropdown menu
		/// </summary>
		[Parameter] public (int X, int Y) Offset { get; set; }

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
