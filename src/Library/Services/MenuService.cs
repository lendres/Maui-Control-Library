using CommunityToolkit.Mvvm.Input;

#if MACCATALYST
	using UIKit;
#endif

namespace DigitalProduction.Maui.Services;

/// <summary>
/// Dynamic menu handling service.
/// 
/// <ContentPage.MenuBarItems>
/// 	<MenuBarItem Text="Menu Bar Item">
/// 		<MenuFlyoutItem Text="Menu Flyout Item" />
/// 		<MenuFlyoutSubItem Text="Menu Flyout Sub Item">
/// 			<MenuFlyoutItem Text="Menu Flyout Item" />
/// 		</MenuFlyoutSubItem>
/// 	</MenuBarItem>
/// </ContentPage.MenuBarItems>
/// </summary>
public class MenuService : IMenuService
{
	#region Properties
	
	public Page? HostingPage { get; set; }

	#endregion

	#region Menu Bar Items

	/// <summary>
	/// Get a menu bar item (MenuBarItem) from the menu bar.  These are the top level items in a menu bar.
	/// </summary>
	/// <param name="menuBarItemName">Name of the item to get.</param>
	/// <returns>The specified MenuBarItem.</returns>
	/// <exception cref="InvalidOperationException"></exception>
	public MenuBarItem GetMenuBarItem(string menuBarItemName)
	{
		if (HostingPage == null)
		{
			throw new InvalidOperationException($"{nameof(HostingPage)} must not be null.");
		}

		return HostingPage.MenuBarItems.ToList().SingleOrDefault(menuBarItem => menuBarItem.Text == menuBarItemName) ??
			throw new InvalidOperationException($"No MenuBarItem with text {menuBarItemName} was found.");
	}

	#endregion

	#region Items (MenuFlyoutItem) on Menu Bar Items (Top Level Menu Items)

	/// <summary>
	/// Checks if a MenuFlyoutItem is on a MenuBar.
	/// </summary>
	/// <param name="flyoutItemName">Name of the MenuFlyoutItem to check for.</param>
	/// <returns>True if it exists, false if not.</returns>
	public bool MenuFlyoutItemExists(string flyoutItemName) => GetMenuFlyoutItem(flyoutItemName) != null;

	/// <summary>
	/// Add a MenuFlyoutItem to a MenuMarItem.
	/// </summary>
	/// <param name="menuBarItemName">Menu bar item name (name of a top level item on the menu bar).</param>
	/// <param name="flyoutItemName">Name of thee MenuFlyoutItem to add.</param>
	/// <param name="execute">Command to execute on clicking.</param>
	/// <param name="position">Position to place the item.</param>
	/// <param name="modifiers">Keyboard accelerator modifiers.</param>
	/// <param name="shortCutKey">Keyboard short cut key.</param>
	/// <exception cref="InvalidOperationException">Thrown when a duplicate entry is found.</exception>
	public void AddMenuFlyoutItem(string menuBarItemName, string flyoutItemName, Action execute, int position = -1, KeyboardAcceleratorModifiers modifiers = KeyboardAcceleratorModifiers.None, string? shortCutKey = null)
	{
		MenuBarItem menuBarItem = GetMenuBarItem(menuBarItemName);
		AddMenuFlyoutItem(menuBarItem, flyoutItemName, execute, position, modifiers, shortCutKey);
	}

	/// <summary>
	/// Add a MenuFlyoutItem to a MenuMarItem.
	/// </summary>
	/// <param name="menuBarItem">The menu bar item (MenuBarItem).</param>
	/// <param name="flyoutItemName">Name of thee MenuFlyoutItem to add.</param>
	/// <param name="execute">Command to execute on clicking.</param>
	/// <param name="position">Position to place the item.</param>
	/// <param name="modifiers">Keyboard accelerator modifiers.</param>
	/// <param name="shortCutKey">Keyboard short cut key.</param>
	/// <exception cref="InvalidOperationException">Thrown when a duplicate entry is found.</exception>
	public void AddMenuFlyoutItem(MenuBarItem menuBarItem, string flyoutItemName, Action execute, int position = -1, KeyboardAcceleratorModifiers modifiers = KeyboardAcceleratorModifiers.None, string? shortCutKey = null)
	{
		if (menuBarItem.Any(element => element.Text == flyoutItemName))
		{
			throw new InvalidOperationException($"MenuBarItem with text {menuBarItem.Text} already contains an item with text '{flyoutItemName}'.");
		}

		MenuFlyoutItem itemToAdd = MakeMenuFlyoutItem(flyoutItemName, execute, modifiers, shortCutKey);

		if (position != -1)
		{
			menuBarItem.Insert(position, itemToAdd);
		}
		else
		{
			menuBarItem.Add(itemToAdd);
		}

		ForceMenuRebuild();
	}

