using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Services;
using System.Collections.ObjectModel;

namespace DigitalProduction.Demo.ViewModels;

public partial class RecentlyUsedSettingViewModel : ObservableObject
{
	private readonly IRecentPathsManagerService      _recentPathsManagerService;

	public RecentlyUsedSettingViewModel()
	{
		_recentPathsManagerService = Maui.Services.ServiceProvider.GetService<IRecentPathsManagerService>();
		Initialize();
	}

	[ObservableProperty]
	public partial bool							RemoveNotFoundPaths { get; set; }

	[ObservableProperty]
	public partial int							NumberOfItemsShown { get; set; }

	[ObservableProperty]
	public partial ObservableCollection<int>	DisplayedItemsNumbers { get; set; }		= [5, 6, 7, 8, 9, 10];

	[ObservableProperty]
	public partial int							MaximumNumberOfItems { get; set; }

	[ObservableProperty]
	public partial ObservableCollection<int>	MaximumItemsNumbers { get; set; }		= [10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];


	public void Initialize()
	{
		RemoveNotFoundPaths		= _recentPathsManagerService.RemoveNotFoundPaths;
		NumberOfItemsShown		= (int)_recentPathsManagerService.NumberOfItemsShown;
		MaximumNumberOfItems	= (int)_recentPathsManagerService.MaxSize;
	}

	public void Save()
	{
		_recentPathsManagerService.RemoveNotFoundPaths	= RemoveNotFoundPaths;
		_recentPathsManagerService.NumberOfItemsShown	= (uint)NumberOfItemsShown;
		_recentPathsManagerService.MaxSize				= (uint)MaximumNumberOfItems;
	}
}