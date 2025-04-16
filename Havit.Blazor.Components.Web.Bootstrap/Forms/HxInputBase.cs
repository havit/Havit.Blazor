using System.Reflection;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Havit.Blazor.Components.Web.Infrastructure;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// A base class for form input components. This base class automatically integrates
/// with a Microsoft.AspNetCore.Components.Forms.EditContext, which must be supplied
/// as a cascading parameter.
/// Extends the <see cref="InputBase{TValue}"/> class.
/// 
/// Adds support for rendering bootstrap-based input with a validator.
/// See also <see href="https://getbootstrap.com/docs/5.3/forms/overview/">https://getbootstrap.com/docs/5.3/forms/overview/</see>.
/// </summary>
public abstract class HxInputBase<TValue> : InputBase<TValue>, ICascadeEnabledComponent, IFormValueComponent
{
	/// <summary>
	/// CSS class used for invalid input.
	/// </summary>
	public const string InvalidCssClass = "is-invalid";

	/// <summary>
	/// Returns the defaults for <see cref="HxInputBase{TValue}"/>.
	/// Enables not sharing defaults in descendants with base classes.
	/// Enables having multiple descendants that differ in the default values.
	/// </summary>
	protected virtual InputSettings GetDefaults() => HxInputBase.Defaults;

	/// <summary>
	/// Gets the set of settings to be applied to the component instance (overrides <see cref="HxInputBase.Defaults"/>, overridden by individual parameters).
	/// </summary>
	/// <remarks>
	/// Using an interface does not force the implementation of settings to use a specific class as a base type.</remarks>
	protected abstract InputSettings GetSettings();

	/// <summary>
	/// Specifies how the validation message should be displayed.<br/>
	/// The default is <see cref="ValidationMessageMode.Regular"/>, you can override the application-wide default for all inputs in <see cref="HxInputBase.Defaults"/>.
	/// </summary>
	[Parameter] public ValidationMessageMode? ValidationMessageMode { get; set; }
	protected ValidationMessageMode ValidationMessageModeEffective => ValidationMessageMode ?? GetSettings()?.ValidationMessageMode ?? GetDefaults().ValidationMessageMode ?? HxInputBase.Defaults?.ValidationMessageMode ?? throw new InvalidOperationException(nameof(ValidationMessageMode) + " default for " + nameof(HxInputBase<TValue>) + " has to be set.");

	/// <inheritdoc cref="Web.FormState" />
	[CascadingParameter] protected FormState FormState { get; set; }
	FormState ICascadeEnabledComponent.FormState { get => FormState; set => FormState = value; }

	#region IFormValueComponent public properties
	/// <summary>
	/// The label text.
	/// </summary>
	[Parameter] public string Label { get; set; }

	/// <summary>
	/// The label content.
	/// </summary>
	[Parameter] public RenderFragment LabelTemplate { get; set; }

	/// <summary>
	/// The hint to render after the input as form-text.
	/// </summary>
	[Parameter] public string Hint { get; set; }

	/// <summary>
	/// The hint to render after the input as form-text.
	/// </summary>
	[Parameter] public RenderFragment HintTemplate { get; set; }

	/// <summary>
	/// The custom CSS class to render with the wrapping div.
	/// </summary>
	[Parameter] public new string CssClass { get; set; }

	/// <summary>
	/// The custom CSS class to render with the label.
	/// </summary>
	[Parameter] public string LabelCssClass { get; set; }
	#endregion

	/// <summary>
	/// The custom CSS class to render with the input element.
	/// </summary>
	[Parameter] public string InputCssClass { get; set; }

	/// <summary>
	/// When <c>true</c>, <see cref="HxChipGenerator"/> is used to generate chip item(s). The default is <c>true</c>.
	/// </summary>
	[Parameter] public bool GenerateChip { get; set; } = true;

	/// <summary>
	/// The chip template.
	/// </summary>
	[Parameter] public RenderFragment ChipTemplate { get; set; }

	/// <summary>
	/// When <c>null</c> (default), the <c>Enabled</c> value is received from the cascading <see cref="FormState" />.
	/// When the value is <c>false</c>, the input is rendered as disabled.
	/// To set multiple controls as disabled, use <seealso cref="HxFormState" />.
	/// </summary>
	[Parameter] public bool? Enabled { get; set; }

	/// <summary>
	/// Gets the effective value for the <see cref="Enabled"/> property.
	/// </summary>
	protected virtual bool EnabledEffective => CascadeEnabledComponent.EnabledEffective(this);

