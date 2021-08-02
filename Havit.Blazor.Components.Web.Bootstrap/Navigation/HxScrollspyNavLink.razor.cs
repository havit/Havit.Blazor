using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Havit.Diagnostics.Contracts;
using Microsoft.AspNetCore.Components;

namespace Havit.Blazor.Components.Web.Bootstrap
{
	/// <summary>
	/// Temporary (?) NavLink component to be used with <see cref="HxScrollspy"/> where <code>#id</code> anchors are required and <code>page-route#id</code> cannot be used.
	/// </summary>
	public partial class HxScrollspyNavLink
	{
		/// <summary>
		/// The navigation target in <c>#id</c> form.
		/// </summary>
		// TODO [EditorRequired]
		[Parameter] public string Href { get; set; }

		/// <summary>
		/// Additional attributes to be splatted onto an underlying <code>&lt;a&gt;</code> element.
		/// </summary>
		[Parameter(CaptureUnmatchedValues = true)] public Dictionary<string, object> AdditionalAttributes { get; set; }

		[Parameter] public RenderFragment ChildContent { get; set; }

		[Inject] protected NavigationManager NavigationManager { get; set; }

		protected override void OnParametersSet()
		{
			Contract.Requires<InvalidOperationException>((Href is not null) && (Href.StartsWith("#")), $"{nameof(HxScrollspyNavLink)}.{nameof(HxScrollspyNavLink.Href)} has to start with #. Use only for local elements.");
		}

		private void HandleClick()
		{
			var targetUri = NavigationManager.ToAbsoluteUri(NavigationManager.Uri).GetLeftPart(UriPartial.Path) + Href;
			NavigationManager.NavigateTo(targetUri);
		}
	}
}
