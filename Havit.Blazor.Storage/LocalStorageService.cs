using Havit.Blazor.Storage.Infrastructure;

namespace Havit.Blazor.Storage;

/// <inheritdoc cref="ILocalStorageService"/>
internal sealed class LocalStorageService(BrowserStorageAccessor browserStorageAccessor)
	: BrowserStorageService(browserStorageAccessor), ILocalStorageService
{
	protected override BrowserStorageType StorageType => BrowserStorageType.Local;
}