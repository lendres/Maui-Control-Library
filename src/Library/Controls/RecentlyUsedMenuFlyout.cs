using DigitalProduction.Maui.Services;
using Microsoft.Maui.Controls;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace DigitalProduction.Maui.Controls;

/// <summary>
/// A class for creating a recently used files list on a menu.
/// </summary>
public partial class RecentlyUsedMenuFlyout : MenuFlyoutSubItem
{
	#region Fields

	// Services.
	private IMenuService						_menuService					= new MenuService();
	//private IRecentPathsManagerService?			_pathStorageService;

	// Menu item the list is attached to.
	//private readonly List<MenuFlyoutItem>       _recentlyUsedSubMenus            = new();

	#endregion

	#region Construction

	/// <summary>
	/// Basic constructor.
	/// </summary>
	public RecentlyUsedMenuFlyout()
	{
	}

	#endregion

	#region Bindable Properties

    public static readonly BindableProperty RecentPathsManagerServiceProperty =
        BindableProperty.Create(nameof(RecentPathsManagerService), typeof(IRecentPathsManagerService), typeof(RecentlyUsedMenuFlyout), null,
			propertyChanged: (bindable, oldObject, newObject) =>
            {
				System.Diagnostics.Debug.Assert(newObject != null);

                if (newObject == oldObject || bindable is not RecentlyUsedMenuFlyout self)
                {
                    return;
                }

                // Disconnect events.
				if (oldObject != null)
				{
					IRecentPathsManagerService oldService = oldObject as IRecentPathsManagerService ??
						throw new InvalidOperationException($"Invalid object, expected {nameof(IRecentPathsManagerService)}.");
					oldService.OnPathsChanged -= self.UpdateFlyoutItems;
				}

				// Connect events.
				IRecentPathsManagerService newService = newObject as IRecentPathsManagerService ??
					throw new InvalidOperationException($"Invalid object, expected {nameof(IRecentPathsManagerService)}.");
				self.UpdateFlyoutItems(newService.GetRecentPaths());
				newService.OnPathsChanged += self.UpdateFlyoutItems;
            }
		);

	public IRecentPathsManagerService RecentPathsManagerService
	{
		get => (RecentPathsManagerService)GetValue(RecentPathsManagerServiceProperty);
		set => SetValue(RecentPathsManagerServiceProperty, value);
	}

	#endregion

	#region Private Controls Manipulation Functions

	private void UpdateFlyoutItems(List<string> paths)
	{
		Clear();
		CreateFlyoutItems(paths);
	}

	private void CreateFlyoutItems(List<string> paths)
	{
		// Generate all the menu item instances.
		for (int i = 0; i < paths.Count; i++)
		{
			string fileNumber = (i+1).ToString();
			string name = fileNumber + " " + System.IO.Path.GetFileName(paths[i])  + " (" + paths[i] + ")";
			_menuService.AddMenuFlyoutItemToSubMenu(this, name, () => ShowDebugMessage(name));
		}
	}

	private void ShowDebugMessage(string commandName)
	{
		System.Diagnostics.Debug.WriteLine($"Executing {commandName} showing this message.");
	}

	#endregion

} // End class.