using System.Reflection;
using LoxSmoke.DocXml;

namespace Havit.Blazor.Documentation.Model;

public class PropertyModel : MemberModel
{
	public PropertyInfo PropertyInfo { get; set; }
	public bool EditorRequired { get; set; }

	public CommonComments Comments
	{
		set
		{
			CommonComments inputComments = value;
			try { inputComments.Summary = TryFormatComment(inputComments.Summary, PropertyInfo.DeclaringType); } catch { }
			_comments = inputComments;
		}
		get
		{
			return _comments;
		}
	}
	private CommonComments _comments;

	public bool IsStatic => PropertyInfo.GetAccessors(false).Any(o => o.IsStatic);
}
