namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Represents a named view for <see cref="HxListLayout{TFilterModel}" />.
/// </summary>
public class NamedView<TFilterModel>
{
	private Func<TFilterModel> _filterModelFactory;

	/// <summary>
	/// Name of the view. Used as a label in the list. Can be used as a title for the <see cref="HxListLayout{TFilterModel}" />.
	/// </summary>
	public string Name { get; }


	/// <summary>
	/// Creates a new instance of <see cref="NamedView{TFilterModel}" /> which uses a factory to build a filter model.
	/// </summary>
	/// <param name="name">Name of the view.</param>
	/// <param name="filterModelFactory">Function which builds a new filter model to be applied when the view is selected.</param>
	public NamedView(string name, Func<TFilterModel> filterModelFactory)
	{
		Name = name;
		_filterModelFactory = filterModelFactory;
	}

	/// <summary>
	/// Creates a new filter model for the view.
	/// </summary>
	/// <returns>A new instance of the filter model.</returns>
	public TFilterModel CreateFilterModel()
	{
		return _filterModelFactory();
	}
}
