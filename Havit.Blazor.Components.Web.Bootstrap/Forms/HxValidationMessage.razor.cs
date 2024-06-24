using System.Linq.Expressions;
using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Displays a list of validation messages for a specified field within a cascaded <see cref="EditContext"/>.<br />
/// Reimplementation of Blazor <see cref="ValidationMessage{TValue}"/> as <see href="https://getbootstrap.com/docs/5.3/forms/validation/">Bootstrap 5 validation</see>.<br/>
/// Used by <see cref="HxInputBase{TValue}"/> and derived components.<br />
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxValidationMessage">https://havit.blazor.eu/components/HxValidationMessage</see>
/// </summary>
public partial class HxValidationMessage<TValue> : ComponentBase, IDisposable
{
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
	/// Mutual exclusive with <see cref="ForFieldName"/> and <see cref="ForFieldNames"/>.
	/// </summary>
	[Parameter] public Expression<Func<TValue>> For { get; set; }

	/// <summary>
	/// Specifies the field for which validation messages should be displayed.
	/// Mutual exclusive with <see cref="For"/> and <see cref="ForFieldNames"/>.
	/// </summary>
	[Parameter] public string ForFieldName { get; set; }

	/// <summary>
	/// Specifies the field for which validation messages should be displayed.
	/// Mutual exclusive with <see cref="For"/> and <see cref="ForFieldName"/>.
	/// </summary>
	[Parameter] public string[] ForFieldNames { get; set; }

	/// <summary>
	/// Specifies how the validation message should be displayed.
	/// </summary>
	[Parameter] public ValidationMessageMode Mode { get; set; } = ValidationMessageMode.Regular;

	private EditContext _previousEditContext;
	private Expression<Func<TValue>> _previousFor;
	private string _previousForFieldName;
	private string[] _previousForFieldNames;
	private readonly EventHandler<ValidationStateChangedEventArgs> _validationStateChangedHandler;
	private FieldIdentifier[] _fieldIdentifiers;
	private EditContext _currentEditContext;

	/// <summary>
	/// Constructs an instance of <see cref="HxValidationMessage{TValue}"/>.
	/// </summary>
	public HxValidationMessage()
	{
		_validationStateChangedHandler = (sender, eventArgs) => StateHasChanged();
	}

	/// <inheritdoc />
	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		_currentEditContext = CascadingEditContext ?? EditContext;
		if (_currentEditContext == null)
		{
			throw new InvalidOperationException($"{GetType()} requires a cascading parameter of type {nameof(Microsoft.AspNetCore.Components.Forms.EditContext)} or {nameof(EditContext)} property set. Use {GetType()} inside an {nameof(EditForm)}.");
		}

		if (For == null && String.IsNullOrEmpty(ForFieldName) && (ForFieldNames == null))
		{
			throw new InvalidOperationException($"{GetType()} requires a value for the {nameof(For)} or {nameof(ForFieldName)} or {nameof(ForFieldNames)} parameter.");
		}

		if (For != null && !String.IsNullOrEmpty(ForFieldName))
		{
			throw new InvalidOperationException($"{GetType()} requires a value for the {nameof(For)} or {nameof(ForFieldName)} parameter, but not both parameters.");
		}

		if ((For != null) && (For != _previousFor))
		{
			_fieldIdentifiers = new[] { FieldIdentifier.Create(For) };
			_previousFor = For;
		}

		if (!String.IsNullOrEmpty(ForFieldName) && (ForFieldName != _previousForFieldName))
		{
			_fieldIdentifiers = new[] { new FieldIdentifier(_currentEditContext.Model, ForFieldName) };
			_previousForFieldName = ForFieldName;
		}

		if ((ForFieldNames != null) && (ForFieldNames != _previousForFieldNames))
		{
			_fieldIdentifiers = ForFieldNames.Select(forFieldName => new FieldIdentifier(_currentEditContext.Model, forFieldName)).ToArray();
			_previousForFieldNames = ForFieldNames;
		}

		if (_currentEditContext != _previousEditContext)
		{
			DetachValidationStateChangedListener();
			_currentEditContext.OnValidationStateChanged += _validationStateChangedHandler;
			_previousEditContext = _currentEditContext;
		}
	}

	/// <inheritdoc />
	public void Dispose()
	{
		Dispose(true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			DetachValidationStateChangedListener();
		}
	}

	private void DetachValidationStateChangedListener()
	{
		if (_previousEditContext != null)
		{
			_previousEditContext.OnValidationStateChanged -= _validationStateChangedHandler;
		}
	}
}