	/// <summary>
	/// Gets a MenuFlyoutItem on the menu bar, if it  exists.  Will not find items in submenus.
	/// </summary>
	/// <param name="flyoutItemName">Name of the MenuFlyoutItem to search for.</param>
	/// <returns>MenuFlyoutItem if found, otherwise null.</returns>
	/// <exception cref="InvalidOperationException"></exception>
	public IMenuFlyoutItem? GetMenuFlyoutItem(string flyoutItemName)
	{
		if (HostingPage == null)
		{
			throw new InvalidOperationException($"{nameof(HostingPage)} must not be null.");
		}

		IMenuFlyoutItem? result = null;

		HostingPage.MenuBarItems.ToList().ForEach(menuBarItem =>
		{
			IMenuElement? foundItem = menuBarItem.SingleOrDefault(menuElement => menuElement is MenuFlyoutItem menuFlyoutItem && menuFlyoutItem.Text == flyoutItemName);

			if (foundItem != null)
			{
				result = foundItem as MenuFlyoutItem;
			}
		});

		return result;
	}

	/// <summary>
	/// Remove a MenuFlyoutItem from the specified MenuBarItem.
	/// </summary>
	/// <param name="menuBarItemName">Name of the MenuBarItem to remove the MenuFlyoutItem from.</param>
	/// <param name="menuFlyoutItemName">Name of the MenuFlyoutItem to remove.</param>
	/// <exception cref="InvalidOperationException">Thrown when the hosting page, menu bar item, or menu flyout item is not found.</exception>
	public void RemoveMenuFlyoutItem(string menuBarItemName, string menuFlyoutItemName)
	{
		MenuBarItem menuBarItem = GetMenuBarItem(menuBarItemName);
		RemoveMenuFlyoutItem(menuBarItem, menuFlyoutItemName);
	}

	/// <summary>
	/// Remove a MenuFlyoutItem from the specified MenuBarItem.
	/// </summary>
	/// <param name="menuBarItem">The MenuBarItem to remove the MenuFlyoutItem from.</param>
	/// <param name="menuFlyoutItemName">Name of the MenuFlyoutItem to remove.</param>
	/// <exception cref="InvalidOperationException">Thrown when the hosting page, menu bar item, or menu flyout item is not found.</exception>
	public void RemoveMenuFlyoutItem(MenuBarItem menuBarItem, string menuFlyoutItemName)
	{
		IMenuFlyoutItem? itemToRemove = GetMenuFlyoutItem(menuFlyoutItemName) ??
			throw new InvalidOperationException($"No MenuFlyoutItem with text {menuFlyoutItemName} found in MenuBarItem with text {menuBarItem.Text}.");
		menuBarItem.Remove(itemToRemove);
		ForceMenuRebuild();
	}

	#endregion

	#region Submenus (MenuFlyoutSubItem) on Menu Bar Items (Top Level Menu Items)

	public bool SubMenuExists(string subMenuName) => GetSubMenu(subMenuName) != null;

	/// <summary>
	/// Gets a MenuFlyoutSubItem.
	/// </summary>
	/// <param name="subMenuName"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException">Thown when the hosting page is null.</exception>
	public IMenuFlyoutSubItem? GetSubMenu(string subMenuName)
	{
		if (HostingPage == null)
		{
			throw new InvalidOperationException($"{nameof(HostingPage)} must not be null.");
		}

		IMenuFlyoutSubItem? result = null;

		HostingPage.MenuBarItems.ToList().ForEach(menuBarItem =>
		{
			IMenuElement? foundItem = menuBarItem.SingleOrDefault(menuElement => menuElement is MenuFlyoutSubItem subMenu && subMenu.Text == subMenuName);

			if (foundItem != null)
			{
				result = foundItem as MenuFlyoutSubItem;
			}
		});

		return result;
	}

	#endregion

	#region Flyout on Submenu