	/// <summary>
	/// The CSS class to be rendered with the wrapping div.
	/// </summary>
	private protected virtual string CoreCssClass => "hx-form-group position-relative";

	/// <summary>
	/// The CSS class to be rendered with the input element.
	/// </summary>
	private protected virtual string CoreInputCssClass => "form-control";

	/// <summary>
	/// The CSS class to be rendered with the label.
	/// </summary>
	private protected virtual string CoreLabelCssClass =>
		((this is IInputWithLabelType inputWithLabelType) && (inputWithLabelType.LabelTypeEffective == LabelType.Floating))
		? null
		: "form-label";

	/// <summary>
	/// The CSS class to be rendered with the hint.
	/// </summary>
	private protected virtual string CoreHintCssClass => "form-text";

	/// <summary>
	/// The ID of the input element. Autogenerated when used with a label.
	/// </summary>
	protected string InputId { get; private set; }

	/// <summary>
	/// The input ElementReference.
	/// Can be <c>null</c>. 
	/// </summary>
	protected ElementReference InputElement { get; set; }

	/// <summary>
	/// Elements rendering order.
	/// </summary>
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

	string IFormValueComponent.LabelFor => InputId;
	string IFormValueComponent.CoreCssClass => CoreCssClass;
	string IFormValueComponent.CoreLabelCssClass => CoreLabelCssClass;
	string IFormValueComponent.CoreHintCssClass => CoreHintCssClass;
	LabelValueRenderOrder IFormValueComponent.RenderOrder => RenderOrder;

