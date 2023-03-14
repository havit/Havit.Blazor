using System.Reflection;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A base class for form input components. This base class automatically integrates
/// with an Microsoft.AspNetCore.Components.Forms.EditContext, which must be supplied
/// as a cascading parameter.
/// Extends <see cref="InputBase{TValue}"/> class.
/// 
/// Adds support for rendering bootstrap based input with validator.
/// See also <see href="https://getbootstrap.com/docs/5.2/forms/overview/">https://getbootstrap.com/docs/5.2/forms/overview/</see>.
/// </summary>
public abstract class HxInputBase<TValue> : InputBase<TValue>, ICascadeEnabledComponent, IFormValueComponent
{
	/// <summary>
	/// CSS class used for invalid input.
	/// </summary>
	public const string InvalidCssClass = "is-invalid";

	/// <summary>
	/// Return <see cref="HxInputBase{TValue}"/> defaults.
	/// Enables to not share defaults in descandants with base classes.
	/// Enables to have multiple descendants which differs in the default values.
	/// </summary>
	protected virtual InputSettings GetDefaults() => HxInputBase.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxInputBase.Defaults"/>, overriden by individual parameters).
	/// </summary>
	/// <remarks>
	/// Using interface does not force the implementation of settings to use specific class as a base type.</remarks>
	protected abstract InputSettings GetSettings();

	/// <summary>
	/// Specifies how the validation message should be displayed.<br/>
	/// Default is <see cref="ValidationMessageMode.Regular"/>, you can override application-wide default for all inputs in <see cref="HxInputBase.Defaults"/>.
	/// </summary>
	[Parameter] public ValidationMessageMode? ValidationMessageMode { get; set; }
	protected ValidationMessageMode ValidationMessageModeEffective => this.ValidationMessageMode ?? this.GetSettings()?.ValidationMessageMode ?? GetDefaults().ValidationMessageMode ?? HxInputBase.Defaults?.ValidationMessageMode ?? throw new InvalidOperationException(nameof(ValidationMessageMode) + " default for " + nameof(HxInputBase<TValue>) + " has to be set.");

	/// <inheritdoc cref="Web.FormState" />
	[CascadingParameter] protected FormState FormState { get; set; }
	FormState ICascadeEnabledComponent.FormState { get => this.FormState; set => this.FormState = value; }

	#region IFormValueComponent public properties
	/// <summary>
	/// Label text.
	/// </summary>
	[Parameter] public string Label { get; set; }

	/// <summary>
	/// Label content.
	/// </summary>
	[Parameter] public RenderFragment LabelTemplate { get; set; }

	/// <summary>
	/// Hint to render after input as form-text.
	/// </summary>
	[Parameter] public string Hint { get; set; }

	/// <summary>
	/// Hint to render after input as form-text.
	/// </summary>
	[Parameter] public RenderFragment HintTemplate { get; set; }

	/// <summary>
	/// Custom CSS class to render with wrapping div.
	/// </summary>
	[Parameter] public new string CssClass { get; set; }

	/// <summary>
	/// Custom CSS class to render with the label.
	/// </summary>
	[Parameter] public string LabelCssClass { get; set; }
	#endregion

	/// <summary>
	/// Custom CSS class to render with the input element.
	/// </summary>
	[Parameter] public string InputCssClass { get; set; }

	/// <summary>
	/// When <c>true</c>, <see cref="HxChipGenerator"/> is used to generate chip item(s). Default is <c>true</c>.
	/// </summary>
	[Parameter] public bool GenerateChip { get; set; } = true;

	/// <summary>
	/// Chip template.
	/// </summary>
	[Parameter] public RenderFragment ChipTemplate { get; set; }

	/// <summary>
	/// When <c>null</c> (default), the <c>Enabled</c> value is received from cascading <see cref="FormState" />.
	/// When value is <c>false</c>, input is rendered as disabled.
	/// To set multiple controls as disabled use <seealso cref="HxFormState" />.
	/// </summary>
	[Parameter] public bool? Enabled { get; set; }

	/// <summary>
	/// Returns effective value for <see cref="Enabled"/> property.
	/// </summary>
	protected virtual bool EnabledEffective => CascadeEnabledComponent.EnabledEffective(this);

	/// <summary>
	/// CSS class to be rendered with the wrapping div.
	/// </summary>
	private protected virtual string CoreCssClass => CssClassHelper.Combine("hx-form-group position-relative",
		((this is IInputWithLabelType inputWithLabelType) && (inputWithLabelType.LabelTypeEffective == LabelType.Floating))
		? "form-floating"
		: null);

