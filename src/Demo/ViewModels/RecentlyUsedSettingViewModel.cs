using CommunityToolkit.Mvvm.ComponentModel;
using DigitalProduction.Maui.Services;

namespace DigitalProduction.Demo.ViewModels;

public partial class RecentlyUsedSettingViewModel : ObservableObject
{
	private readonly IRecentPathsManagerService      _recentPathsManagerService;
//OnNumberOfItemsShownChanged

	public RecentlyUsedSettingViewModel()
	{
		_recentPathsManagerService = Maui.Services.ServiceProvider.GetService<IRecentPathsManagerService>();
		Initialize();
	}

	[ObservableProperty]
	public partial int		NumberOfItemsShown { get; set; }

	[ObservableProperty]
	public partial bool		RemoveNotFoundPaths { get; set; }

	public void Initialize()
	{
		NumberOfItemsShown	= (int)_recentPathsManagerService.NumberOfItemsShown;
		RemoveNotFoundPaths	= _recentPathsManagerService.RemoveNotFoundPaths;
	}

	public void Save()
	{
		_recentPathsManagerService.NumberOfItemsShown	= (uint)NumberOfItemsShown;
		_recentPathsManagerService.RemoveNotFoundPaths	= RemoveNotFoundPaths;
	}
}