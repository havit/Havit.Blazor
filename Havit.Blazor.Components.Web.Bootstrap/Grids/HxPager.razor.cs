using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap.Grids
{
	public partial class HxPager : ComponentBase
	{
		[Parameter] public int TotalPages { get; set; }
		[Parameter] public int CurrentPageIndex { get; set; }
		[Parameter] public EventCallback<int> CurrentPageIndexChanged { get; set; }
		[Parameter] public bool ShowAllButton { get; set; } = true;
		[Parameter] public EventCallback ShowAllButtonClicked { get; set; }
		
		protected int pageFromInclusive;
		protected int pageToExclusive;
		protected bool showFirstLast;

		public override async Task SetParametersAsync(ParameterView parameters)
		{
			await base.SetParametersAsync(parameters);
			CalculateRenderFields();
		}

		protected async Task ChangeCurrentPageIndexTo(int newPageIndex)
		{
			Contract.Requires(newPageIndex >= 0, nameof(newPageIndex));
			Contract.Requires(newPageIndex < TotalPages, nameof(newPageIndex));

			CurrentPageIndex = newPageIndex;
			await CurrentPageIndexChanged.InvokeAsync(CurrentPageIndex);
			CalculateRenderFields();
		}
		
		private void CalculateRenderFields()
		{
			// todo: Možná doplnit logiku a posouvat se dle přiblížení k "hranici"
			if (CurrentPageIndex >= 5)
			{
				pageToExclusive = Math.Min(TotalPages, CurrentPageIndex + 5);
				pageFromInclusive = Math.Max(0, pageToExclusive - 10);
			}
			else
			{
				pageToExclusive = Math.Min(TotalPages, 10);
				pageFromInclusive = 0;
			}
			showFirstLast = true; // always true
		}

		private async Task ShowAllButtonClick()
		{
			await ShowAllButtonClicked.InvokeAsync(null);
		}
	}
}
