namespace BlazorAppTest.Pages;

public partial class HxListLayout_Issue962_Test
{
	[Inject] protected NavigationManager NavigationManager { get; set; }

	private DataItemDto _currentItem;
	private FilterModelDto _filterModel = new FilterModelDto();
	private HxGrid<DataItemDto> _gridComponent;

	private Task<GridDataProviderResult<DataItemDto>> LoadDataItems(GridDataProviderRequest<DataItemDto> request)
	{
		List<DataItemDto> data = new List<DataItemDto>()
		{
			new DataItemDto() { ItemId = 1, Name = "Edward P. Lopez", Email="EdwardPLopez@teleworm.us", PhoneNumber="918-483-4067", Age=45},
			new DataItemDto() { ItemId = 2, Name = "Stanley M. Bolduc", Email="StanleyMBolduc@armyspy.com", PhoneNumber="330-510-7016", Age=29},
			new DataItemDto() { ItemId = 3, Name = "David D. Lounsbury", Email="DavidDLounsbury@armyspy.com", PhoneNumber="212-292-4140", Age=38},
			new DataItemDto() { ItemId = 4, Name = "Benjamin M. Damato", Email="BenjaminMDamato@jourrapide.com", PhoneNumber="225-268-0659", Age=63},
			new DataItemDto() { ItemId = 5, Name = "Carol B. Jhonson", Email="CarolBJhonson@teleworm.us", PhoneNumber="217-243-3921", Age=25},
			new DataItemDto() { ItemId = 6, Name = "Juan I. Nichols", Email="JuanINichols@rhyta.com", PhoneNumber="727-562-0918", Age=60}
		};

		IEnumerable<DataItemDto> result = data.ToList();
		if (_filterModel.ComDomain is not null)
		{
			result = result.Where(i => i.Email.EndsWith(".com") == _filterModel.ComDomain);
		}

		return Task.FromResult(new GridDataProviderResult<DataItemDto>()
		{
			Data = result,
			TotalCount = result.Count()
		});
	}

	private async Task RefreshDataAsync()
	{
		await _gridComponent.RefreshDataAsync();
	}

	protected async Task SearchRequested()
	{
		await _gridComponent.RefreshDataAsync();
	}

	private Task DeleteItemClicked(DataItemDto dataItemDto)
	{
		_ = dataItemDto;
		return Task.CompletedTask;
	}

	private Task HandleSelectedDataItemChanged(DataItemDto selection)
	{
		_currentItem = selection;
		// await dataItemEditComponent.ShowAsync();
		return Task.CompletedTask;
	}

	private Task NewItemClicked()
	{
		_currentItem = new DataItemDto();
		// await dataItemEditComponent.ShowAsync();
		return Task.CompletedTask;
	}

	public record FilterModelDto
	{
		public bool? ComDomain { get; set; }
	}

	public record DataItemDto
	{
		public int ItemId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }
		public string PhoneNumber { get; set; }
		public int Age { get; set; }
	}

}
