using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Components;
using System.Xml.XPath;
using LoxSmoke.DocXml;
using System.Text.RegularExpressions;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Web;
using System.Diagnostics;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Model;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;
using Havit.Blazor.Components.Web.Bootstrap.Documentation.Pages;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Shared.Components
{
	public partial class ComponentApiDoc
	{
		[Parameter] public RenderFragment ChildContent { get; set; }

		[Parameter] public RenderFragment MainContent { get; set; }

		[Parameter] public RenderFragment CssVariables { get; set; }

		/// <summary>
		/// A type to generate the documentation for
		/// </summary>
		[Parameter] public Type Type { get; set; }

		/// <summary>
		/// Names of members that will be excluded from the displayed documentation
		/// </summary>
		[Parameter] public List<string> ExcludedMembers { get; set; } = new();

		[Parameter] public bool Delegate { get; set; }

		[Inject] protected IComponentApiDocModelBuilder ComponentApiDocModelBuilder { get; set; }
		[Inject] protected ILogger<ComponentApiDoc> Logger { get; set; }

		private ComponentApiDocModel model;
		private TimeSpan? startTime;

		protected override void OnParametersSet()
		{
			startTime ??= DateTime.Now.TimeOfDay;

			model = ComponentApiDocModelBuilder.BuildModel(this.Type, this.Delegate, this.ExcludedMembers);

			Logger.LogWarning($"ComponentApiDoc({this.GetHashCode()})_DownloadFileAndGetSummaries: Elapsed {(DateTime.Now.TimeOfDay - startTime.Value).TotalMilliseconds} ms");
		}


		protected override void OnAfterRender(bool firstRender)
		{
			Logger.LogWarning($"ComponentApiDoc({this.GetHashCode()})_OnAfterRender: Elapsed {(DateTime.Now.TimeOfDay - startTime.Value).TotalMilliseconds} ms");
			startTime = null;
		}
	}
}