	/// <summary>
	/// CSS class to be rendered with the input element.
	/// </summary>
	private protected virtual string CoreInputCssClass => "form-control";

	/// <summary>
	/// CSS class to be rendered with the label.
	/// </summary>
	private protected virtual string CoreLabelCssClass =>
		((this is IInputWithLabelType inputWithLabelType) && (inputWithLabelType.LabelTypeEffective == LabelType.Regular))
		? "form-label"
		: null;

	/// <summary>
	/// CSS class to be rendered with the hint.
	/// </summary>
	private protected virtual string CoreHintCssClass => "form-text";

	/// <summary>
	/// ID if the input element. Autogenerated when used with label.
	/// </summary>
	protected string InputId { get; private set; }

	/// <summary>
	/// Input ElementReference.
	/// Can be <c>null</c>. 
	/// </summary>
	protected ElementReference InputElement { get; set; }

	/// <summary>
	/// Elements rendering order. Overriden in the <see cref="HxInputCheckbox"/> component.
	/// </summary>
	// TODO Remove when HxInputCheckbox removed?
	protected virtual LabelValueRenderOrder RenderOrder =>
		((this is IInputWithLabelType inputWithLabelType) && (inputWithLabelType.LabelTypeEffective == LabelType.Floating))
		? LabelValueRenderOrder.ValueLabel
		: LabelValueRenderOrder.LabelValue;

	/// <summary>
	/// Gets or sets the current value of the input.
	/// Setter can be used only when the component is enabled.
	/// </summary>
	protected new TValue CurrentValue
	{
		get => base.CurrentValue;
		set
		{
			ThrowIfNotEnabled();
			base.CurrentValue = value;
		}
	}

	/// <summary>
	/// Gets or sets the current value of the input, represented as a string.
	/// Setter can be used only when the component is enabled.
	/// </summary>
	/// <remarks>
	/// Although CurrentValueAsString calls CurrentValue we MUST implement the check here because
	/// CurrentValue is not virtual, so CurrentValueAsString calls CurrentValue in a base class.
	/// There is no such check.
	/// </remarks>
	protected new string CurrentValueAsString
	{
		get => base.CurrentValueAsString;
		set
		{
			ThrowIfNotEnabled();
			base.CurrentValueAsString = value;
		}
	}

	string IFormValueComponent.LabelFor => this.InputId;
	string IFormValueComponent.CoreCssClass => this.CoreCssClass;
	string IFormValueComponent.CoreLabelCssClass => this.CoreLabelCssClass;
	string IFormValueComponent.CoreHintCssClass => this.CoreHintCssClass;
	LabelValueRenderOrder IFormValueComponent.RenderOrder => this.RenderOrder;

	private EditContext autoCreatedEditContext;

