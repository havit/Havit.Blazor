using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Text-based (string) input base class.
	/// </summary>
	public abstract class HxInputTextBase : HxInputBaseWithInputGroups<string>, IInputWithSize, IInputWithPlaceholder, IInputWithLabelType
	{
		/// <summary>
		/// Set of settings to be applied to the component instance (overrides <see cref="HxInputText.Defaults"/>, overriden by individual parameters).
		/// </summary>
		[Parameter] public InputTextSettings Settings { get; set; }

		/// <summary>
		/// Return <see cref="HxInputText"/> defaults.
		/// Enables to not share defaults in descandants with base classes.
		/// Enables to have multiple descendants which differs in the default values.
		/// </summary>
		protected abstract InputTextSettings GetDefaults();
		IInputSettingsWithSize IInputWithSize.GetDefaults() => GetDefaults(); // might be replaced with C# vNext convariant return types on interfaces
		IInputSettingsWithSize IInputWithSize.GetSettings() => this.Settings;

		/// <summary>
		/// Hint to browsers as to the type of virtual keyboard configuration to use when editing.<br/>
		/// Default is <c>null</c> (not set).
		/// </summary>
		[Parameter] public InputMode? InputMode { get; set; }
		protected InputMode? InputModeEffective => this.InputMode ?? this.Settings?.InputMode ?? this.GetDefaults()?.InputMode;

		/// <summary>
		/// Gets or sets the behavior when the model is updated from then input.
		/// </summary>
		[Parameter] public BindEvent BindEvent { get; set; } = BindEvent.OnChange;

		/// <summary>
		/// Placeholder for the input.
		/// </summary>
		[Parameter] public string Placeholder { get; set; }

		/// <inheritdoc cref="Bootstrap.InputSize" />
		[Parameter] public InputSize? InputSize { get; set; }

		/// <inheritdoc cref="Bootstrap.LabelType" />
		[Parameter] public LabelType? LabelType { get; set; }

		/// <inheritdoc />
		protected override void BuildRenderInput(RenderTreeBuilder builder)
		{
			builder.OpenElement(0, GetElementName());
			BuildRenderInput_AddCommonAttributes(builder, GetTypeAttributeValue());

			MaxLengthAttribute maxLengthAttribute = GetValueAttribute<MaxLengthAttribute>();
			if ((maxLengthAttribute != null) && (maxLengthAttribute.Length > 0))
			{
				builder.AddAttribute(1000, "maxlength", maxLengthAttribute.Length);
			}

			builder.AddAttribute(1002, "value", FormatValueAsString(Value));
			builder.AddAttribute(1003, BindEvent.ToEventName(), EventCallback.Factory.CreateBinder<string>(this, value => CurrentValueAsString = value, CurrentValueAsString));

			if (this.InputModeEffective is not null)
			{
				builder.AddAttribute(1004, "inputmode", this.InputModeEffective.Value.ToString("f").ToLower());
			}

			builder.AddEventStopPropagationAttribute(1004, "onclick", true);
			builder.AddElementReferenceCapture(1005, elementReferece => InputElement = elementReferece);

			builder.CloseElement();
		}

		/// <summary>
		/// Returns element name to render.
		/// </summary>
		private protected abstract string GetElementName();

		/// <summary>
		/// Returns type attribute value.
		/// </summary>
		private protected abstract string GetTypeAttributeValue();

		/// <inheritdoc />
		protected override bool TryParseValueFromString(string value, out string result, out string validationErrorMessage)
		{
			result = value;
			validationErrorMessage = null;
			return true;
		}
	}
}
