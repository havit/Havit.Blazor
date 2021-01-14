using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Layouts
{
	/// <summary>
	/// To be used as CascadingValue in Layouts to be able to pass between components.
	/// </summary>
	public class LayoutDetailContext : INotifyPropertyChanged
	{

		public string DetailTitle
		{
			get
			{
				return detailTitle;
			}
			set
			{
				if (detailTitle != value)
				{
					detailTitle = value;
					PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DetailTitle)));
				}
			}
		}
		private string detailTitle;

		public event PropertyChangedEventHandler PropertyChanged;

		public override bool Equals(object obj)
		{
			if (obj is LayoutDetailContext ctx)
			{
				return this.DetailTitle?.Equals(ctx.DetailTitle) ?? false;
			}
			return base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(this.DetailTitle);
		}
	}
}
