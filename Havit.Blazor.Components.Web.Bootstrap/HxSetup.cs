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
	public static string BootstrapVersion = "5.3.7";

	/// <summary>
	/// Renders the <c>&lt;script&gt;</c> tag that references the corresponding Bootstrap JavaScript bundle with Popper.<br/>
	/// To be used in <c>_Layout.cshtml</c> as <c>@Html.Raw(HxSetup.RenderBootstrapJavaScriptReference())</c>.
	/// </summary>
	/// <remarks>
	/// We do not want to use TagHelper or HTML Helper here as we do not want to introduce a dependency on server-side ASP.NET Core (MVC/Razor) to our library (a separate NuGet package would have to be created).
	/// </remarks>
	public static string RenderBootstrapJavaScriptReference()
	{
		return "<script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/js/bootstrap.bundle.min.js\" integrity=\"sha384-ndDqU0Gzau9qJ1lfW4pNLlhNTkCfHzAVBReH9diLvGRem5+R9g2FzA8ZGN954O5Q\" crossorigin=\"anonymous\"></script>";
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
			BootstrapFlavor.PlainBootstrap => "<link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.7/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-LN+7fdVzj6u52u30Kp6M/trliBMCMKTyK833zpbD+pXdCLuTusPj697FH4R/5mcr\" crossorigin=\"anonymous\">",
			_ => throw new ArgumentOutOfRangeException($"Unknown {nameof(BootstrapFlavor)} value {bootstrapFlavor}.")
		};
	}

	internal static string VersionIdentifierHavitBlazorBootstrap { get; } = Havit.Blazor.Components.Web.JSRuntimeExtensions.GetAssemblyVersionIdentifierForUri(typeof(HxSetup).Assembly);
}
