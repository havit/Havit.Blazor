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
	public partial class HxListLayoutTest
	{
		[Inject] protected NavigationManager NavigationManager { get; set; }

		private DataItemDto currentItem;
		private FilterModelDto filterModel = new FilterModelDto();
		private HxGrid<DataItemDto> gridComponent;

		private readonly IEnumerable<NamedView<FilterModelDto>> namedViews = new List<NamedView<FilterModelDto>>()
		{
			new NamedView<FilterModelDto>("Letos vystavené", () => new FilterModelDto { } ),
			new NamedView<FilterModelDto>("Neuhrazené po splatnosti", () => new FilterModelDto { }),
			new NamedView<FilterModelDto>("Po splatnosti > 30 dnů", () => new FilterModelDto { })
		};

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

		protected async Task NamedViewSelected(NamedView<FilterModelDto> namedView)
		{
			filterModel = namedView.Filter();
			await gridComponent.RefreshDataAsync();
		}

		protected async Task SearchRequested()
		{
			await gridComponent.RefreshDataAsync();
		}

		private Task DeleteItemClicked(DataItemDto dataItemDto)
		{
			_ = dataItemDto;
			return Task.CompletedTask;
		}

		private Task HandleSelectedDataItemChanged(DataItemDto selection)
		{
			currentItem = selection;
			// await dataItemEditComponent.ShowAsync();
			return Task.CompletedTask;
		}

		private Task NewItemClicked()
		{
			currentItem = new DataItemDto();
			// await dataItemEditComponent.ShowAsync();
			return Task.CompletedTask;
		}

		public record FilterModelDto
		{
			public int ContactId { get; set; }
		}

		public record DataItemDto
		{
			public int ItemId { get; set; }
			public string Name { get; set; }
		}
	}
}
