using Moq;

namespace Havit.Blazor.Components.Web.Tests;

public class ModelClonerTests
{
	[Fact]
	public void ModelCloner_TryCloneCloneable()
	{
		// Arrange
		Mock<ICloneable> modelMock = new Mock<ICloneable>(MockBehavior.Strict);
		modelMock.Setup(m => m.Clone()).Returns(() => modelMock.Object); // we do not need true clone (in this test).

		// Act
		bool success = ModelCloner.TryCloneCloneable(modelMock.Object, out ICloneable modelClone);

		// Assert
		Assert.True(success);
		modelMock.Verify(m => m.Clone(), Times.Once()); // Clone was called
	}


	[Fact]
	public void ModelCloner_TryCloneRecord()
	{
		// Arrange
		ModelRecord model = new ModelRecord { Name = "Mr. Unit Test" };

		// Act
		bool success = ModelCloner.TryCloneRecord(model, out ModelRecord modelClone);

		// Assert
		Assert.True(success);
		Assert.NotSame(model, modelClone);
		Assert.Equal(model.Name, modelClone.Name);
	}

	[Fact]
	public void ModelCloner_TryCloneRecordWithList_WithItem()
	{
		// Arrange
		var model = new ModelRecordWithList();
		model.Tags.Add("tag1");

		// Act
		var modelClone = ModelCloner.Clone(model);

		// Assert
		Assert.NotSame(model, modelClone);
		Assert.Equal(model, modelClone);
		Assert.True(modelClone.Tags.SequenceEqual(model.Tags));
	}

	[Fact]
	public void ModelCloner_Clone_RecordWithList_Empty()
	{
		// Arrange
		var model = new ModelRecordWithList();

		// Act
		var modelClone = ModelCloner.Clone(model);

		// Assert
		Assert.NotSame(model, modelClone);
		Assert.Equal(model, modelClone);
		Assert.True(modelClone.Tags.SequenceEqual(model.Tags));
	}

	[Fact]
	public void ModelCloner_CloneMemberwiseClone()
	{
		// Arrange
		ModelClass model = new ModelClass { Name = "Mr. Unit Test" };

		// Act
		ModelClass modelClone = ModelCloner.CloneMemberwiseClone(model);

		// Assert
		Assert.NotSame(model, modelClone);
		Assert.Equal(model.Name, modelClone.Name);
	}

	public record ModelRecord
	{
		public string Name { get; set; }
	}

	public class ModelClass
	{
		public string Name { get; set; }
	}

	public record ModelRecordWithList
	{
		public List<string> Tags { get; set; } = new();

		// due to Tags property, we need to override Equals
		public virtual bool Equals(ModelRecordWithList other)
		{
			return (other is not null)
				&& Tags.SequenceEqual(other.Tags);
		}

		// due to Equals method, we need to override GetHashCode
		public override int GetHashCode()
		{
			var hashCode = new HashCode();
			foreach (var tag in Tags)
			{
				hashCode.Add(tag);
			}
			return hashCode.ToHashCode();
		}
	}

}
