using Microsoft.Extensions.Localization;

namespace Havit.Extensions.Localization;

/// <summary>
/// Base-class for delegating IStringLocalizer implementations.
/// To be used for implementation of strong-API localizers.
/// </summary>
public abstract class DelegatingStringLocalizer<T> : IStringLocalizer<T>
{
	protected readonly IStringLocalizer<T> InnerLocalizer;

	protected DelegatingStringLocalizer(IStringLocalizer<T> innerLocalizer)
	{
		this.InnerLocalizer = innerLocalizer;
	}

	/// <inheritdoc/>
	public virtual LocalizedString this[string name] => InnerLocalizer[name];
	/// <inheritdoc/>
	public virtual LocalizedString this[string name, params object[] arguments] => InnerLocalizer[name, arguments];
	/// <inheritdoc/>
	public virtual IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures = false) => InnerLocalizer.GetAllStrings(includeParentCultures);
}
