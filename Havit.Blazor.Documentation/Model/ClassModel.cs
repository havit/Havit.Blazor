using LoxSmoke.DocXml;

namespace Havit.Blazor.Documentation.Model;

public class ClassModel : MemberModel
{
	public TypeComments Comments
	{
		get => comments;
		set
		{
			TypeComments inputComments = value;
			try { inputComments.Summary = TryFormatComment(inputComments.Summary); } catch { }
			comments = inputComments;
		}
	}
	private TypeComments comments;
}
