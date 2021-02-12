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
		protected FormModel model = new FormModel();
		
		private FilterContext<FormModel> filterContext;
		private ChipItem[] chips;

		private async Task HandleApplyClick()
		{
			await filterContext.UpdateModelAsync();
		}

		private void HandleChipsUpdated(ChipItem[] chips)
		{
			this.chips = chips;
			StateHasChanged();
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
	}
}
