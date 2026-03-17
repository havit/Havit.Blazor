using Havit.Blazor.ApplicationInsights.Options;

namespace Havit.Blazor.ApplicationInsights.Tests.Options;

[TestClass]
public class BlazorApplicationInsightsClientOptionsTests
{
	[TestMethod]
	public void BlazorApplicationInsightsClientOptions_MergeTo_ThrowsOnNullTarget()
	{
		// Arrange
		var source = new BlazorApplicationInsightsClientOptions();

		// Act & assert
		Assert.ThrowsExactly<ArgumentNullException>(() => source.MergeTo(null));
	}

	[TestMethod]
	public void BlazorApplicationInsightsClientOptions_MergeTo_SetsNullPropertiesFromSource()
	{
		// Arrange
		var source = new BlazorApplicationInsightsClientOptions
		{
			ConnectionString = "source-connection",
			AccountId = "source-account",
			SamplingPercentage = 50
		};
		var target = new BlazorApplicationInsightsClientOptions();

		// Act
		source.MergeTo(target);

		// Assert
		Assert.AreEqual("source-connection", target.ConnectionString);
		Assert.AreEqual("source-account", target.AccountId);
		Assert.AreEqual(50, target.SamplingPercentage);
	}

	[TestMethod]
	public void BlazorApplicationInsightsClientOptions_MergeTo_DoesNotOverwriteAlreadySetProperties()
	{
		// Arrange
		var source = new BlazorApplicationInsightsClientOptions
		{
			ConnectionString = "source-connection",
			AccountId = "source-account",
			SamplingPercentage = 50
		};
		var target = new BlazorApplicationInsightsClientOptions
		{
			ConnectionString = "target-connection",
			AccountId = "target-account",
			SamplingPercentage = 100
		};

		// Act
		source.MergeTo(target);

		// Assert
		Assert.AreEqual("target-connection", target.ConnectionString);
		Assert.AreEqual("target-account", target.AccountId);
		Assert.AreEqual(100, target.SamplingPercentage);
	}

	[TestMethod]
	public void BlazorApplicationInsightsClientOptions_MergeTo_MergesPartially()
	{
		// Arrange — target has ConnectionString set, AccountId is null
		var source = new BlazorApplicationInsightsClientOptions
		{
			ConnectionString = "source-connection",
			AccountId = "source-account"
		};
		var target = new BlazorApplicationInsightsClientOptions
		{
			ConnectionString = "target-connection"
		};

		// Act
		source.MergeTo(target);

		// Assert
		Assert.AreEqual("target-connection", target.ConnectionString); // not overwritten
		Assert.AreEqual("source-account", target.AccountId);           // filled from source
	}

	[TestMethod]
	public void BlazorApplicationInsightsClientOptions_MergeTo_DoesNotCopyNullSourceProperties()
	{
		// Arrange — source has null ConnectionString, target has it set
		var source = new BlazorApplicationInsightsClientOptions
		{
			AccountId = "source-account"
		};
		var target = new BlazorApplicationInsightsClientOptions
		{
			ConnectionString = "target-connection"
		};

		// Act
		source.MergeTo(target);

		// Assert
		Assert.AreEqual("target-connection", target.ConnectionString); // unchanged
		Assert.AreEqual("source-account", target.AccountId);
	}

	[TestMethod]
	public void BlazorApplicationInsightsClientOptions_MergeTo_WorksWithInheritedConfigurationProperties()
	{
		// Arrange — InstrumentationKey is defined on ApplicationInsightsConfiguration (base of base)
		var source = new BlazorApplicationInsightsClientOptions
		{
			InstrumentationKey = "source-ikey"
		};
		var target = new BlazorApplicationInsightsClientOptions();

		// Act
		source.MergeTo(target);

		// Assert
		Assert.AreEqual("source-ikey", target.InstrumentationKey);
	}
}
