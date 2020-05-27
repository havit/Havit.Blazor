using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.NamedViews
{
	public class NamedView<TFilterType>
	{
		public string Name { get; }

		public Func<TFilterType> Filter { get; } // TODO: Naming?

		public NamedView(string name) : this(name, () => default)
		{
			// NOOP
		}

		public NamedView(string name, TFilterType filter) : this(name, () => filter)
		{
			// NOOP
		}

		public NamedView(string name, Func<TFilterType> filterFunc)
		{
			Name = name;
			Filter = filterFunc;
		}
	}
}
