namespace Havit.Blazor.Documentation.Model;

public class EnumModel : MemberModel
{
	public int Value { get; set; }
	public string Name { get; set; }

	public string Summary
	{
		get
		{
			return _summary;
		}
		set
		{
			try
			{
				_summary = TryFormatComment(value);
			}
			catch
			{
				_summary = value;
			}

		}
	}
	private string _summary;
}
