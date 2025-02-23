using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;

namespace DigitalProduction.Demo.ViewModels;

public partial class DynamicMenusPageViewModel(IDialogService dialogService, IMenuService menuService) : BaseViewModel
{
	#region Fields

	private readonly IDialogService		_dialogService		= dialogService;
	private readonly IMenuService		_menuService		= menuService;

	#endregion

	#region Properties

	[ObservableProperty]
	public partial bool					CanAddFlyoutItem { get; set; }		= true;

	[ObservableProperty]
	public partial bool					CanAddFlyoutSubItem { get; set; }	= true;

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
		_menuService.AddMenuFlyoutItem("Menu Flyout Item", "Added Item", () => ShowSelectedMessage("Added Item"));
		CanAddFlyoutItem = false;
	}

	[RelayCommand]
	void RemoveFlyOutItem()
	{
		_menuService.RemoveMenuFlyoutItem("Menu Flyout Item", "Added Item");
		CanAddFlyoutItem = true;
	}

	[RelayCommand]
	void AddFlyOutSubItem()
	{
		_menuService.AddMenuFlyoutItemToSubMenu("Flyout", "Added Sub Item", () => ShowSelectedMessage("Added Sub Item"));
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
		_menuService.RemoveMenuFlyoutItemFromSubMenu("Flyout", "Added Sub Item");
		CanAddFlyoutSubItem = true;
	}

	#endregion
}