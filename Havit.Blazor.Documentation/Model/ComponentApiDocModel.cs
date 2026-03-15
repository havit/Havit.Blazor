namespace Havit.Blazor.Documentation.Model;

public class ComponentApiDocModel
{
	public Type Type { get; set; }
	public ClassModel Class { get; set; }
	public bool IsEnum { get; set; }
	public bool IsDelegate { get; set; }
	public string DelegateSignature { get; set; }


	public List<PropertyModel> Properties { get; } = new();
	public List<PropertyModel> Parameters { get; } = new();
	public List<PropertyModel> StaticProperties { get; } = new();
	public List<PropertyModel> Events { get; } = new();

	public List<MethodModel> Methods { get; } = new();
	public List<MethodModel> StaticMethods { get; } = new();

	public List<EnumModel> EnumMembers { get; } = new();

	public bool HasValues => (Parameters is not null) || (Properties is not null) || (Events is not null) || (StaticMethods is not null) || (StaticProperties is not null);
}