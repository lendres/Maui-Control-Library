using DigitalProduction.Maui.Services;
using System.Windows.Input;
#if MACCATALYST
	using UIKit;
#endif

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

    public static readonly BindableProperty PathCommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(RecentlyUsedMenuFlyout), null,
			propertyChanged: (bindable, oldObject, newObject) =>
            {
				System.Diagnostics.Debug.Assert(newObject != null);

                if (newObject == oldObject || bindable is not RecentlyUsedMenuFlyout self)
                {
                    return;
                }

				if (self.RecentPathsManagerService != null)
				{
					self.UpdateFlyoutItems(self.RecentPathsManagerService.GetRecentPaths());
				}
            }
		);

	public ICommand PathCommand
	{
		get => (ICommand)GetValue(PathCommandProperty);
		set => SetValue(PathCommandProperty, value);
	}

	#endregion

	#region Methods

	private void UpdateFlyoutItems(List<string> paths)
	{
		Clear();
		CreateFlyoutItems(paths);
		ForceMenuRebuild();
	}

	private void CreateFlyoutItems(List<string> paths)
	{
		// Generate all the menu item instances.
		for (int i = 0; i < paths.Count; i++)
		{
			MenuFlyoutPath menuFlyoutPath = new()
			{
				Number						= i,
				Path						= paths[i],
				Command						= PathCommand,
				RecentPathsManagerService	= RecentPathsManagerService
			};
			this.Add(menuFlyoutPath);
		}
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

} // End class.