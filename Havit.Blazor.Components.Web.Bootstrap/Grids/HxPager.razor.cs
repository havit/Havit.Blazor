﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Pager.
	/// </summary>
	public partial class HxPager : ComponentBase
	{
		/// <summary>
		/// Total pages of data items.
		/// </summary>
		[Parameter] public int TotalPages { get; set; }  // TODO RH: Nerozdělit na TotalItems a PageSize? ...bude se to líp používat; Kdyžtak DevExpress tomu říkají PageCount

		/// <summary>
		/// Current page index. Zero based.
		/// Displayed numbers start with 1 which is page index 0.
		/// </summary>
		[Parameter] public int CurrentPageIndex { get; set; }  // TODO RH: ActivePageIndex?

		/// <summary>
		/// Event raised where page index is changed.
		/// </summary>
		[Parameter] public EventCallback<int> CurrentPageIndexChanged { get; set; }

		/// <summary>
		/// Count of numbers to display. Default value is 10.
		/// </summary>
		[Parameter] public int DisplayNumberCount { get; set; } = 10; // TODO RH: Rename NumberCount? Viz https://docs.telerik.com/blazor-ui/components/pager/overview; nebo VisibleNumericButtonCount (https://demos.devexpress.com/blazor/Pager)

		/// <summary>
		/// Changes current page index and fires event.
		/// </summary>
		protected async Task SetCurrentPageIndexTo(int newPageIndex)
		{
			Contract.Requires(newPageIndex >= 0, nameof(newPageIndex));
			Contract.Requires(newPageIndex < TotalPages, nameof(newPageIndex));

			CurrentPageIndex = newPageIndex;
			await CurrentPageIndexChanged.InvokeAsync(CurrentPageIndex);
		}
	}
}
