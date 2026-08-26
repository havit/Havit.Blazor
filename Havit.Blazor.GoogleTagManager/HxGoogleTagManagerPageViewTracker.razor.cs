using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.Extensions.Options;

namespace Havit.Blazor.GoogleTagManager;

/// <summary>
/// Initializes Google Tag Manager and tracks page-views to the GTM data-layer.
/// <para>
/// The component covers all render modes:
/// <list type="bullet">
///   <item><description>
///     <b>Static SSR and prerendering:</b> the GTM snippet is emitted inline as a <c>&lt;script&gt;</c> tag
///     directly into the HTML response (see the <c>.razor</c> file), so GTM starts loading while the page
///     is still being parsed. No JS interop is available in this phase.
///   </description></item>
///   <item><description>
///     <b>Static SSR with enhanced navigation:</b> the inline snippet is not executed again when Blazor
///     patches a new page into the DOM, so page-views for enhanced navigations are pushed by the
///     <c>Havit.Blazor.GoogleTagManager.lib.module.js</c> JS initializer, which listens for the
///     <c>enhancedload</c> event.
///   </description></item>
///   <item><description>
///     <b>Interactive rendering (Server, WebAssembly, Auto):</b> page-views are pushed in reaction to
///     <see cref="NavigationManager.LocationChanged"/>, which is what covers navigations handled by the
///     interactive router. When the page was prerendered, GTM is already initialized by the inline snippet;
///     otherwise the first push initializes it through JS interop.
///   </description></item>
/// </list>
/// </para>
/// <para>
/// Initialization and automatic page-views are deduplicated in JavaScript through shared
/// <c>window.hxGoogleTagManager</c> state, so more than one instance of the component (for example one in
/// <c>App.razor</c> for the earliest possible GTM load and one in the layout to cover interactive routing)
/// is safe and does not produce duplicate events.
/// </para>
/// Full documentation and demos: <see href="https://havit.blazor.eu/components/HxGoogleTagManager">https://havit.blazor.eu/components/HxGoogleTagManager</see>
/// </summary>
public partial class HxGoogleTagManagerPageViewTracker : IDisposable
{
	/// <summary>
	/// Optional CSP nonce to include on the inline <c>&lt;script&gt;</c> tag (static SSR/prerendering scenario).
	/// Required when a Content Security Policy with <c>nonce-*</c> is in use.
	/// </summary>
	[Parameter] public string Nonce { get; set; }

	[Inject] protected NavigationManager NavigationManager { get; set; }
	[Inject] protected IHxGoogleTagManager HxGoogleTagManager { get; set; }
	[Inject] protected IOptions<HxGoogleTagManagerOptions> Options { get; set; }

	private LocationChangedEventArgs _locationChangedEventArgsToReportOnAfterRenderAsync;
	private bool _subscribedToLocationChanged;

	/// <summary>
	/// <c>false</c> during static SSR and prerendering, where JS interop is not available and the inline
	/// snippet has to be rendered instead.
	/// </summary>
	private bool IsInteractiveRendering =>
#if NET9_0_OR_GREATER
		(this.AssignedRenderMode is not null) && this.RendererInfo.IsInteractive;
#else
		true; // RendererInfo/AssignedRenderMode are .NET 9+; on .NET 8 keep the original interactive-only behavior
#endif

	protected override void OnInitialized()
	{
		base.OnInitialized();

		if (IsInteractiveRendering)
		{
			NavigationManager.LocationChanged += OnLocationChanged;
			_subscribedToLocationChanged = true;
		}
	}

	protected override async Task OnAfterRenderAsync(bool firstRender)
	{
		// OnAfterRenderAsync is never called during static SSR, so this is the interactive path only.
		if (firstRender || (_locationChangedEventArgsToReportOnAfterRenderAsync is not null))
		{
			var argsToReport = _locationChangedEventArgsToReportOnAfterRenderAsync;
			_locationChangedEventArgsToReportOnAfterRenderAsync = null;
			await HxGoogleTagManager.PushPageViewAsync(argsToReport);
		}

		await base.OnAfterRenderAsync(firstRender);
	}

	private void OnLocationChanged(object sender, LocationChangedEventArgs args)
	{
		_locationChangedEventArgsToReportOnAfterRenderAsync = args;
		StateHasChanged();
	}

	/// <summary>
	/// Returns the inline GTM snippet for static SSR/prerendering.
	/// It intentionally repeats what <c>HxGoogleTagManager.js</c> <c>initialize()</c> does - the snippet has to run
	/// during HTML parsing, long before any ES module can be imported - and joins the same
	/// <c>window.hxGoogleTagManager</c> state, so whichever of the two runs first is the one that initializes GTM.
	/// </summary>
	private string GetInitializationScript()
	{
		var options = Options.Value;
		return $$"""
			(function () {
				var s = window.hxGoogleTagManager = window.hxGoogleTagManager || { initialized: false, config: null, lastPageViewUrl: null, initialPageViewHandled: false };
				s.config = { gtmId: {{ToJsString(options.GtmId)}}, pageViewEventName: {{ToJsString(options.PageViewEventName)}}, pageViewUrlVariableName: {{ToJsString(options.PageViewUrlVariableName)}}, enableInitialPageViewTracking: {{(options.EnableInitialPageViewTracking ? "true" : "false")}} };
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

	private static string ToJsString(string value)
	{
		return (value is null)
			? "null"
			: "\"" + JsonEncodedText.Encode(value) + "\"";
	}

	public void Dispose()
	{
		Dispose(true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing && _subscribedToLocationChanged)
		{
			NavigationManager.LocationChanged -= OnLocationChanged;
		}
	}
}
