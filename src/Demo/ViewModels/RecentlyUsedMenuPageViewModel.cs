using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;
using System.Collections.ObjectModel;

namespace DigitalProduction.Demo.ViewModels;

public partial class RecentlyUsedMenuPageViewModel : BaseViewModel
{
	#region Fields

	private readonly IDialogService     _dialogService;

	#endregion

	#region Construction

	public RecentlyUsedMenuPageViewModel(IDialogService dialogService, IMenuService menuService)
	{
		_dialogService      = dialogService;

		RecentPathsManagerService = new RecentPathsManagerService();

		// During the construction of the RecentPathsManagerService, it will restore any saved values.  If
		// No values were saved, then we will add some here for demonstration purposed (so we don't get a blank
		// control).  Normally, you would not do this.
		if (RecentPathsManagerService.GetRecentPaths().Count == 0)
		{
			RecentPathsManagerService.PushTop(@"C:\Temp\File.txt");
			RecentPathsManagerService.PushTop(@"C:\Users\Lance\Notes.txt");
		}
	}

	#endregion

	#region Properties


	[ObservableProperty]
	public partial IRecentPathsManagerService		RecentPathsManagerService { get; set; }

	//[ObservableProperty]
	//public partial bool								CanAddNewPath { get; set; }					= true;

	//[ObservableProperty]
	//public partial bool								CanAddFlyoutSubItem { get; set; }				= true;

	public Page? MenuHostingPage
	{
		get => _dialogService.HostingPage;
		set => _dialogService.HostingPage = value;
	}

	#endregion

	#region Methods

	[RelayCommand]
	void ShowSelectedMessage(string message)
	{
		_dialogService.ShowMessage("Menu clicked", $"The path \"{message}\" was selected.", "OK");
	}

	[RelayCommand]
	void AddNewPath()
	{
		RecentPathsManagerService.PushTop(@"C:\New\New File.txt");
	}

	[RelayCommand]
	void RemoveNewPath()
	{
		RecentPathsManagerService.RemovePath(@"C:\New\New File.txt");
	}

	[RelayCommand]
	void ResetPaths()
	{
		RecentPathsManagerService.RemovePath(@"C:\New\New File.txt");
	}

	//[RelayCommand]
	//void AddFlyOutSubItem()
	//{
	//	_menuService.AddMenuFlyoutItemToSubMenu("Menu Flyout Sub Item", "Added Sub Item", () => ShowSelectedMessage("Added Sub Item"));
	//	CanAddFlyoutSubItem = false;

	//	IMenuFlyoutSubItem menuFlyoutSubItem = _menuService.GetSubMenu("Flyout")!;
	//	System.Diagnostics.Debug.WriteLine("");
	//	foreach (IMenuElement item in menuFlyoutSubItem)
	//	{
	//		System.Diagnostics.Debug.WriteLine(item.Text);
	//	}
	//}

	//[RelayCommand]
	//void RemoveFlyOutSubItem()
	//{
	//	_menuService.RemoveMenuFlyoutItemFromSubMenu("Menu Flyout Sub Item", "Added Sub Item");
	//	CanAddFlyoutSubItem = true;
	//}

	#endregion
}