	private EditContext _autoCreatedEditContext;

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
			&& (inputWithLabelType.LabelTypeEffective == LabelType.Floating)
			&& !String.IsNullOrEmpty(inputWithPlaceholder.Placeholder))
		{
			throw new InvalidOperationException($"[{GetType().Name}] Cannot use {nameof(IInputWithPlaceholder.Placeholder)} with floating labels.");
		}
	}

	/// <summary>
	/// When there is no EditContext cascading parameter, let's create a new one and assign it to the CascadedEditContext private property in the base InputBase class.
	/// </summary>
	/// <remarks>
	/// Even though there is a protected EditContext property, we cannot assign a value. When doing so, an InvalidOperationException exception is thrown.
	/// </remarks>
	private void EnsureCascadingEditContext()
	{
		var cascadedEditContextProperty = typeof(InputBase<TValue>).GetProperty("CascadedEditContext", BindingFlags.NonPublic | BindingFlags.Instance);

		if (cascadedEditContextProperty.GetValue(this) == null)
		{
			_autoCreatedEditContext ??= new EditContext(new object());
			cascadedEditContextProperty.SetValue(this, _autoCreatedEditContext);
		}
	}

	/// <inheritdoc />
	protected override sealed void BuildRenderTree(RenderTreeBuilder builder)
	{
		// in checkbox label is rendered after input but we need InputId.
		if (!String.IsNullOrEmpty(Label) || (LabelTemplate != null))
		{
			EnsureInputId();
		}

		builder.OpenRegion(0);
		base.BuildRenderTree(builder);
		builder.CloseRegion();

		FormValueRenderer.Current.Render(1, builder, this);

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
	/// When the EditContext was automatically created, this method renders the CascadingValue component with this EditContext and the content of the renderFragment.
	/// Otherwise, only the renderFragment is rendered.
	/// </summary>
	protected void RenderWithAutoCreatedEditContextAsCascadingValue(RenderTreeBuilder builder, int sequence, RenderFragment renderFragment)
	{
		if (_autoCreatedEditContext != null)
		{
			builder.OpenRegion(sequence);
			builder.OpenComponent<CascadingValue<EditContext>>(1);
			builder.AddAttribute(2, nameof(CascadingValue<EditContext>.IsFixed), true);
			builder.AddAttribute(3, nameof(CascadingValue<EditContext>.Value), _autoCreatedEditContext);
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
	/// Renders the input.
	/// </summary>
	protected abstract void BuildRenderInput(RenderTreeBuilder builder);

	/// <summary>
	/// Adds common attributes to the input.
	/// </summary>
	private protected virtual void BuildRenderInput_AddCommonAttributes(RenderTreeBuilder builder, string typeValue)
	{
		builder.AddMultipleAttributes(1, AdditionalAttributes);
		builder.AddAttribute(2, "id", InputId);
		if (!String.IsNullOrEmpty(NameAttributeValue))
		{
			builder.AddAttribute(3, "name", NameAttributeValue);
		}
		builder.AddAttribute(4, "type", typeValue);
		builder.AddAttribute(5, "class", GetInputCssClassToRender());
		builder.AddAttribute(6, "disabled", !EnabledEffective);
		if ((this is IInputWithLabelType inputWithLabelType) && (inputWithLabelType.LabelTypeEffective == LabelType.Floating))
		{
			builder.AddAttribute(7, "placeholder", "placeholder"); // there must be a nonempty value (which is not visible)
		}
		else if (this is IInputWithPlaceholder inputWithPlaceholder)
		{
			builder.AddAttribute(8, "placeholder", inputWithPlaceholder.Placeholder);
		}

	}

	/// <summary>
	/// Renders the validation message (component <see cref="HxValidationMessage{TValue}" />) when not disabled (<seealso cref="ValidationMessageModeEffective" />).
	/// </summary>
	protected virtual void BuildRenderValidationMessage(RenderTreeBuilder builder)
	{
		if (ValidationMessageModeEffective != Bootstrap.ValidationMessageMode.None) // if: performance
		{
			//<div class="invalid-feedback">
			//Please provide a valid city.
			//</div>
			builder.OpenComponent<HxValidationMessage<TValue>>(1);
			if (_autoCreatedEditContext != null)
			{
				builder.AddAttribute(2, nameof(HxValidationMessage<TValue>.EditContext), _autoCreatedEditContext);
			}
			builder.AddAttribute(3, nameof(HxValidationMessage<TValue>.For), ValueExpression);
			builder.AddAttribute(4, nameof(HxValidationMessage<TValue>.Mode), ValidationMessageModeEffective);
			builder.CloseComponent();
		}
	}

	/// <summary>
	/// Renders the chip generator.
	/// </summary>
	protected virtual void RenderChipGenerator(RenderTreeBuilder builder)
	{
		builder.OpenComponent<HxChipGenerator>(0);
		builder.AddAttribute(1, nameof(HxChipGenerator.ChildContent), (RenderFragment)RenderChipTemplate);
		builder.AddAttribute(2, nameof(HxChipGenerator.ChipRemoveAction), GetChipRemoveAction());

		builder.CloseComponent();
	}

	/// <summary>
	/// Returns true when the chip should be rendered.
	/// Supposed to make a decision based on CurrentValue.
	/// </summary>
	/// <remarks>
	/// This method is called only when <see cref="GenerateChip" /> is <c>true</c>.
	/// The implementation of the method is not supposed to use the <see cref="GenerateChip" /> property itself.
	/// </remarks>
	protected virtual bool ShouldRenderChipGenerator()
	{
		if (CurrentValue is string currentValueString)
		{
			// fixes #659 [HxInputText] Generates chip for String.Empty value
			return !String.IsNullOrEmpty(currentValueString);
		}
		return !EqualityComparer<TValue>.Default.Equals(CurrentValue, default(TValue));
	}

	/// <summary>
	/// Returns the chip template.
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
			builder.AddAttribute(1, "class", "hx-chip-list-label");
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
		builder.AddContent(0, Label);
	}

	protected virtual void RenderChipValue(RenderTreeBuilder builder)
	{
		builder.AddContent(0, CurrentValueAsString);
	}

	/// <summary>
	/// Returns the action to remove the chip from the model.
	/// </summary>
	protected virtual Action<object> GetChipRemoveAction()
	{
		TValue value = GetChipRemoveValue(); // carefully! don't use the method call in lambda below to allow "this" for GC

		var builder = new ChipRemoveActionBuilder(ValueExpression, value);
		Action<object> removeAction = builder.Build();

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
			throw new InvalidOperationException($"[{GetType().Name}] Unable to focus, {nameof(InputElement)} reference not available.  You are most likely calling the method too early. The first render must complete before calling this method.");
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
		return FieldIdentifier.Model.GetType().GetMember(FieldIdentifier.FieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic).FirstOrDefault()?.GetCustomAttribute<TAttribute>();
	}

	/// <summary>
	/// Throws <see cref="System.InvalidOperationException" /> when the component is disabled.
	/// </summary>
	protected void ThrowIfNotEnabled()
	{
		Contract.Requires<InvalidOperationException>(EnabledEffective, $"The {GetType().Name} component is in a disabled state.");
	}
}