namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// <see href="https://getbootstrap.com/docs/5.0/forms/floating-labels/#textareas" target="_blank">Textarea</see>.
	/// To set a custom height, do not use the rows attribute. Instead, set an explicit height (either inline or via custom CSS).
	/// </summary>
	public class HxInputTextArea : HxInputText
	{
		/// <inheritdoc />
		private protected override string GetElementName() => "textarea";

		/// <inheritdoc />
		private protected override string GetTypeAttributeValue() => null;
	}
}
