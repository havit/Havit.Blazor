namespace Havit.Blazor.Components.Web.Bootstrap;

/// <summary>
/// Grid column base class.
/// </summary>
public abstract class HxGridColumnBase<TItem> : ComponentBase, IHxGridColumn<TItem>, IAsyncDisposable
{
	/// <summary>
	/// Cascading parameter to register column to the grid.
	/// </summary>
	[CascadingParameter(Name = HxGrid<TItem>.ColumnsRegistrationCascadingValueName)] protected CollectionRegistration<IHxGridColumn<TItem>> ColumnsRegistration { get; set; }

	/// <inheritdoc />
	protected override void OnInitialized()
	{
		base.OnInitialized();

		Contract.Requires(ColumnsRegistration != null, $"Grid column invalid usage. Must be used in a {typeof(HxGrid<TItem>).FullName}.");
		ColumnsRegistration.Register(this);
	}

	/// <inheritdoc />
	string IHxGridColumn<TItem>.GetId() => GetId();

	/// <inheritdoc />
	bool IHxGridColumn<TItem>.IsVisible() => IsColumnVisible();

	/// <inheritdoc />
	int IHxGridColumn<TItem>.GetOrder() => this.GetColumnOrder();

	/// <inheritdoc />
	GridCellTemplate IHxGridColumn<TItem>.GetHeaderCellTemplate(GridHeaderCellContext context) => this.GetHeaderCellTemplate(context);

	/// <inheritdoc />
	GridCellTemplate IHxGridColumn<TItem>.GetItemCellTemplate(TItem item) => this.GetItemCellTemplate(item);

	/// <inheritdoc />
	GridCellTemplate IHxGridColumn<TItem>.GetItemPlaceholderCellTemplate(GridPlaceholderCellContext context) => this.GetItemPlaceholderCellTemplate(context);

	/// <inheritdoc />
	GridCellTemplate IHxGridColumn<TItem>.GetFooterCellTemplate(GridFooterCellContext context) => this.GetFooterCellTemplate(context);

	/// <inheritdoc />
	SortingItem<TItem>[] IHxGridColumn<TItem>.GetSorting() => sorting ??= this.GetSorting().ToArray();
	private SortingItem<TItem>[] sorting;

	/// <inheritdoc />
	int? IHxGridColumn<TItem>.GetDefaultSortingOrder() => GetDefaultSortingOrder();

	/// <summary>
	/// Returns the unique column identifier.
	/// </summary>
	protected abstract string GetId();

	/// <summary>
	/// Indicates whether the column is visible (otherwise the column is hidden).
	/// It is not suitable to conditionally display the column using @if statement in the markup code.
	/// </summary>
	protected virtual bool IsColumnVisible() => true;

	/// <summary>
	/// Returns the column order.
	/// </summary>
	/// <remarks>
	/// Currently it ensures the correct order of MultiSelectGridColumn when enabled dynamically.		
	/// In future we can implement better in <see cref="HxGridColumn{TItem}"/> to enable dynamic column order.
	/// </remarks>
	protected abstract int GetColumnOrder();

	/// <summary>
	/// Returns header cell template.
	/// </summary>
	protected abstract GridCellTemplate GetHeaderCellTemplate(GridHeaderCellContext context);

	/// <summary>
	/// Returns data cell template for the specific item.
	/// </summary>
	protected abstract GridCellTemplate GetItemCellTemplate(TItem item);

	/// <summary>
	/// Returns placeholder cell template.
	/// </summary>
	protected abstract GridCellTemplate GetItemPlaceholderCellTemplate(GridPlaceholderCellContext context);

	/// <summary>
	/// Returns footer cell template.
	/// </summary>
	protected abstract GridCellTemplate GetFooterCellTemplate(GridFooterCellContext context);

	/// <summary>
	/// Returns column sorting.
	/// </summary>
	protected abstract IEnumerable<SortingItem<TItem>> GetSorting();

	/// <summary>
	/// Returns not-null value for default sort column.
	/// For multiple sort items, set value in ascendant order.
	/// </summary>
	/// <remarks>
	/// Current implementation of grid columns uses only null and zero values.
	/// </remarks>
	/// <example>
	/// To set default sorting by LastName, then FirstName use value 1 for LastName and value 2 for FirstName).
	/// </example>
	protected abstract int? GetDefaultSortingOrder();

	/// <inheritdoc />
	public async ValueTask DisposeAsync()
	{
		await DisposeAsyncCore();
	}

	protected virtual async Task DisposeAsyncCore()
	{
		await ColumnsRegistration.UnregisterAsync(this);
	}
}
