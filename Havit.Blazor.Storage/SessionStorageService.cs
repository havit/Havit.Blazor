using Havit.Blazor.Storage.Infrastructure;

namespace Havit.Blazor.Storage;

/// <inheritdoc cref="ISessionStorageService"/>
internal sealed class SessionStorageService(BrowserStorageAccessor browserStorageAccessor)
	: BrowserStorageService(browserStorageAccessor), ISessionStorageService
{
	protected override BrowserStorageType StorageType => BrowserStorageType.Session;
}