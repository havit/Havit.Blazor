using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class HxChipGenerator : ComponentBase, IHxChipGenerator, IDisposable
	{
		[CascadingParameter(Name = HxFilterForm<object>.ChipGeneratorRegistrationCascadingValueName)] public CollectionRegistration<IHxChipGenerator> ChipGeneratorsRegistration { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter] public Action<object> ChipRemoveAction { get; set; }

		/// <inheritdoc cref="ComponentBase.OnInitialized" />
		protected override void OnInitialized()
		{
			base.OnInitialized();
			ChipGeneratorsRegistration?.Register(this);
		}

		IEnumerable<ChipItem> IHxChipGenerator.GetChips()
		{
			yield return new ChipItem
			{
				ChipTemplate = ChildContent,
				Removable = ChipRemoveAction != default,
				RemoveAction = ChipRemoveAction
			};
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				ChipGeneratorsRegistration?.Unregister(this);
			}
		}
	}
}
