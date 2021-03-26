using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Displays a list of validation messages for a specified field within a cascaded <see cref="EditContext"/>.
	/// </summary>
	public class HxValidationMessage<TValue> : ComponentBase, IDisposable
	{
		private EditContext previousEditContext;
		private Expression<Func<TValue>> previousFieldAccessor;
		private readonly EventHandler<ValidationStateChangedEventArgs> validationStateChangedHandler;
		private FieldIdentifier fieldIdentifier;
		private EditContext currentEditContext;

		/// <summary>
		/// Cascading EditContext.
		/// </summary>
		[CascadingParameter] protected EditContext CascadingEditContext { get; set; }

		/// <summary>
		/// EditContext. For exceptional use where EditContext is not used as a CascadingParameter.
		/// </summary>
		[Parameter] public EditContext EditContext { get; set; }

		/// <summary>
		/// Specifies the field for which validation messages should be displayed.
		/// </summary>
		[Parameter] public Expression<Func<TValue>> For { get; set; }

		/// <summary>
		/// Constructs an instance of <see cref="HxValidationMessage{TValue}"/>.
		/// </summary>
		public HxValidationMessage()
		{
			validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
		}

		/// <inheritdoc />
		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			currentEditContext = CascadingEditContext ?? EditContext;
			if (currentEditContext == null)
			{
				throw new InvalidOperationException($"{GetType()} requires a cascading parameter of type {nameof(Microsoft.AspNetCore.Components.Forms.EditContext)} or {nameof(EditContext)} property set. Use {GetType()} inside an {nameof(EditForm)}.");
			}

			if (For == null) // Not possible except if you manually specify T
			{
				throw new InvalidOperationException($"{GetType()} requires a value for the {nameof(For)} parameter.");
			}
			else if (For != previousFieldAccessor)
			{
				fieldIdentifier = FieldIdentifier.Create(For);
				previousFieldAccessor = For;
			}

			if (currentEditContext != previousEditContext)
			{
				DetachValidationStateChangedListener();
				currentEditContext.OnValidationStateChanged += validationStateChangedHandler;
				previousEditContext = currentEditContext;
			}
		}

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			List<string> messages = currentEditContext.GetValidationMessages(fieldIdentifier).ToList();

			if (messages.Any())
			{
				builder.OpenElement(0, "div");
				builder.AddAttribute(1, "class", "invalid-tooltip");

				foreach (string message in messages)
				{
					builder.OpenElement(2, "span");
					builder.AddContent(3, message);
					builder.CloseElement();
				}

				builder.CloseElement();
			}

		}

		/// <inheritdoc />
		void IDisposable.Dispose()
		{
			DetachValidationStateChangedListener();
		}

		private void DetachValidationStateChangedListener()
		{
			if (previousEditContext != null)
			{
				previousEditContext.OnValidationStateChanged -= validationStateChangedHandler;
			}
		}
	}
}
