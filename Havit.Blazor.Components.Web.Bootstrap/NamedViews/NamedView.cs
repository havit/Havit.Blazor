namespace Havit.Blazor.Components.Web.Bootstrap;

public class NamedView<TFilterModel>
{
	public string Name { get; }

	public Func<TFilterModel> Filter { get; }

	public NamedView(string name) : this(name, () => default)
	{
		// NOOP
	}

	public NamedView(string name, TFilterModel filter) : this(name, () => filter)
	{
		// NOOP
	}

	public NamedView(string name, Func<TFilterModel> filterFunc)
	{
		Name = name;
		Filter = filterFunc;
	}
}
