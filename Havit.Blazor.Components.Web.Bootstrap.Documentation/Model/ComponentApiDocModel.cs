namespace Havit.Blazor.Components.Web.Bootstrap.Documentation.Model;

public class ComponentApiDocModel
{
	public ClassModel Class { get; set; }
	public bool IsEnum { get; set; }
	public string DelegateSignature { get; set; }


	public List<PropertyModel> Properties { get; set; }
	public List<PropertyModel> Parameters { get; set; }
	public List<PropertyModel> StaticProperties { get; set; }
	public List<PropertyModel> Events { get; set; }

	public List<MethodModel> Methods { get; set; }
	public List<MethodModel> StaticMethods { get; set; }

	public List<EnumModel> EnumMembers { get; } = new();
}