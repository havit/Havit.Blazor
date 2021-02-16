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
	/// <summary>
	/// Edit form wrapper which provides strong type model and model instance update when valid form is submitted.
	/// </summary>
	public class HxModelEditForm<TModel> : ComponentBase
	{
		/// <summary>
		/// Model.
		/// </summary>
		[Parameter] public TModel Model { get;set; }

		/// <summary>
		/// Model event callback. Invoked when valid form is updated.
		/// </summary>
		[Parameter] public EventCallback<TModel> ModelChanged { get; set; }
		
		/// <summary>
		/// Invoked when valid form is submitted.
		/// </summary>
		[Parameter] public EventCallback<EditContext> OnValidSubmit { get; set; }

		/// <summary>
		/// Child content.
		/// </summary>
		[Parameter] public RenderFragment<TModel> ChildContent { get; set; }

		/// <summary>
		/// Model in edit (clone of Model).
		/// </summary>
		protected TModel ModelInEdit { get; set; }

		private TModel previousModel;

		protected override void OnParametersSet()
		{
			base.OnParametersSet();

			if (!EqualityComparer<TModel>.Default.Equals(previousModel, Model))
			{				
				ModelSet();
				previousModel = Model;
			}
		}

		/// <summary>
		/// Fired when a new model is set from outside (databind, etc).
		/// </summary>
		protected virtual void ModelSet()
		{
			// we are going to let user edit a clone of the model
			ModelInEdit = CloneModel(Model);
		}

		/// <summary>
		/// Updates Model by current ModelInEdit.
		/// </summary>
		public virtual async Task UpdateModelAsync()
		{
			Model = ModelInEdit; 
			previousModel = Model; // to suppress cloning Model in OnParametersSet, must be before ModelChanged is invoked!
			await ModelChanged.InvokeAsync(ModelInEdit);

			ModelInEdit = CloneModel(ModelInEdit);
		}

		/// <inheritdoc />
		protected override void BuildRenderTree(RenderTreeBuilder builder)
		{
			builder.OpenComponent<EditForm>(0);
			builder.AddAttribute(1, nameof(EditForm.Model), ModelInEdit);
			builder.AddAttribute(2, nameof(EditForm.OnValidSubmit), EventCallback.Factory.Create<EditContext>(this, HandleValidSubmit));
			builder.AddAttribute(3, nameof(EditForm.ChildContent), (RenderFragment<EditContext>)((EditContext _) => ChildContent?.Invoke(ModelInEdit)));
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
