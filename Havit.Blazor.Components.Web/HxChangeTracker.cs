using System.ComponentModel;
using Microsoft.Extensions.Logging;

namespace Havit.Blazor.Components.Web;

/// <summary>
/// Subscribes to INotifyPropertyChanged changes and triggers re-rendering of ChildContent.
/// To be used when you want to limit the scope of re-rendering and prevent re-rendering of a wider component.
/// </summary>
public class HxChangeTracker : ComponentBase, IDisposable
{
	/// <summary>
	/// The content to which the value should be provided.
	/// </summary>
	[Parameter] public RenderFragment ChildContent { get; set; }

	/// <summary>
	/// The value to be tracked.
	/// </summary>
	[Parameter] public INotifyPropertyChanged Value { get; set; }

	[Inject] protected ILogger<HxChangeTracker> Logger { get; set; }

	private INotifyPropertyChanged _previousValueSet;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		if (Value is null)
		{
			throw new ArgumentException($"Missing required parameter '{nameof(Value)}' for component '{GetType().Name}'.");
		}

		if (_previousValueSet != Value)
		{
			Logger.LogDebug("Value.PropertyChanged += Value_PropertyChanged;");
			Value.PropertyChanged += Value_PropertyChanged;

			if (_previousValueSet is not null)
			{
				Logger.LogDebug("previousValueSet.PropertyChanged -= Value_PropertyChanged");
				_previousValueSet.PropertyChanged -= Value_PropertyChanged;
			}
		}
		_previousValueSet = Value;
	}

	private void Value_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		Logger.LogDebug("Value_PropertyChanged");
		StateHasChanged();
	}

	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		Logger.LogDebug("BuildRenderTree");
		builder.AddContent(0, ChildContent);
	}

	public void Dispose()
	{
		Dispose(true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			Logger.LogDebug("Dispose: Value.PropertyChanged -= Value_PropertyChanged;");
			Value.PropertyChanged -= Value_PropertyChanged;
		}
	}
}
