using Havit.Blazor.Components.Web.Bootstrap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorAppTest.Pages
{
	public partial class HxFilterFormTest
	{
		protected FormModel model = new FormModel { Text1 = "initial value" };
		private HxGrid<string> myGrid;

		private HxFilterForm<FormModel> filterForm;
		private ChipItem[] chips;

		private void HandleChipsUpdated(ChipItem[] chips)
		{
			this.chips = chips;
		}

		private async Task HandleChipRemoveClick(ChipItem chipToRemove)
		{
			await filterForm.RemoveChipAsync(chipToRemove);
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

		private async Task<GridDataProviderResult<string>> GridDataProvider(GridDataProviderRequest<string> request)
		{
			await Task.Delay(3000); // simulate server call

			var stringValues = new List<string>();
			stringValues.Add(model.Text1);
			stringValues.Add(model.Text2);
			stringValues.Add(model.Text3);
			return request.ApplyTo(stringValues);
		}
	}
}
