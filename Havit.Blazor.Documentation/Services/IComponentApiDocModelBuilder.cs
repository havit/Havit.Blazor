using Havit.Blazor.Documentation.Model;

namespace Havit.Blazor.Documentation.Services;

public interface IComponentApiDocModelBuilder
{
	ComponentApiDocModel BuildModel(Type type);
}