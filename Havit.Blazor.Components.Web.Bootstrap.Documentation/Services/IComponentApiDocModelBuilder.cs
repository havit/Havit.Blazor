using Havit.Blazor.Components.Web.Bootstrap.Documentation.Model;

namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Services;
public interface IComponentApiDocModelBuilder
{
	ComponentApiDocModel BuildModel(Type type, bool isDelegate);
}