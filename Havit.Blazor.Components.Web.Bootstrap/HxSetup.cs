using System.Diagnostics;

namespace Havit.Blazor.Components.Web.Bootstrap;

public static class HxSetup
{
	/// <summary>
	/// Global settings for the Havit Blazor Components library.
	/// </summary>
	public static GlobalSettings Defaults { get; } = new GlobalSettings();

	/// <summary>
	/// Bootstrap version used by the library.
	/// </summary>
	public static string BootstrapVersion = "6.0.0-alpha1";

	/// <summary>
	/// Renders the <c>&lt;script&gt;</c> tag that references the corresponding Bootstrap JavaScript bundle (with Floating UI).<br/>
	/// To be used in <c>_Layout.cshtml</c> as <c>@Html.Raw(HxSetup.RenderBootstrapJavaScriptReference())</c>.
	/// </summary>
	/// <remarks>
	/// Bootstrap 6 ships ES modules only — there is no UMD bundle and no implicit <c>window.bootstrap</c> global anymore.
	/// The bundle is shipped with this library (static web asset) to stay in lock-step with the compiled CSS
	/// (Bootstrap 6 alpha is not available on public CDNs yet).
	/// The rendered module script re-exposes <c>window.bootstrap</c> as a compatibility bridge for the Hx* JS modules
	/// (to be replaced by explicit imports as components are migrated to v6).
	/// Note: the library's JS initializer (<c>Havit.Blazor.Components.Web.Bootstrap.lib.module.js</c>, <c>beforeWebStart</c>/<c>beforeStart</c>)
	/// also establishes this bridge before the Blazor runtime starts, which guarantees availability for component JS interop regardless of
	/// module-script timing; this reference remains recommended so the bundle download starts as early as possible.<br/>
	/// We do not want to use TagHelper or HTML Helper here as we do not want to introduce a dependency on server-side ASP.NET Core (MVC/Razor) to our library (a separate NuGet package would have to be created).
	/// </remarks>
	public static string RenderBootstrapJavaScriptReference()
	{
		// window.bootstrap ??= : the library JS initializer (beforeWebStart/beforeStart) may have already established the bridge; guarding prevents a second module instance (different URL due to ?v=) with duplicate data-API listeners.
		return "<script type=\"module\">window.bootstrap ??= await import(\"./_content/Havit.Blazor.Components.Web.Bootstrap/bootstrap.bundle.min.js?v=" + VersionIdentifierHavitBlazorBootstrap + "\");</script>";
	}

	/// <summary>
	/// Renders the <c>&lt;link&gt;</c> tag that references the corresponding Bootstrap CSS.<br/>
	/// To be used in <c>_Layout.cshtml</c> as <c>@Html.Raw(HxSetup.RenderBootstrapCssReference())</c>.
	/// </summary>
	/// <remarks>
	/// We do not want to use TagHelper or HTML Helper here as we do not want to introduce a dependency on server-side ASP.NET Core (MVC/Razor) to our library (a separate NuGet package would have to be created).
	/// </remarks>
	public static string RenderBootstrapCssReference(BootstrapFlavor bootstrapFlavor = BootstrapFlavor.HavitDefault)
	{
		return bootstrapFlavor switch
		{
			BootstrapFlavor.HavitDefault => "<link href=\"_content/Havit.Blazor.Components.Web.Bootstrap/bootstrap.min.css?v=" + VersionIdentifierHavitBlazorBootstrap + "\" rel=\"stylesheet\" />",
			// Bootstrap 6 alpha is not published to npm/CDN yet; jsDelivr serves the pinned commit of the twbs/bootstrap GitHub repository (dist files are committed upstream).
			BootstrapFlavor.PlainBootstrap => "<link href=\"https://cdn.jsdelivr.net/gh/twbs/bootstrap@82b040e9f74c79430093a3cf46bd691825e7c10d/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-GUynUYOBgl+38vArVElig7kL4gR98li4lUFBUOjGxyBEPFIRjwr4Jhof/q9Pnkp5\" crossorigin=\"anonymous\">",
			_ => throw new ArgumentOutOfRangeException($"Unknown {nameof(BootstrapFlavor)} value {bootstrapFlavor}.")
		};
	}

	internal static string VersionIdentifierHavitBlazorBootstrap { get; } = Havit.Blazor.Components.Web.JSRuntimeExtensions.GetAssemblyVersionIdentifierForUri(typeof(HxSetup).Assembly);
}
