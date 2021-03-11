using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class NamedView<TFilterModelType>
	{
		public string Name { get; }

		public Func<TFilterModelType> Filter { get; } // TODO: Naming?

		public NamedView(string name) : this(name, () => default)
		{
			// NOOP
		}

		public NamedView(string name, TFilterModelType filter) : this(name, () => filter)
		{
			// NOOP
		}

		public NamedView(string name, Func<TFilterModelType> filterFunc)
		{
			Name = name;
			Filter = filterFunc;
		}
	}
}
