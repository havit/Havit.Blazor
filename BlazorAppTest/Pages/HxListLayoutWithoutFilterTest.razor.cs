using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Havit.Blazor.Components.Web.Bootstrap;
using Microsoft.AspNetCore.Components;

namespace BlazorAppTest.Pages
{
	public partial class HxListLayoutWithoutFilterTest
	{
		[Inject] protected NavigationManager NavigationManager { get; set; }

		private DataItemDto currentItem;
		private HxGrid<DataItemDto> gridComponent;

		private Task<GridDataProviderResult<DataItemDto>> LoadDataItems(GridDataProviderRequest<DataItemDto> request)
		{
			List<DataItemDto> result = new List<DataItemDto>()
			{
				new DataItemDto() { ItemId = 1, Name = "Jednička"},
				new DataItemDto() { ItemId = 2, Name = "Dvojka"},
				new DataItemDto() { ItemId = 3, Name = "Trojka"}
			};

			return Task.FromResult(new GridDataProviderResult<DataItemDto>()
			{
				Data = result,
				TotalCount = result.Count
			});
		}

		protected Task NewInvoiceClicked()
		{
			this.currentItem = new DataItemDto();
			// OpenDetail() ?
			return Task.CompletedTask;
		}

		protected Task DeleteClicked(DataItemDto item)
		{
			return Task.CompletedTask;
		}

		public record DataItemDto
		{
			public int ItemId { get; set; }
			public string Name { get; set; }
		}
	}
}
