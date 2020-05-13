using System;
using System.Collections.Generic;
using System.Text;

namespace Havit.Blazor.Components.Web.Bootstrap.NamedViews
{
	// TODO: Pro použití by stačil interface
	public class NamedView
	{
		public string Name { get; set; }

		public NamedView()
		{
			// NOOP
		}

		public NamedView(string name)
		{
			this.Name = name;
		}
	}
}
