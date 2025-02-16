using System.Windows.Input;

namespace DigitalProduction.Maui.Services;

public interface IMenuService
{
	Page? HostingPage { get;  set; }

	// Menu bar items.
	public MenuBarItem GetMenuBarItem(string menuBarItemName);

	// Items (MenuFlyoutItem) on menu bar items (top level menu items).
    bool MenuFlyoutItemExists(string flyoutItemName);

    void AddMenuFlyoutItem(string menuBarItemName, string flyoutItemName, Action execute, int position = -1, KeyboardAcceleratorModifiers modifiers = KeyboardAcceleratorModifiers.None, string? shortCutKey = null);
	void AddMenuFlyoutItem(MenuBarItem menuBarItem, string flyoutItemName, Action execute, int position = -1, KeyboardAcceleratorModifiers modifiers = KeyboardAcceleratorModifiers.None, string? shortCutKey = null);

    IMenuFlyoutItem? GetMenuFlyoutItem(string flyoutItemName);

	void RemoveMenuFlyoutItem(string menuBarItemName, string menuFlyoutItemName);
	void RemoveMenuFlyoutItem(MenuBarItem menuBarItem, string menuFlyoutItemName);

	// Submenus (MenuFlyoutSubItem) on menu bar items (top level menu items).
    bool SubMenuExists(string subMenuName);
    IMenuFlyoutSubItem? GetSubMenu(string subMenuName);

	// Flyout on submenu.
    bool MenuFlyoutItemInSubMenuExists(string parentSubMenuName, string flyoutItemName);
	bool MenuFlyoutItemInSubMenuExists(IMenuFlyoutSubItem parentSubMenu, string flyoutItemName);

    void AddMenuFlyoutItemToSubMenu(string parentSubMenuName, string flyoutItemName, Action execute, int position = -1, KeyboardAcceleratorModifiers modifiers = KeyboardAcceleratorModifiers.None, string? shortCutKey = null);
	void AddMenuFlyoutItemToSubMenu(IMenuFlyoutSubItem parentSubMenu, string flyoutItemName, Action execute, int position = -1, KeyboardAcceleratorModifiers modifiers = KeyboardAcceleratorModifiers.None, string? shortCutKey = null);

    IMenuFlyoutItem? GetMenuFlyoutItemInSubMenu(string parentSubMenuName, string flyoutItemName);
	IMenuFlyoutItem? GetMenuFlyoutItemInSubMenu(IMenuFlyoutSubItem parentSubMenu, string flyoutItemName);

    void RemoveMenuFlyoutItemFromSubMenu(string parentSubMenuName, string flyoutItemName);
	void RemoveMenuFlyoutItemFromSubMenu(IMenuFlyoutSubItem parentSubMenu, string flyoutItemName);
}