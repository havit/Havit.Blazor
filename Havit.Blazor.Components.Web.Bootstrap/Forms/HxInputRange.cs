using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Text;
using Havit.Blazor.Components.Web.Bootstrap.Internal;
using Microsoft.JSInterop;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Allows the user to select a number in a specified range using a slider.<br />
/// Mirrors the <see href="https://v6-dev--twbs-bootstrap.netlify.app/docs/6.0/forms/range/">Bootstrap range</see> component:
/// a JavaScript-driven control that draws a filled track and can show an optional value <see cref="Bubble"/>
/// and <see cref="Ticks"/> (tick marks).
/// </summary>
/// <typeparam name="TValue">Supported values: <c>byte (Byte), sbyte (SByte), short (Int16), ushort(UInt16), int (Int32), uint(UInt32), long (Int64), ulong(UInt64), float (Single), double (Double) and decimal (Decimal)</c>.</typeparam>
public class HxInputRange<TValue> : HxInputBase<TValue>, IAsyncDisposable
{
	// DO NOT FORGET TO MAINTAIN DOCUMENTATION! (documentation comment of this class)
	private static HashSet<Type> s_supportedTypes = new HashSet<Type>
	{
		typeof(byte),
		typeof(sbyte),
		typeof(short),
		typeof(ushort),
		typeof(int),
		typeof(uint),
		typeof(long),
		typeof(ulong),
		typeof(float),
		typeof(double),
		typeof(decimal)
	};

	/// <summary>
	/// Returns <see cref="HxInputRange"/> defaults.
	/// Enables not sharing defaults in descendants with base classes.
	/// Enables having multiple descendants which differ in the default values.
	/// </summary>
	protected override InputRangeSettings GetDefaults() => HxInputRange.Defaults;

	/// <summary>
	/// Set of settings to be applied to the component instance (overrides <see cref="HxInputRange.Defaults"/>, overridden by individual parameters).
	/// </summary>
	[Parameter] public InputRangeSettings Settings { get; set; }

	/// <summary>
	/// Returns an optional set of component settings.
	/// </summary>
	/// <remarks>
	/// Similar to <see cref="GetDefaults"/>, enables defining wider <see cref="Settings"/> for component descendants (by returning a derived settings class).
	/// </remarks>
	protected override InputRangeSettings GetSettings() => Settings;

	/// <summary>
	/// By default, <code>HxInputRange</code> snaps to integer values. To change this, you can specify a step value.
	/// </summary>
	[Parameter] public TValue Step { get; set; }

	/// <summary>
	/// Minimum value.
	/// </summary>
	[Parameter, EditorRequired] public TValue Min { get; set; }

	/// <summary>
	/// Maximum value.
	/// </summary>
	[Parameter, EditorRequired] public TValue Max { get; set; }

	/// <summary>
	/// Instructs whether the <c>Value</c> is going to be updated <c>oninput</c> (immediately), or <c>onchange</c> (usually <c>onmouseup</c>).<br />
	/// Default is <see cref="BindEvent.OnChange"/>.
	/// </summary>
	[Parameter] public BindEvent? BindEvent { get; set; }
	protected virtual BindEvent BindEventEffective => BindEvent ?? GetSettings()?.BindEvent ?? GetDefaults()?.BindEvent ?? throw new InvalidOperationException(nameof(BindEvent) + " default for " + nameof(HxInputRange<TValue>) + " has to be set.");

	/// <summary>
	/// When <c>true</c>, a value bubble following the thumb is shown.<br />
	/// The default is <c>false</c>.
	/// </summary>
	[Parameter] public bool? Bubble { get; set; }
	protected virtual bool BubbleEffective => Bubble ?? GetSettings()?.Bubble ?? GetDefaults()?.Bubble ?? throw new InvalidOperationException(nameof(Bubble) + " default for " + nameof(HxInputRange<TValue>) + " has to be set.");

	/// <summary>
	/// Tick marks rendered along the track from a linked <c>&lt;datalist&gt;</c>. Each tick is positioned at its
	/// <see cref="InputRangeTick.Value"/> (even when the values are unevenly spaced) and an optional
	/// <see cref="InputRangeTick.Label"/> is shown beneath it.
	/// </summary>
	[Parameter] public IEnumerable<InputRangeTick> Ticks { get; set; }

	private protected override string CoreInputCssClass => "form-range-input";

	[Inject] protected IJSRuntime JSRuntime { get; set; }

	/// <summary>
	/// The input ElementReference.
	/// Can be <c>null</c>.
	/// </summary>
	protected ElementReference InputElement { get; set; }

	private ElementReference _rangeElement;
	private string _ticksDataListId;
	private IJSObjectReference _jsModule;
	private bool _jsInitialized;
	private string _jsInitConfigToken; // (re)initialize the plugin only when the configuration affecting the DOM decorations changes
	private string _jsValue; // value as known by the JavaScript plugin (kept in sync via JS interop)
	private bool _disposed;

	public HxInputRange()
	{
		Type underlyingType = typeof(TValue);

		if (!s_supportedTypes.Contains(underlyingType))
		{
			throw new InvalidOperationException($"[{GetType().Name}] Unsupported type {typeof(TValue)}.");
		}
	}

