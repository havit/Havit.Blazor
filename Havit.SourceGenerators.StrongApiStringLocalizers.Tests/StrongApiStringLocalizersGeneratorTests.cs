﻿using System.Collections.Immutable;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio.TestPlatform.MSTest.TestAdapter.ObjectModel;

namespace Havit.SourceGenerators.StrongApiStringLocalizers.Tests;

[TestClass]
public class StrongApiStringLocalizersGeneratorTests
{
	[TestMethod]
	public async Task StrongApiStringLocalizersGenerator_Test()
	{
		using var globalResxStream = File.OpenRead("Global.resx");

		var test = new Microsoft.CodeAnalysis.CSharp.Testing.CSharpSourceGeneratorTest<StrongApiStringLocalizersGenerator, Microsoft.CodeAnalysis.Testing.DefaultVerifier>
		{
			TestState =
			{
				AnalyzerConfigFiles =
				{
					("/.editorconfig", $@"""
						is_global=true
						build_property.RootNamespace = MyApp.Resources
						build_property.ProjectDir = Z:\MyResources\
						""")
				}
			},
			ReferenceAssemblies = ReferenceAssemblies.Net
				.Net90
				.AddPackages(ImmutableArray.Create(
					new PackageIdentity("Microsoft.Extensions.Localization", "9.0.0"))) // we are using IStringLocalizer from this package in the generated code
		};

		// resource file
		test.TestState.AdditionalFiles.Add(("Z:\\MyResources\\MyResources\\Global.resx", SourceText.From(globalResxStream)));

		// EXPECTED OUTOUT

		// marker file
		test.TestState.GeneratedSources.Add((typeof(StrongApiStringLocalizersGenerator), "MyApp.Resources.MyResources.IGlobalLocalizer.g.cs", @"// <auto-generated />

namespace MyApp.Resources.MyResources;

using System.CodeDom.Compiler;
using Microsoft.Extensions.Localization;

[GeneratedCode(""Havit.SourceGenerators.StrongApiStringLocalizers.StrongApiStringLocalizersGenerator"", ""2.0.0.0"")]
public interface IGlobalLocalizer : IStringLocalizer
{
	/// <summary>
	/// Čeština je &lt;b&gt;skvělá&lt;/b&gt;!
	/// </summary>
	LocalizedString CzechAndHtml { get; }

	/// <summary>
	/// Hello world resource comment.
	/// </summary>
	LocalizedString HelloWorld { get; }

}
"));

		// TestProject - defined by the TestState implementation
		test.TestState.GeneratedSources.Add((typeof(StrongApiStringLocalizersGenerator), $"MyApp.Resources.MyResources.GlobalLocalizer.g.cs", @"// <auto-generated />

namespace MyApp.Resources.MyResources;

using System.CodeDom.Compiler;
using System.Collections.Generic;
using Microsoft.Extensions.Localization;

[GeneratedCode(""Havit.SourceGenerators.StrongApiStringLocalizers.StrongApiStringLocalizersGenerator"", ""2.0.0.0"")]
public class GlobalLocalizer : IGlobalLocalizer
{
	private readonly IStringLocalizer _localizer;

	public GlobalLocalizer(IStringLocalizerFactory stringLocalizerFactory)
	{
		_localizer = stringLocalizerFactory.Create(""MyResources.Global"", ""TestProject"");
	}

	/// <summary>
	/// Čeština je &lt;b&gt;skvělá&lt;/b&gt;!
	/// </summary>
	public LocalizedString CzechAndHtml => _localizer[""CzechAndHtml""];

	/// <summary>
	/// Hello world resource comment.
	/// </summary>
	public LocalizedString HelloWorld => _localizer[""HelloWorld""];

	LocalizedString IStringLocalizer.this[string name] => _localizer[name];
	LocalizedString IStringLocalizer.this[string name, params object[] arguments] => _localizer[name, arguments];
	IEnumerable<LocalizedString> IStringLocalizer.GetAllStrings(bool includeParentCultures) => _localizer.GetAllStrings(includeParentCultures);
}
"));

		test.TestState.GeneratedSources.Add((typeof(StrongApiStringLocalizersGenerator), "MyApp.Resources.ServiceCollectionExtensions.g.cs", @"// <auto-generated />

namespace MyApp.Resources;

using System.CodeDom.Compiler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

[GeneratedCode(""Havit.SourceGenerators.StrongApiStringLocalizers.StrongApiStringLocalizersGenerator"", ""2.0.0.0"")]
public static class ServiceCollectionExtensions
{
	public static IServiceCollection AddGeneratedResourceWrappers(this IServiceCollection services)
	{
		services.AddTransient<MyApp.Resources.MyResources.IGlobalLocalizer, MyApp.Resources.MyResources.GlobalLocalizer>();
		return services;
	}
}
"));

		await test.RunAsync();
	}
}
