using System.Windows.Input;

namespace MauiDynamicMenus;

public interface IMenuService
{
	Page? HostingPage { get;  set; }

    bool MenuFlyoutItemExists(string name);
    void AddMenuFlyoutItem(string menu, string name, Action execute, int position = -1, KeyboardAcceleratorModifiers modifiers = KeyboardAcceleratorModifiers.None, string? shortCutKey = null);
    IMenuFlyoutItem? GetMenuFlyoutItem(string name);
    void RemoveMenuFlyoutItem(string menu, string name);

    bool SubMenuExists(string name);
    IMenuFlyoutSubItem? GetSubMenu(string name);

    bool MenuFlyoutItemInSubMenuExists(string parentSubMenu, string name);
    void AddMenuFlyoutItemToSubMenu(string parentSubMenu, string name, Action execute, int position = -1, KeyboardAcceleratorModifiers modifiers = KeyboardAcceleratorModifiers.None, string? shortCutKey = null);
    IMenuFlyoutItem? GetMenuFlyoutItemInSubMenu(string parentSubMenu, string name);
    void RemoveMenuFlyoutItemFromSubMenu(string parentSubMenu, string name);
}