using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Havit.Blazor.Components.Web.Bootstrap.Tests.Grids;

[TestClass]
public class GridDataProviderRequestExtensionsTests
{
	[TestMethod]
	public void GridDataProviderRequestExtensions_ApplyGridDataProviderRequest_EntityFrameworkCore_CanBeCompiledForSqlServer()
	{
		// Arrange
		var gridDataProviderRequest = new GridDataProviderRequest<Item>
		{
			Sorting = new List<SortingItem<Item>>
			{
				new SortingItem<Item>(null, item => item.A, Collections.SortDirection.Ascending),
				new SortingItem<Item>(null, item => item.B, Collections.SortDirection.Descending),
			},
			StartIndex = 1, // zero based (skipping first item)
			Count = 3
		};

		var dbContext = new TestDbContext();
		var query = dbContext.Items.ApplyGridDataProviderRequest(gridDataProviderRequest);

#pragma warning disable EF1001 // Internal EF Core API usage.

		// Act

		// We just want to check if EF Core is able to compile the query.
		// We don't want to execute the query against the database (otherwise we need code migrations, database cleanup, etc.)
		_ = dbContext.GetService<IDatabase>().CompileQuery<SingleQueryingEnumerable<Item>>(query.Expression, false);

#pragma warning restore EF1001 // Internal EF Core API usage.

		// Assert
		// No exception "(IComparable)...) could not be translated. Either rewrite the query in a form that can be translated, ..." was thrown.
	}

	private class TestDbContext : DbContext // nested class
	{
		public DbSet<Item> Items { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			base.OnConfiguring(optionsBuilder);

			// fake connection string (we don't need a real database connection for this test)
			optionsBuilder.UseSqlServer("Data Source=FAKE");
		}
	}

	private class Item // nested class
	{
		public int Id { get; set; }

		public string A { get; set; }
		public int B { get; set; }
	}
}