	protected override void BuildRenderInput(RenderTreeBuilder builder)
	{
		bool hasTicks = Ticks?.Any() ?? false;
		if (hasTicks)
		{
			_ticksDataListId ??= "el" + Guid.NewGuid().ToString("N");
		}

		// .form-range wrapper - owns the component tokens and the JS-driven --bs-range-fill custom property
		builder.OpenElement(1, "div");
		builder.AddAttribute(2, "class", "form-range");
		builder.AddElementReferenceCapture(3, value => _rangeElement = value);

		// .form-range-input
		builder.OpenElement(10, "input");
		builder.AddMultipleAttributes(11, AdditionalAttributes);
		builder.AddAttribute(12, "class", GetInputCssClassToRender());
		builder.AddAttribute(13, "type", "range");

		builder.AddAttribute(14, "value", BindConverter.FormatValue(Value));
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates
		// TODO VSTHRD101 via RuntimeHelpers.CreateInferredBindSetter?
		builder.AddAttribute(15, BindEventEffective.ToEventName(), EventCallback.Factory.CreateBinder(this, async value => await HandleValueChanged(value), Value));
#pragma warning restore VSTHRD101 // Avoid unsupported async delegates
		builder.SetUpdatesAttributeName("value");

		builder.AddAttribute(16, "min", BindConverter.FormatValue(Min));
		builder.AddAttribute(17, "max", BindConverter.FormatValue(Max));
		builder.AddAttribute(18, "step", BindConverter.FormatValue(Step));

		builder.AddAttribute(20, "disabled", !EnabledEffective);

		builder.AddAttribute(30, "id", InputId);
		if (!String.IsNullOrEmpty(NameAttributeValue))
		{
			builder.AddAttribute(31, "name", NameAttributeValue);
		}

		string ariaDescribedBy = String.Join(" ", HintId, ValidationMessageId).Trim();
		if (!String.IsNullOrEmpty(ariaDescribedBy))
		{
			builder.AddAttribute(32, "aria-describedby", ariaDescribedBy);
		}

		if (hasTicks)
		{
			builder.AddAttribute(33, "list", _ticksDataListId);
		}

		// Capture ElementReference to the input to make focusing it programmatically possible.
		builder.AddElementReferenceCapture(40, value => InputElement = value);

		builder.CloseElement(); // input.form-range-input

		if (hasTicks)
		{
			builder.OpenElement(50, "datalist");
			builder.AddAttribute(51, "id", _ticksDataListId);
			foreach (InputRangeTick tick in Ticks)
			{
				builder.OpenElement(52, "option");
				builder.AddAttribute(53, "value", tick.Value.ToString(CultureInfo.InvariantCulture));
				if (!String.IsNullOrEmpty(tick.Label))
				{
					builder.AddAttribute(54, "label", tick.Label);
				}
				builder.CloseElement(); // option
			}
			builder.CloseElement(); // datalist
		}

		builder.CloseElement(); // div.form-range
	}

	protected async Task HandleValueChanged(TValue value)
	{
		Value = value;
		_jsValue = FormatValueInvariant(value); // the plugin already updated the fill from the same DOM event - keep it in sync to avoid a redundant update() call
		await ValueChanged.InvokeAsync(Value);
	}

	/// <inheritdoc />
	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		await base.OnAfterRenderAsync(firstRender);

		if (_disposed)
		{
			return;
		}

		string configToken = GetJsInitConfigToken();
		string currentValue = FormatValueInvariant(Value);

		if (!_jsInitialized || (_jsInitConfigToken != configToken))
		{
			await EnsureJsModuleAsync();
			if (_disposed)
			{
				return;
			}

			_jsInitialized = true;
			_jsInitConfigToken = configToken;
			_jsValue = currentValue;

			await _jsModule.InvokeVoidAsync("initialize", _rangeElement, BubbleEffective);
		}
		else if (currentValue != _jsValue)
		{
			// propagate a programmatic value change to the JavaScript plugin so it redraws the fill (and the bubble)
			_jsValue = currentValue;
			await _jsModule.InvokeVoidAsync("update", _rangeElement);
		}
	}

	private static string FormatValueInvariant(TValue value) => BindConverter.FormatValue(value)?.ToString();

	private string GetJsInitConfigToken()
	{
		// the plugin builds the bubble and the tick marks once at construction, so it must be reinitialized when any of these change
		var token = new StringBuilder();
		token.Append(BubbleEffective ? '1' : '0');
		token.Append('|').Append(FormatValueInvariant(Min));
		token.Append('|').Append(FormatValueInvariant(Max));
		token.Append('|');
		if (Ticks != null)
		{
			foreach (InputRangeTick tick in Ticks)
			{
				token.Append(tick.Value.ToString(CultureInfo.InvariantCulture)).Append(':').Append(tick.Label).Append(';');
			}
		}
		return token.ToString();
	}

	private async Task EnsureJsModuleAsync()
	{
		_jsModule ??= await JSRuntime.ImportHavitBlazorBootstrapModuleAsync(nameof(HxInputRange));
	}

	protected override bool TryParseValueFromString(string value, [MaybeNullWhen(false)] out TValue result, [NotNullWhen(false)] out string validationErrorMessage)
	{
		throw new InvalidOperationException("HxInputRange displays no text value and receives the initial value as float, therefore, this method must not be called.");
	}

	/// <summary>
	/// Focuses the input range.
	/// </summary>
	public async ValueTask FocusAsync() => await InputElement.FocusOrThrowAsync(this);

	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async ValueTask DisposeAsyncCore()
	{
		_disposed = true;

		if (_jsModule != null)
		{
			try
			{
				if (_jsInitialized)
				{
					await _jsModule.InvokeVoidAsync("dispose", _rangeElement);
				}
				await _jsModule.DisposeAsync();
			}
			catch (JSDisconnectedException)
			{
				// NOOP
			}
			catch (TaskCanceledException)
			{
				// NOOP
			}

			_jsModule = null;
		}
	}
}