	/// <summary>
	/// Checks if a MenuFlyoutItem is on a submenu (MenuFlyoutSubItem).
	/// </summary>
	/// <param name="parentSubMenuName">The parent sub menu (MenuFlyoutSubItem).</param>
	/// <param name="flyoutItemName">Name of thee MenuFlyoutItem to add.</param>
	/// <returns>True if it exists, false otherwise.</returns>
	public bool MenuFlyoutItemInSubMenuExists(string parentSubMenuName, string flyoutItemName) => GetMenuFlyoutItemInSubMenu(parentSubMenuName, flyoutItemName) != null;

	/// <summary>
	/// Checks if a MenuFlyoutItem is on a submenu (MenuFlyoutSubItem).
	/// </summary>
	/// <param name="parentSubMenu">The parent sub menu (MenuFlyoutSubItem).</param>
	/// <param name="flyoutItemName">Name of thee MenuFlyoutItem to add.</param>
	/// <returns>True if it exists, false otherwise.</returns>
	public bool MenuFlyoutItemInSubMenuExists(IMenuFlyoutSubItem parentSubMenu, string flyoutItemName) => GetMenuFlyoutItemInSubMenu(parentSubMenu, flyoutItemName) != null;

	/// <summary>
	/// Adds a MenuFlyoutItem to submenu (MenuFlyoutSubItem).
	/// </summary>
	/// <param name="parentSubMenuName">The name of the parent sub menu (MenuFlyoutSubItem).</param>
	/// <param name="flyoutItemName">Name of thee MenuFlyoutItem to add.</param>
	/// <param name="execute">Command to execute on clicking.</param>
	/// <param name="position">Position to place the item.</param>
	/// <param name="modifiers">Keyboard accelerator modifiers.</param>
	/// <param name="shortCutKey">Keyboard short cut key.</param>
	/// <exception cref="InvalidOperationException"></exception>
	public void AddMenuFlyoutItemToSubMenu(string parentSubMenuName, string flyoutItemName, Action execute, int position = -1, KeyboardAcceleratorModifiers modifiers = KeyboardAcceleratorModifiers.None, string? shortCutKey = null)
	{
		IMenuFlyoutSubItem? parentSubMenu = GetSubMenu(parentSubMenuName) ??
			throw new InvalidOperationException($"No MenuFlyoutSubItem with text {parentSubMenuName} was found.");
		AddMenuFlyoutItemToSubMenu(parentSubMenu, flyoutItemName, execute, position, modifiers, shortCutKey);
	}

	/// <summary>
	/// Adds a MenuFlyoutItem to submenu (MenuFlyoutSubItem).
	/// </summary>
	/// <param name="parentSubMenu">The parent sub menu (MenuFlyoutSubItem).</param>
	/// <param name="flyoutItemName">Name of thee MenuFlyoutItem to add.</param>
	/// <param name="execute">Command to execute on clicking.</param>
	/// <param name="position">Position to place the item.</param>
	/// <param name="modifiers">Keyboard accelerator modifiers.</param>
	/// <param name="shortCutKey">Keyboard short cut key.</param>
	/// <exception cref="InvalidOperationException"></exception>
	public void AddMenuFlyoutItemToSubMenu(IMenuFlyoutSubItem parentSubMenu, string flyoutItemName, Action execute, int position = -1, KeyboardAcceleratorModifiers modifiers = KeyboardAcceleratorModifiers.None, string? shortCutKey = null)
	{
		if (MenuFlyoutItemInSubMenuExists(parentSubMenu, flyoutItemName))
		{
			throw new InvalidOperationException($"MenuFlyoutSubItem with text {parentSubMenu.Text} already contains an item with text '{flyoutItemName}'.");
		}

		MenuFlyoutItem itemToAdd = MakeMenuFlyoutItem(flyoutItemName, execute, modifiers, shortCutKey);

		if (position != -1)
		{
			parentSubMenu.Insert(position, itemToAdd);
		}
		else
		{
			parentSubMenu.Add(itemToAdd);
		}

		ForceMenuRebuild();
	}