	public override Task SetParametersAsync(ParameterView parameters)
	{
		parameters.SetParameterProperties(this); // set properties to the component
		EnsureCascadingEditContext(); // create edit context when there was none
		return base.SetParametersAsync(ParameterView.Empty); // process base method (validations & EditContext property logic)
	}

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		if ((this is IInputWithLabelType inputWithLabelType)
			&& (this is IInputWithPlaceholder inputWithPlaceholder)
			&& (inputWithLabelType.LabelType == Havit.Blazor.Components.Web.Bootstrap.LabelType.Floating)
			&& !String.IsNullOrEmpty(inputWithPlaceholder.Placeholder))
		{
			throw new InvalidOperationException($"Cannot use {nameof(IInputWithPlaceholder.Placeholder)} with floating labels.");
		}
	}
	/// <summary>
	/// When there is no EditContext cascading parameter, lets create a new one and assing it to CascadedEditContext private property in a base InputBase class.
	/// </summary>
	/// <remarks>
	/// Even there is a protected EditContext property we cannot assign a value. When doing so InvalidOperationException exception is thrown.
	/// </remarks>
	private void EnsureCascadingEditContext()
	{
		var cascadedEditContextProperty = typeof(InputBase<TValue>).GetProperty("CascadedEditContext", BindingFlags.NonPublic | BindingFlags.Instance);

		if (cascadedEditContextProperty.GetValue(this) == null)
		{
			autoCreatedEditContext ??= new EditContext(new object());
			cascadedEditContextProperty.SetValue(this, autoCreatedEditContext);
		}
	}

	/// <inheritdoc />
	protected override sealed void BuildRenderTree(RenderTreeBuilder builder)
	{
		// in checkbox label is renderead after input but we need InputId.
		if (!String.IsNullOrEmpty(Label) || (LabelTemplate != null))
		{
			EnsureInputId();
		}

		builder.OpenRegion(0);
		base.BuildRenderTree(builder);
		builder.CloseRegion();

		HxFormValueRenderer.Current.Render(1, builder, this);

		if (GenerateChip && ShouldRenderChipGenerator())
		{
			builder.OpenRegion(2);
			RenderChipGenerator(builder);
			builder.CloseRegion();
		}
	}

	void IFormValueComponent.RenderValue(RenderTreeBuilder builder)
	{
		BuildRenderInput(builder);
	}

	/// <summary>
	/// When EditContext was automaticly created, this method renders CascandingValue component with this EditContext and the content of the renderFrament.
	/// Otherwise only renderFragment is rendered.
	/// </summary>
	private protected void RenderWithAutoCreatedEditContextAsCascandingValue(RenderTreeBuilder builder, int sequence, RenderFragment renderFragment)
	{
		if (autoCreatedEditContext != null)
		{
			builder.OpenRegion(sequence);
			builder.OpenComponent<CascadingValue<EditContext>>(1);
			builder.AddAttribute(2, nameof(CascadingValue<EditContext>.IsFixed), true);
			builder.AddAttribute(3, nameof(CascadingValue<EditContext>.Value), autoCreatedEditContext);
			builder.AddAttribute(4, nameof(CascadingValue<EditContext>.ChildContent), (object)renderFragment);
			builder.CloseComponent();
			builder.CloseRegion();
		}
		else
		{
			renderFragment(builder);
		}
	}

	void IFormValueComponent.RenderValidationMessage(RenderTreeBuilder builder)
	{
		BuildRenderValidationMessage(builder);
	}

	/// <summary>
	/// Renders input.
	/// </summary>
	protected abstract void BuildRenderInput(RenderTreeBuilder builder);

	/// <summary>
	/// Add common attributes to the input.
	/// </summary>
	private protected virtual void BuildRenderInput_AddCommonAttributes(RenderTreeBuilder builder, string typeValue)
	{
		builder.AddMultipleAttributes(1, AdditionalAttributes);
		builder.AddAttribute(2, "id", InputId);
		builder.AddAttribute(3, "type", typeValue);
		builder.AddAttribute(4, "class", GetInputCssClassToRender());
		builder.AddAttribute(5, "disabled", !EnabledEffective);
		if ((this is IInputWithLabelType inputWithLabelType) && (inputWithLabelType.LabelTypeEffective == LabelType.Floating))
		{
			builder.AddAttribute(6, "placeholder", "placeholder"); // there must be a nonempty value (which is not visible)
		}
		else if (this is IInputWithPlaceholder inputWithPlaceholder)
		{
			builder.AddAttribute(7, "placeholder", inputWithPlaceholder.Placeholder);
		}

	}

	/// <summary>
	/// Renders validation message (component <see cref="HxValidationMessage{TValue}" />) when not disabled (<seealso cref="ValidationMessageModeEffective" />).
	/// </summary>
	protected virtual void BuildRenderValidationMessage(RenderTreeBuilder builder)
	{
		if (this.ValidationMessageModeEffective != Bootstrap.ValidationMessageMode.None) // if: performance
		{
			//<div class="invalid-feedback">
			//Please provide a valid city.
			//</div>
			builder.OpenComponent<HxValidationMessage<TValue>>(1);
			if (autoCreatedEditContext != null)
			{
				builder.AddAttribute(2, nameof(HxValidationMessage<TValue>.EditContext), autoCreatedEditContext);
			}
			builder.AddAttribute(3, nameof(HxValidationMessage<TValue>.For), ValueExpression);
			builder.AddAttribute(4, nameof(HxValidationMessage<TValue>.Mode), ValidationMessageModeEffective);
			builder.CloseComponent();
		}
	}

	/// <summary>
	/// Renders chip generator.
	/// </summary>
	protected virtual void RenderChipGenerator(RenderTreeBuilder builder)
	{
		builder.OpenComponent<HxChipGenerator>(0);
		builder.AddAttribute(1, nameof(HxChipGenerator.ChildContent), (RenderFragment)RenderChipTemplate);
		builder.AddAttribute(2, nameof(HxChipGenerator.ChipRemoveAction), GetChipRemoveAction());

		builder.CloseComponent();
	}

	/// <summary>
	/// Returns true when chip should be rendered.
	/// Supposed to make decision based on CurrentValue.
	/// </summary>
	/// <remarks>
	/// This method is called only when <see cref="GenerateChip" /> is <c>true</c>.
	/// The implementation of the method is not supposed to use <see cref="GenerateChip" /> property itself.
	/// </remarks>
	protected virtual bool ShouldRenderChipGenerator()
	{
		return !EqualityComparer<TValue>.Default.Equals(CurrentValue, default(TValue));
	}

	/// <summary>
	/// Returns chip template.
	/// </summary>
	protected void RenderChipTemplate(RenderTreeBuilder builder)
	{
		if (ChipTemplate != null)
		{
			builder.AddContent(0, ChipTemplate);
		}
		else
		{
			builder.OpenElement(0, "span");
			builder.AddAttribute(1, "class", "hx-chip-label");
			builder.OpenRegion(2);
			RenderChipLabel(builder);
			builder.CloseRegion();
			builder.AddContent(3, ": ");
			builder.CloseElement();
			builder.OpenRegion(4);
			RenderChipValue(builder);
			builder.CloseRegion();
		}
	}

	protected virtual void RenderChipLabel(RenderTreeBuilder builder)
	{
		builder.AddContent(0, this.Label);
	}

	protected virtual void RenderChipValue(RenderTreeBuilder builder)
	{
		builder.AddContent(0, this.CurrentValueAsString);
	}

	/// <summary>
	/// Returns action to remove chip from model.
	/// </summary>
	protected virtual Action<object> GetChipRemoveAction()
	{
		string fieldName = this.FieldIdentifier.FieldName; // carefully! don't use "this" in lambda below to allow it for GC
		TValue value = GetChipRemoveValue(); // carefully! don't use the method call in lambda below to allow "this" for GC
		Action<object> removeAction = (model) =>
		{
			var propertyInfo = model.GetType().GetProperty(fieldName);
			Contract.Assert(propertyInfo is not null, "Invalid FieldIdentifier. Check ValueExpression parameter.");
			propertyInfo.SetValue(model, value);
		};

		return removeAction;
	}

	/// <summary>
	/// Returns a value to be used to remove a chip.
	/// </summary>
	protected virtual TValue GetChipRemoveValue()
	{
		return default(TValue);
	}

	/// <summary>
	/// Gives focus to the input element.
	/// </summary>
	public virtual async ValueTask FocusAsync()
	{
		if (EqualityComparer<ElementReference>.Default.Equals(InputElement, default))
		{
			throw new InvalidOperationException($"Cannot focus {this.GetType()}. The method must be called after first render.");
		}
		await InputElement.FocusAsync();
	}

	/// <summary>
	/// Sets InputId to a random value when empty.
	/// </summary>
	protected void EnsureInputId()
	{
		if (String.IsNullOrEmpty(InputId))
		{
			InputId = "el" + Guid.NewGuid().ToString("N");
		}
	}

	/// <summary>
	/// Returns <c>true</c> when the Value is considered do be invalid. Otherwise <c>false</c>.
	/// </summary>
	private protected bool IsValueInvalid() => EditContext.GetValidationMessages(FieldIdentifier).Any();

	/// <summary>
	/// Gets css class for input.
	/// </summary>
	protected virtual string GetInputCssClassToRender()
	{
		string validationCssClass = IsValueInvalid() ? InvalidCssClass : null;
		return CssClassHelper.Combine(CoreInputCssClass, InputCssClass, validationCssClass, (this is IInputWithSize inputWithSize) ? inputWithSize.GetInputSizeCssClass() : null);
	}

	/// <summary>
	/// Returns attribute from the bounded property if exists. Otherwise returns <c>null</c>.
	/// </summary>
	protected TAttribute GetValueAttribute<TAttribute>()
		where TAttribute : Attribute
	{
		return FieldIdentifier.Model.GetType().GetMember(FieldIdentifier.FieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).Single().GetCustomAttribute<TAttribute>();
	}

	/// <summary>
	/// Throws <see cref="System.InvalidOperationException" /> when the component is disabled.
	/// </summary>
	protected void ThrowIfNotEnabled()
	{
		Contract.Requires<InvalidOperationException>(EnabledEffective, $"The {GetType().Name} component is in a disabled state.");
	}
}