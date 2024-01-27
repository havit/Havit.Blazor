using Microsoft.AspNetCore.Components.Forms;

namespace Havit.Blazor.Components.Web;

/// <summary>
/// Edit form wrapper which provides a strongly typed model and updates the model instance when a valid form is submitted.
/// </summary>
public class HxModelEditForm<TModel> : ComponentBase
{
	/// <summary>
	/// Form element id.
	/// </summary>
	[Parameter] public string Id { get; set; }

	/// <summary>
	/// Model.
	/// </summary>
	[Parameter] public TModel Model { get; set; }

	/// <summary>
	/// Model event callback. Invoked when a valid form is updated.
	/// </summary>
	[Parameter] public EventCallback<TModel> ModelChanged { get; set; }
	/// <summary>
	/// Triggers the <see cref="ModelChanged"/> event. Allows interception of the event in derived components.
	/// </summary>
	protected virtual Task InvokeModelChangedAsync(TModel newValue) => ModelChanged.InvokeAsync(newValue);

	/// <summary>
	/// Child content.
	/// </summary>
	[Parameter] public RenderFragment<TModel> ChildContent { get; set; }

	/// <summary>
	/// Model being edited (a clone of the Model).
	/// </summary>
	protected TModel ModelInEdit { get; set; }

	private TModel _previousModel;

	protected override void OnParametersSet()
	{
		base.OnParametersSet();

		if (!EqualityComparer<TModel>.Default.Equals(_previousModel, Model))
		{
			OnModelSet();
			_previousModel = Model;
		}
	}

	/// <summary>
	/// Fired when a new model is set from outside (data bind, etc.).
	/// </summary>
	protected virtual void OnModelSet()
	{
		// We are going to let the user edit a clone of the model.
		ModelInEdit = CloneModel(Model);
	}

	/// <summary>
	/// Updates the Model with the current ModelInEdit.
	/// </summary>
	public virtual async Task UpdateModelAsync()
	{
		Model = ModelInEdit;
		_previousModel = Model; // To suppress cloning the Model in OnParametersSet, this must be done before ModelChanged is invoked!
		await InvokeModelChangedAsync(Model);

		ModelInEdit = CloneModel(ModelInEdit);
		StateHasChanged(); // We are changing the state - ModelInEdit.
	}

	/// <inheritdoc cref="ComponentBase.BuildRenderTree(RenderTreeBuilder)" />
	protected override void BuildRenderTree(RenderTreeBuilder builder)
	{
		builder.OpenComponent<EditForm>(0);
		builder.AddAttribute(1, nameof(EditForm.Model), ModelInEdit);
		builder.AddAttribute(2, nameof(EditForm.OnValidSubmit), EventCallback.Factory.Create<EditContext>(this, HandleValidSubmit));
		builder.AddAttribute(3, nameof(EditForm.ChildContent), (RenderFragment<EditContext>)((EditContext _) => ChildContent?.Invoke(ModelInEdit)));
		builder.AddAttribute(4, nameof(Id), Id);
		builder.AddAttribute(5, "class", "hx-form");
		builder.CloseComponent();
	}

	private async Task HandleValidSubmit(EditContext editContext)
	{
		await UpdateModelAsync();
	}

	protected internal static TModel CloneModel(TModel modelToClone)
	{
		return ModelCloner.Clone(modelToClone);
	}
}