	/// <summary>
	/// Finds a MenuFlyoutItem on the submenu, if it  exists.
	/// </summary>
	/// <param name="parentSubMenuName">The name of the parent submenu (MenuFlyoutSubItem) to search.</param>
	/// <param name="flyoutItemName">Name of the MenuFlyoutItem to search for.</param>
	/// <returns>MenuFlyoutItem if found, otherwise null.</returns>
	/// <exception cref="InvalidOperationException">Thrown if hosting page is null or parent sub menu is not found.</exception>
	public IMenuFlyoutItem? GetMenuFlyoutItemInSubMenu(string parentSubMenuName, string flyoutItemName)
	{
		IMenuFlyoutSubItem?	parentSubMenu = GetSubMenu(parentSubMenuName)  ??
			throw new InvalidOperationException($"No MenuFlyoutSubItem with text {parentSubMenuName} was found.");
		return GetMenuFlyoutItemInSubMenu(parentSubMenu, flyoutItemName);
	}

	/// <summary>
	/// Gets a MenuFlyoutItem from a submenu (MenuFlyoutSubItem), if it exists.
	/// </summary>
	/// <param name="parentSubMenu">The parent submenu (MenuFlyoutSubItem).</param>
	/// <param name="flyoutItemName">Name of the MenuFlyoutItem to search for.</param>
	/// <returns>The MenuFlyoutItem if it was found, null otherwise.</returns>
	public IMenuFlyoutItem? GetMenuFlyoutItemInSubMenu(IMenuFlyoutSubItem parentSubMenu, string flyoutItemName)
	{
		IMenuElement? result = parentSubMenu.SingleOrDefault(element => element.Text == flyoutItemName);
		return result as MenuFlyoutItem;
	}

	/// <summary>
	/// Removes a MenuFlyoutItem on the submenu, if it  exists.
	/// </summary>
	/// <param name="parentSubMenuName">The name of the parent submenu (MenuFlyoutSubItem) to search.</param>
	/// <param name="flyoutItemName">Name of the MenuFlyoutItem to search for.</param>
	/// <exception cref="InvalidOperationException"></exception>
	public void RemoveMenuFlyoutItemFromSubMenu(string parentSubMenuName, string flyoutItemName)
	{
		IMenuFlyoutSubItem? parentSubMenu = GetSubMenu(parentSubMenuName) ??
			throw new InvalidOperationException($"No MenuFlyoutSubItem with text {parentSubMenuName} was found.");
		RemoveMenuFlyoutItemFromSubMenu(parentSubMenu, flyoutItemName);
	}

	/// <summary>
	/// Removes a MenuFlyoutItem on the submenu, if it  exists.
	/// </summary>
	/// <param name="parentSubMenu">The parent submenu (MenuFlyoutSubItem).</param>
	/// <param name="flyoutItemName">Name of the MenuFlyoutItem to search for.</param>
	/// <exception cref="InvalidOperationException">Thrown if the item does not exist.</exception>
	public void RemoveMenuFlyoutItemFromSubMenu(IMenuFlyoutSubItem parentSubMenu, string flyoutItemName)
	{
		IMenuFlyoutItem? itemToRemove = GetMenuFlyoutItemInSubMenu(parentSubMenu, flyoutItemName) ??
			throw new InvalidOperationException($"No MenuFlyoutItem with text {flyoutItemName} in parent MenuFlyoutSubItem with text {parentSubMenu.Text} was found.");
		parentSubMenu.Remove(itemToRemove);
		ForceMenuRebuild();
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Create a MenuFlyoutItem
	/// </summary>
	/// <param name="flyoutItemName">Name of thee MenuFlyoutItem to add.</param>
	/// <param name="execute">Command to execute on clicking.</param>
	/// <param name="modifiers">Keyboard accelerator modifiers.</param>
	/// <param name="shortCutKey">Keyboard short cut key.</param>
	/// <returns>A new MenuFlyoutItem.</returns>
	private static MenuFlyoutItem MakeMenuFlyoutItem(string flyoutItemName, Action execute, KeyboardAcceleratorModifiers modifiers, string? shortCutKey)
	{
		MenuFlyoutItem itemToAdd = new()
		{
			Text    = flyoutItemName,
			Command = new RelayCommand(execute)
		};

		if (modifiers != KeyboardAcceleratorModifiers.None && !string.IsNullOrWhiteSpace(shortCutKey))
		{
			itemToAdd.KeyboardAccelerators.Add(new KeyboardAccelerator()
			{
				Modifiers	= modifiers,
				Key			= shortCutKey
			});
		}

		return itemToAdd;
	}

	#endregion

	#region Platform

	private void ForceMenuRebuild()
	{
		#if MACCATALYST
            UIMenuSystem.MainSystem.SetNeedsRebuild();
		#endif
	}

	#endregion
}