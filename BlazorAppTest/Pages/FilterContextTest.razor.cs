using Havit.Blazor.Components.Web.Bootstrap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppTest.Pages
{
	public partial class FilterContextTest
	{
		protected FormModel model = new FormModel { Text1 = "initial value" };
		private HxGrid<CultureInfo> myGrid;

		private FilterContext<FormModel> filterContext;
		private ChipItem[] chips;

		private async Task HandleApplyClick()
		{
			await filterContext.UpdateModelAsync();
		}

		private void HandleChipsUpdated(ChipItem[] chips)
		{
			this.chips = chips;
		}

		private void HandleChipRemoveClick(ChipItem chipToRemove)
		{
			@filterContext.RemoveChip(chipToRemove);
		}

		#region Nested class FormModel
		public class FormModel : ICloneable
		{
			[Required]
			[MaxLength(50)]
			public string Text1 { get; set; }
			[Required]
			public string Text2 { get; set; }
			[Required]
			public string Text3 { get; set; }

			// TODO: Nahradit?
			public object Clone()
			{
				return new FormModel { Text1 = this.Text1, Text2 = this.Text2, Text3 = this.Text3 };
			}
		}
		#endregion

		private async Task HandleModelChanged(FormModel newModel)
		{
			model = newModel;
			//StateHasChanged();
			await myGrid.RefreshDataAsync();
		}

		private List<CultureInfo> GetCultureInfos()
		{
			return CultureInfo.GetCultures(CultureTypes.SpecificCultures).OrderBy(item => item.EnglishName /* only for skip! */).Skip(0).ToList();
		}

		private async Task<GridDataProviderResult<CultureInfo>> ClientCultureInfosDataProvider(GridDataProviderRequest<CultureInfo> request)
		{
			await Task.Delay(3000); // simulate server call

			var cultures = GetCultureInfos();
			return request.ApplyTo(cultures);
		}
	}
}
