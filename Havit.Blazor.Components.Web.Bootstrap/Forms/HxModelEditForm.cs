using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	public class HxModelEditForm<TModel> : ComponentBase
	{
		[Parameter] public TModel Model { get;set; }
		[Parameter] public EventCallback<TModel> ModelChanged { get; set; }
		
		[Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }

		[Parameter] public RenderFragment<TModel> ChildContent { get; set; }

		private TModel modelInEdit;
		private TModel previousModel;



		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			if (!EqualityComparer<TModel>.Default.Equals(previousModel, Model))
			{
				// we are going to let user edit a clone of the model
				modelInEdit = CloneModel(Model);
				previousModel = Model;
			}
		}

		public async Task UpdateModelAsync()
		{
			Model = modelInEdit; 
			previousModel = Model; // to suppress cloning Model in OnParametersSet, must be before ModelChanged is invoked!
			await ModelChanged.InvokeAsync(modelInEdit);

			modelInEdit = CloneModel(Model);
		}

		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent<EditForm>(0);
			builder.AddAttribute(1, nameof(EditForm.Model), modelInEdit);
			builder.AddAttribute(2, nameof(EditForm.OnValidSubmit), EventCallback.Factory.Create<EditContext>(this, HandleValidSubmit));
			builder.AddAttribute(3, nameof(EditForm.ChildContent), (RenderFragment<EditContext>)((EditContext _) => ChildContent?.Invoke(modelInEdit)));
			builder.CloseComponent();
		}

		private async Task HandleValidSubmit(EditContext editContext)
		{
			await UpdateModelAsync();
			await OnValidSubmit.InvokeAsync(editContext);
		}

		internal static TModel CloneModel(TModel modelToClone)
		{
			return (TModel)((ICloneable)modelToClone).Clone(); // TODO: strategie klonování
		}
	}
}
