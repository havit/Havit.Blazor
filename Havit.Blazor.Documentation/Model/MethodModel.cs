using System.Reflection;
using Havit.Blazor.Documentation.Services;
using LoxSmoke.DocXml;

namespace Havit.Blazor.Documentation.Model;

public class MethodModel : MemberModel
{
	public MethodInfo MethodInfo { get; set; }

	public MethodComments Comments
	{
		set
		{
			MethodComments inputComments = value;
			try { inputComments.Summary = TryFormatComment(inputComments.Summary); } catch { }
			comments = inputComments;
		}
		get
		{
			return comments;
		}
	}
	private MethodComments comments;

	public string GetParameters()
	{
		StringBuilder concatenatedParameters = new StringBuilder();
		var parameters = MethodInfo.GetParameters();

		if (parameters is null || parameters.Length == 0)
		{
			return "()";
		}

		concatenatedParameters.Append("(");
		foreach (var parameter in parameters)
		{
			concatenatedParameters.Append($"{ApiRenderer.FormatType(parameter.ParameterType)} {parameter.Name}, ");
		}
		concatenatedParameters.Remove(concatenatedParameters.Length - 2, 2);
		concatenatedParameters.Append(")");

		return concatenatedParameters.ToString();
	}
}
