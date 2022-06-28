﻿using Havit.Blazor.Components.Web.Bootstrap.Documentation.Model;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components;

public partial class ComponentApiDoc
{
	[Parameter] public RenderFragment ChildContent { get; set; }

	[Parameter] public RenderFragment MainContent { get; set; }

	[Parameter] public RenderFragment CssVariables { get; set; }

	/// <summary>
	/// A type to generate the documentation for
	/// </summary>
	[Parameter] public Type Type { get; set; }

	[Inject] protected IComponentApiDocModelBuilder ComponentApiDocModelBuilder { get; set; }

	private ComponentApiDocModel model;

	protected override void OnParametersSet()
	{
		model = ComponentApiDocModelBuilder.BuildModel(this.Type);
	}
}
