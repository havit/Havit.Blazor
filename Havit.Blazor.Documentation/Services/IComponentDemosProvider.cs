namespace Havit.Blazor.Documentation.Services;

/// <summary>
/// Provides access to demo resources embedded in the documentation assembly.
/// </summary>
public interface IComponentDemosProvider
{
	/// <summary>
	/// Returns the file names of all demos for the specified component.
	/// </summary>
	IReadOnlyList<string> GetComponentDemoFileNames(string componentName);

	/// <summary>
	/// Returns the resource names of all demo samples for the specified component.
	/// </summary>
	IReadOnlyList<string> GetComponentDemoResourceNames(string componentName);

	/// <summary>
	/// Reads the content of a demo by its file name.
	/// Returns <c>null</c> if the sample is not found.
	/// </summary>
	string GetDemoContentByFileName(string demoFileName);

	/// <summary>
	/// Reads the content of a demo sample by its full embedded resource name.
	/// </summary>
	string GetDemoContentByResourceName(string resourceName);
}
