using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Text input (also password, search, etc.)
	/// </summary>
	public class HxInputText : HxInputTextBase, IHxChipGenerator, IDisposable
	{
		/// <summary>
		/// Input type.
		/// </summary>
		[Parameter] public InputType Type { get; set; } = InputType.Text;

		/// <inheritdoc />
		private protected override string GetElementName() => "input";

		/// <inheritdoc />
		private protected override string GetTypeAttributeValue() => Type.ToString().ToLower();
		
		[CascadingParameter(Name = FilterContext<object>.ChipGeneratorRegistrationCascadingValueName)] public CollectionRegistration<IHxChipGenerator> ChipGeneratorsRegistration { get; set; }

		/// <inheritdoc />
		protected override void OnInitialized()
		{
			base.OnInitialized();
			ChipGeneratorsRegistration?.Register(this);
		}

		// TODO: Do by šlo hezky do bázové třídy :-)
		public Task<ChipItem[]> GetChipsAsync()
		{
			if (String.IsNullOrEmpty(CurrentValueAsString))
			{
				return Task.FromResult(new ChipItem[0]);
			}

			return Task.FromResult(new[]
			{
				new ChipItem
				{
					FieldIdentifier = this.FieldIdentifier,
					ChipTemplate = RenderFragmentBuilder.CreateFrom(Label + ":" + CurrentValueAsString, null),
					CanBeRemoved = true
				}
			});
		}

		public Task<bool> TryRemoveChipAsync(ChipItem chipToRemove)
		{
			// TODO: Možná ještě budeme porovnávat podle jiného klíče, zvalidovat možnost Expression?
			if (!EqualityComparer<FieldIdentifier>.Default.Equals(chipToRemove.FieldIdentifier, default) && String.Equals(this.FieldIdentifier.FieldName, chipToRemove.FieldIdentifier.FieldName, StringComparison.OrdinalIgnoreCase))
			{
				CurrentValue = String.Empty;
				Task.FromResult(true);
			}

			return Task.FromResult(false);
		}

		public void Dispose()
		{
			ChipGeneratorsRegistration?.Unregister(this);
		}
	}
}
