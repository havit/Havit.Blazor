namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Model
{
	public class EnumModel : MemberModel
	{
		public int Index { get; set; }
		public string Name { get; set; }

		public string Summary
		{
			get
			{
				return summary;
			}
			set
			{
				try
				{
					summary = TryFormatComment(value);
				}
				catch
				{
					summary = value;
				}

			}
		}
		private string summary;
	}
}
