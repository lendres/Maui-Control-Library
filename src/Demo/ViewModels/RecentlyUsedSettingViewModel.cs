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
	public partial double						NumberOfItemsShown { get; set; }

	[ObservableProperty]
	public partial double						NumberOfItemsToStore { get; set; }

	public void Initialize()
	{
		RemoveNotFoundPaths		= _recentPathsManagerService.RemoveNotFoundPaths;
		NumberOfItemsShown		= (double)_recentPathsManagerService.NumberOfItemsShown;
		NumberOfItemsToStore	= (double)_recentPathsManagerService.MaxSize;
	}

	partial void OnNumberOfItemsToStoreChanged(double value)
	{
		System.Diagnostics.Debug.WriteLine("Number of items to store changed: " + value.ToString());
	}

	public void Save()
	{
		_recentPathsManagerService.RemoveNotFoundPaths	= RemoveNotFoundPaths;
		_recentPathsManagerService.NumberOfItemsShown	= (uint)NumberOfItemsShown;
		_recentPathsManagerService.MaxSize				= (uint)NumberOfItemsToStore;
	}
}