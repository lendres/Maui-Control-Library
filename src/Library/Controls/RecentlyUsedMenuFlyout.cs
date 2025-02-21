using DigitalProduction.Maui.Services;
using System.Windows.Input;

namespace DigitalProduction.Maui.Controls;

/// <summary>
/// A class for creating a recently used files list on a menu.
/// </summary>
public partial class RecentlyUsedMenuFlyout : MenuFlyoutSubItem
{
	#region Fields
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

    public static readonly BindableProperty PathCommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(RecentlyUsedMenuFlyout), null,
			propertyChanged: (bindable, oldObject, newObject) =>
            {
				System.Diagnostics.Debug.Assert(newObject != null);

                if (newObject == oldObject || bindable is not RecentlyUsedMenuFlyout self)
                {
                    return;
                }

				self.CheckAndBuildMenus();
            }
		);

	public ICommand PathCommand
	{
		get => (ICommand)GetValue(PathCommandProperty);
		set => SetValue(PathCommandProperty, value);
	}

    public static readonly BindableProperty PathNotFoundCommandProperty =
        BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(RecentlyUsedMenuFlyout), null,
			propertyChanged: (bindable, oldObject, newObject) =>
            {
				System.Diagnostics.Debug.Assert(newObject != null);

                if (newObject == oldObject || bindable is not RecentlyUsedMenuFlyout self)
                {
                    return;
                }

				self.CheckAndBuildMenus();
            }
		);

	public ICommand PathNotFoundCommand
	{
		get => (ICommand)GetValue(PathNotFoundCommandProperty);
		set => SetValue(PathNotFoundCommandProperty, value);
	}

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
					oldService.OnMaxSizeChanged		-= self.OnMaxSizeChanged;
					oldService.OnMaxSizeChanged		-= self.OnNumberOfItemsShownChanged;
					oldService.OnPathsChanged		-= self.OnPathsChanged;
				}

				// Connect events.
				if (newObject != null)
				{
					IRecentPathsManagerService newService = newObject as IRecentPathsManagerService ??
					throw new InvalidOperationException($"Invalid object, expected {nameof(IRecentPathsManagerService)}.");
					newService.OnMaxSizeChanged     += self.OnMaxSizeChanged;
					newService.OnMaxSizeChanged     += self.OnNumberOfItemsShownChanged;
					newService.OnPathsChanged       += self.OnPathsChanged;

					self.CheckAndBuildMenus();
				}
            }
		);

	public IRecentPathsManagerService RecentPathsManagerService
	{
		get => (RecentPathsManagerService)GetValue(RecentPathsManagerServiceProperty);
		set => SetValue(RecentPathsManagerServiceProperty, value);
	}

	#endregion

	#region Methods

	private void OnMaxSizeChanged(uint numberOfIems)
	{
		OnPathsChanged(RecentPathsManagerService.GetRecentPaths());
	}

	private void OnNumberOfItemsShownChanged(uint numberOfIems)
	{
		OnPathsChanged(RecentPathsManagerService.GetRecentPaths());
	}

	/// <summary>
	/// This is used to prevent building the menus multiple times.  It checks if all the properties are in place and
	/// then builds the menus once they are available.
	/// 
	/// By using this, we are requiring all the properties to be set.  If one is left off, there will be no functionality.
	/// </summary>
	private void CheckAndBuildMenus()
	{
		if (RecentPathsManagerService != null && PathCommand != null && PathNotFoundCommand != null)
		{
			OnPathsChanged(RecentPathsManagerService.GetRecentPaths());
		}
	}

	private void OnPathsChanged(List<string> paths)
	{
		Clear();
		CreateFlyoutItems(paths);
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
				PathCommand					= PathCommand,
				PathNotFoundCommand			= PathNotFoundCommand,
				RecentPathsManagerService	= RecentPathsManagerService
			};
			Add(menuFlyoutPath);
		}
		MenuService.ForceMenuRebuild();
	}

	#endregion

} // End class.