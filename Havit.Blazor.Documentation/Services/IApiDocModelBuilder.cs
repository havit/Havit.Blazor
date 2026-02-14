using Havit.Blazor.Documentation.Model;

namespace Havit.Blazor.Documentation.Services;

public interface IApiDocModelBuilder
{
	ApiDocModel BuildModel(Type type);
}