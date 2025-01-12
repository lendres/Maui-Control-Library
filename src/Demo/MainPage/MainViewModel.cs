using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiDynamicMenus;

namespace DynamicMenuDemo;

public partial class MainViewModel : ObservableObject
{
	#region Fields

	private readonly IDialogService		_dialogService;
	private readonly IMenuService		_menuService;

	[ObservableProperty]
	private bool						_canAddFlyoutItem;

	[ObservableProperty]
	private bool						_canRemoveFlyoutItem;

	[ObservableProperty]
	private bool						_canAddFlyoutSubItem;

	[ObservableProperty]
	private bool						_canRemoveFlyoutSubItem;

	#endregion

	#region Construction

	public MainViewModel(IDialogService dialogService, IMenuService menuService)
    {
		_dialogService		= dialogService;
		_menuService		= menuService;

		CanAddFlyoutItem	= true;
		CanAddFlyoutSubItem	= true;
	}

	#endregion

	#region Properties

	public Page? MenuHostingPage
	{
		get => _menuService.HostingPage;
		set => _menuService.HostingPage = value;
	}

	#endregion

	partial void OnCanAddFlyoutItemChanged(bool value)
	{
		CanRemoveFlyoutItem = !value;
	}

	partial void OnCanAddFlyoutSubItemChanged(bool value)
	{
		CanRemoveFlyoutSubItem = !value;
	}

	private void ShowSelectedMessage(string commandName)
	{
		_dialogService.ShowMessage("MenuFlyoutItem clicked", $"Executing {commandName} showing this message.", "OK");
	}

	[RelayCommand]
	void AddFlyOutItem()
	{
		_menuService.AddMenuFlyoutItem("Menu Flyout Item", "Added Item", ()=> ShowSelectedMessage("Added Item"));
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
		_menuService.AddMenuFlyoutItemToSubMenu("Flyout", "Added Sub Item", ()=> ShowSelectedMessage("Added Sub Item"));
		CanAddFlyoutSubItem = false;

		IMenuFlyoutSubItem menuFlyoutSubItem = _menuService.GetSubMenu("Flyout")!;
		System.Diagnostics.Debug.WriteLine("");
		foreach (var item in menuFlyoutSubItem)
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
}