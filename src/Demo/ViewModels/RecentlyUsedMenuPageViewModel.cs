using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;
using System.Collections.ObjectModel;

namespace DigitalProduction.Demo.ViewModels;

public partial class RecentlyUsedMenuPageViewModel : BaseViewModel
{
	#region Fields

	private readonly IDialogService     _dialogService;
	private readonly IMenuService       _menuService;

	#endregion

	#region Construction

	public RecentlyUsedMenuPageViewModel(IDialogService dialogService, IMenuService menuService)
	{
		_dialogService      = dialogService;
		_menuService        = menuService;

		RecentPathsManagerService = new RecentPathsManagerService();
		RecentPathsManagerService.PushTop(@"C:\Temp\File.txt");
		RecentPathsManagerService.PushTop(@"C:\Users\Lance\Notes.txt");
	}

	#endregion

	#region Properties


	[ObservableProperty]
	public partial IRecentPathsManagerService		RecentPathsManagerService { get; set; }

	[ObservableProperty]
	public partial bool								CanAddFlyoutItem { get; set; }					= true;

	[ObservableProperty]
	public partial bool								CanAddFlyoutSubItem { get; set; }				= true;

	public Page? MenuHostingPage
	{
		get => _menuService.HostingPage;
		set
		{
			_menuService.HostingPage = value;
			_dialogService.HostingPage = value;
		}
	}

	#endregion

	#region Methods

	private void ShowSelectedMessage(string commandName)
	{
		_dialogService.ShowMessage("MenuFlyoutItem clicked", $"Executing {commandName} showing this message.", "OK");
	}

	[RelayCommand]
	void AddFlyOutItem()
	{
		_menuService.AddMenuFlyoutItem("Menu Bar Item", "Added Item", () => ShowSelectedMessage("Added Item"));
		CanAddFlyoutItem = false;
	}

	[RelayCommand]
	void RemoveFlyOutItem()
	{
		_menuService.RemoveMenuFlyoutItem("Menu Bar Item", "Added Item");
		CanAddFlyoutItem = true;
	}

	[RelayCommand]
	void AddFlyOutSubItem()
	{
		_menuService.AddMenuFlyoutItemToSubMenu("Menu Flyout Sub Item", "Added Sub Item", () => ShowSelectedMessage("Added Sub Item"));
		CanAddFlyoutSubItem = false;

		IMenuFlyoutSubItem menuFlyoutSubItem = _menuService.GetSubMenu("Flyout")!;
		System.Diagnostics.Debug.WriteLine("");
		foreach (IMenuElement item in menuFlyoutSubItem)
		{
			System.Diagnostics.Debug.WriteLine(item.Text);
		}
	}

	[RelayCommand]
	void RemoveFlyOutSubItem()
	{
		_menuService.RemoveMenuFlyoutItemFromSubMenu("Menu Flyout Sub Item", "Added Sub Item");
		CanAddFlyoutSubItem = true;
	}

	#endregion
}