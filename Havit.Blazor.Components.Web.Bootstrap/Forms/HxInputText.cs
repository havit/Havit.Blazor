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
		
		[CascadingParameter(Name = HxFilterForm<object>.ChipGeneratorRegistrationCascadingValueName)] public CollectionRegistration<IHxChipGenerator> ChipGeneratorsRegistration { get; set; }

		/// <inheritdoc />
		protected override void OnInitialized()
		{
			base.OnInitialized();
			ChipGeneratorsRegistration?.Register(this);
		}

		// TODO: Do by šlo hezky do bázové třídy :-)
		public async Task<ChipItem[]> GetChipsAsync()
		{
			await Task.Delay(100);

			if (String.IsNullOrEmpty(CurrentValueAsString))
			{
				return null;
			}

			return new[]
			{
				new ChipItem
				{
					ChipTemplate = RenderFragmentBuilder.CreateFrom(Label + ":" + CurrentValueAsString, null),
					Removable = true,
					RemoveCallback = (model) => model.GetType().GetProperty(this.FieldIdentifier.FieldName).SetValue(model, String.Empty)
				}
			};
		}

		public void Dispose()
		{
			ChipGeneratorsRegistration?.Unregister(this);
		}
	}
}
