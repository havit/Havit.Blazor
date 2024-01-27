using LoxSmoke.DocXml;

namespace Havit.Blazor.Documentation.Model;

public class ClassModel : MemberModel
{
	public TypeComments Comments
	{
		get => _comments;
		set
		{
			TypeComments inputComments = value;
			try { inputComments.Summary = TryFormatComment(inputComments.Summary); } catch { }
			_comments = inputComments;
		}
	}
	private TypeComments _comments;
}
