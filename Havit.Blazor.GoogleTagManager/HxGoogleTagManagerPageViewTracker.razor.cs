using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Options;

namespace Havit.Blazor.GoogleTagManager;

/// <summary>
/// Initializes Google Tag Manager and tracks page-views to the GTM data-layer.
/// </summary>
/// <remarks>
/// Supports all render modes:
/// <list type="bullet">
///   <item><description>
///     <b>Static SSR and prerendering</b> — the GTM snippet is rendered inline, so GTM starts loading
///     while the page is still being parsed.
///   </description></item>
///   <item><description>
///     <b>Static SSR with enhanced navigation</b> — page-views are pushed by the
///     <c>Havit.Blazor.GoogleTagManager.lib.module.js</c> JS initializer, which listens for <c>enhancedload</c>.
///   </description></item>
///   <item><description>
///     <b>Interactive Server, WebAssembly and Auto</b> — page-views are pushed in reaction to
///     <see cref="NavigationManager.LocationChanged"/>, which covers navigations resolved by the interactive router.
///   </description></item>
/// </list>
/// The non-interactive render modes are supported on .NET 9 and later; on <c>net8.0</c> the component keeps its
/// original behavior and tracks page-views in interactive render modes only.
/// <para>
/// Initialization and automatic page-views are deduplicated in JavaScript, so rendering the component more
/// than once (e.g. in <c>App.razor</c> for the earliest possible GTM load and in the layout to cover
/// interactive routing) does not produce duplicate events.
/// </para>
/// <para>
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxGoogleTagManager">https://havit.blazor.eu/components/HxGoogleTagManager</see>
/// </para>
/// </remarks>
public partial class HxGoogleTagManagerPageViewTracker : IDisposable
{
	/// <summary>
	/// CSP nonce for the inline <c>&lt;script&gt;</c> tag rendered during static SSR and prerendering.
	/// Required when a Content Security Policy with <c>nonce-*</c> is in use.
	/// </summary>
	[Parameter] public string Nonce { get; set; }

	[Inject] protected NavigationManager NavigationManager { get; set; }
	[Inject] protected IHxGoogleTagManager HxGoogleTagManager { get; set; }
	[Inject] protected IOptions<HxGoogleTagManagerOptions> Options { get; set; }

	private LocationChangedEventArgs _locationChangedEventArgsToReportOnAfterRenderAsync;
	private bool _subscribedToLocationChanged;

	private bool IsInteractiveRendering =>
#if NET9_0_OR_GREATER
		(this.AssignedRenderMode is not null) && this.RendererInfo.IsInteractive;
#else
		true; // RendererInfo and AssignedRenderMode are .NET 9+, so .NET 8 keeps the original interactive-only behavior
#endif

	protected override void OnInitialized()
	{
		base.OnInitialized();

		if (this.IsInteractiveRendering)
		{
			this.NavigationManager.LocationChanged += this.OnLocationChanged;
			_subscribedToLocationChanged = true;
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		if (firstRender || (_locationChangedEventArgsToReportOnAfterRenderAsync is not null))
		{
			var argsToReport = _locationChangedEventArgsToReportOnAfterRenderAsync;
			_locationChangedEventArgsToReportOnAfterRenderAsync = null;
			await this.HxGoogleTagManager.PushPageViewAsync(argsToReport);
		}

		await base.OnAfterRenderAsync(firstRender);
	}

	private void OnLocationChanged(object sender, LocationChangedEventArgs args)
	{
		_locationChangedEventArgsToReportOnAfterRenderAsync = args;
		this.StateHasChanged();
	}

	/// <summary>
	/// Repeats what <c>HxGoogleTagManager.js</c> <c>initialize()</c> does, because the snippet has to run while
	/// the HTML is being parsed, long before any ES module can be imported. Both share the same
	/// <c>window.hxGoogleTagManager</c> state, so whichever runs first is the one that initializes GTM.
	/// </summary>
	private string GetInitializationScript()
	{
		var options = this.Options.Value;
		return $$"""
			(function () {
				var s = window.hxGoogleTagManager = window.hxGoogleTagManager || { initialized: false, config: null, lastPageViewUrl: null, initialPageViewHandled: false };
				s.config = {
					gtmId: {{ToJavaScriptString(options.GtmId)}},
					pageViewEventName: {{ToJavaScriptString(options.PageViewEventName)}},
					pageViewUrlVariableName: {{ToJavaScriptString(options.PageViewUrlVariableName)}},
					enableInitialPageViewTracking: {{(options.EnableInitialPageViewTracking ? "true" : "false")}}
				};
				if (s.initialized) {
					return;
				}
				s.initialized = true;
				window.dataLayer = window.dataLayer || [];
				dataLayer.push({ "gtm.start": new Date().getTime(), event: "gtm.js" });
				var j = document.createElement("script");
				j.async = true;
				j.src = "https://www.googletagmanager.com/gtm.js?id=" + s.config.gtmId;
				document.getElementsByTagName("head")[0].appendChild(j);
				dataLayer.push({ event: "pageview" });
				window.isGTM = true;
			})();
			""";
	}

	private static string ToJavaScriptString(string value)
	{
		return (value is null)
			? "null"
			: "\"" + JsonEncodedText.Encode(value) + "\"";
	}

	public void Dispose()
	{
		this.Dispose(true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && _subscribedToLocationChanged)
		{
			this.NavigationManager.LocationChanged -= this.OnLocationChanged;
		}
	}
}
