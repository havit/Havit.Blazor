using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Layouts
{
	/// <summary>
	/// Interface for column-sized components (e.g. <see cref="HxPlaceholder"/>).
	/// </summary>
	public interface ILayoutColumnComponent
	{
		/// <summary>
		/// Number of template columns to span. Responsive setting for all devices including the extra-small ones (XS) bellow "small" breakpoint (<c>576px</c>).<br />
		/// The value can be any integer number between <c>1</c> and <c>12</c> (<c>.col-1</c>), <c>auto</c> (<c>.col-auto</c>) or <c>true</c> (<c>.col</c>).
		/// </summary>
		string Columns { get; set; }

		/// <summary>
		/// Number of template columns to span for viewports above "small" breakpoint (<c>576px</c>).<br/>
		/// The value can be any integer number between <c>1</c> and <c>12</c>, <c>auto</c> or <c>true</c>.
		/// </summary>
		string ColumnsSmallUp { get; set; }

		/// <summary>
		/// Number of template columns to span for viewports above "medium" breakpoint (<c>768px</c>).<br/>
		/// The value can be any integer number between <c>1</c> and <c>12</c>, <c>auto</c> or <c>true</c>.
		/// </summary>
		string ColumnsMediumUp { get; set; }

		/// <summary>
		/// Number of template columns to span for viewports above "large" breakpoint (<c>992px</c>).<br/>
		/// The value can be any integer number between <c>1</c> and <c>12</c>, <c>auto</c> or <c>true</c>.
		/// </summary>
		string ColumnsLargeUp { get; set; }

		/// <summary>
		/// Number of template columns to span for viewports above "large" breakpoint (<c>1200px</c>).<br/>
		/// The value can be any integer number between <c>1</c> and <c>12</c>, <c>auto</c> or <c>true</c>.
		/// </summary>
		string ColumnsExtraLargeUp { get; set; }

		/// <summary>
		/// Number of template columns to span for viewports above "XXL" breakpoint (<c>1400px</c>).<br/>
		/// The value can be any integer number between <c>1</c> and <c>12</c>, <c>auto</c> or <c>true</c>.
		/// </summary>
		string ColumnsXxlUp { get; set; }
	}
}
