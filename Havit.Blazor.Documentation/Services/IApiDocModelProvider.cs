using Havit.Blazor.Documentation.Model;

namespace Havit.Blazor.Documentation.Services;

/// <summary>
/// Provides <see cref="ApiDocModel"/> instances built from <see cref="IApiDocModelBuilder"/>.
/// </summary>
public interface IApiDocModelProvider
{
	/// <summary>
	/// Returns a cached <see cref="ApiDocModel"/> for the specified type.
	/// </summary>
	ApiDocModel GetApiDocModel(Type type);
}
