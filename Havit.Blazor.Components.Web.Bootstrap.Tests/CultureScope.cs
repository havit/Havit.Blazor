using System.Globalization;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests;

/// <summary>
/// Sets <see cref="CultureInfo.CurrentCulture"/> and <see cref="CultureInfo.CurrentUICulture"/> for the scope lifetime
/// and restores original values on dispose.
/// </summary>
internal sealed class CultureScope : IDisposable
{
	private readonly CultureInfo _originalCulture;
	private readonly CultureInfo _originalUICulture;

	public CultureScope(string cultureName)
		: this(CultureInfo.GetCultureInfo(cultureName))
	{
	}

	public CultureScope(CultureInfo culture)
	{
		_originalCulture = CultureInfo.CurrentCulture;
		_originalUICulture = CultureInfo.CurrentUICulture;

		CultureInfo.CurrentCulture = culture;
		CultureInfo.CurrentUICulture = culture;
	}

	public void Dispose()
	{
		CultureInfo.CurrentCulture = _originalCulture;
		CultureInfo.CurrentUICulture = _originalUICulture;
	}
}
