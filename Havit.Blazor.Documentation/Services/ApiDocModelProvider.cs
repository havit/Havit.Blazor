using System.Collections.Concurrent;
using Havit.Blazor.Documentation.Model;

namespace Havit.Blazor.Documentation.Services;

/// <summary>
/// Caches <see cref="ApiDocModel"/> instances built by <see cref="IApiDocModelBuilder"/>.
/// Uses <see cref="Lazy{T}"/> to prevent stampede (multiple concurrent builds for the same type).
/// </summary>
public class ApiDocModelProvider : IApiDocModelProvider
{
	private readonly ConcurrentDictionary<Type, Lazy<ApiDocModel>> _cache = new();
	private readonly IApiDocModelBuilder _modelBuilder;

	public ApiDocModelProvider(IApiDocModelBuilder modelBuilder)
	{
		_modelBuilder = modelBuilder;
	}

	/// <inheritdoc />
	public ApiDocModel GetApiDocModel(Type type)
	{
		Lazy<ApiDocModel> lazy = _cache.GetOrAdd(
			type,
			static (t, builder) => new Lazy<ApiDocModel>(() => builder.BuildModel(t)),
			_modelBuilder);

		return lazy.Value;
	}
}
