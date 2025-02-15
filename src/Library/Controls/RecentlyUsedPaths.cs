using DigitalProduction.Maui.Services;

namespace DigitalProduction.Maui.Controls;

/// <summary>
/// A class for creating a recently used files list on a menu.
/// </summary>
public partial class RecentlyUsedPaths
{
	#region Delegates

	/// <summary>
	/// Call back delegate for when a recent file control was clicked and the path on the control exists.
	/// </summary>
	/// <param name="path">The path that is displayed on the clicked control.</param>
	public delegate void RecentFileClickedDelegate(string path);

	/// <summary>
	/// Call back delegate for when a recent file control was clicked and the path on the control does not exist.
	/// </summary>
	/// <param name="path">The path that is displayed on the clicked control.</param>
	public delegate void RecentFileNotFoundDelegate(string path);

	// Hold the callback delegate.
	private RecentFileClickedDelegate		_fileClickedDelegate;

	private RecentFileNotFoundDelegate		_fileNotFoundDelegate;

	#endregion

	#region Fields

	// Services.
	private readonly IMenuService           _menuService;
	private readonly IPathStorageService	_registryAccess; //_storageService

	// Maximum allowed size of list.
	const uint								_maxSize										= 10;

	// Current number of items shown.
	uint									_size											= _maxSize;

	// Files.
	private string[]						_paths											= new string[_maxSize];

	// Menu item the list is attached to.
	private readonly IMenuFlyoutSubItem		_recentlyUsedSubMenu;

	private bool							_removeNotFoundFiles							= true;

	#endregion

	#region Construction

	/// <summary>
	/// Basic constructor.
	/// </summary>
	/// <param name="menuItem">Menu item the list is attached to.</param>
	public RecentlyUsedPaths(IMenuFlyoutSubItem menuItem)
	{
		_recentlyUsedSubMenu			= menuItem;
		Initialize();
	}

	/// <summary>
	/// Basic constructor.
	/// </summary>
	/// <param name="menuItem">Menu item the list is attached to.</param>
	/// <param name="fileClickDelegate">The call back function (delegate) to receive the path displayed on the control.</param>
	public RecentlyUsedPaths(IMenuFlyoutSubItem menuItem, RecentFileClickedDelegate fileClickDelegate)
	{
		_recentlyUsedSubMenu	= menuItem;
		_fileClickedDelegate	= fileClickDelegate;
		Initialize();
	}

	/// <summary>
	/// Basic constructor.
	/// </summary>
	/// <param name="menuItem">Menu item the list is attached to.</param>
	/// <param name="fileClickDelegate">The call back function (delegate) to receive the path displayed on the control.</param>
	/// <param name="fileNotFoundDelegate">The call back function (delegate) to receive the path displayed on the control when the corresponding file does not exist.</param>
	public RecentlyUsedPaths(IMenuFlyoutSubItem menuItem, RecentFileClickedDelegate fileClickDelegate, RecentFileNotFoundDelegate fileNotFoundDelegate)
	{
		_recentlyUsedSubMenu	= menuItem;
		_fileClickedDelegate	= fileClickDelegate;
		_fileNotFoundDelegate	= fileNotFoundDelegate;
	}

	#endregion

	#region Properties

	/// <summary>
	/// The call back function for when a recent file menu items is clicked.
	/// </summary>
	public RecentFileClickedDelegate FileClickedDelegate
	{
		get
		{
			return _fileClickedDelegate;
		}
		set
		{
			_fileClickedDelegate = value;
		}
	}

	/// <summary>
	/// The call back function for when a recent file menu item is clicked, but the file does not exist at the location specified on the control.
	/// </summary>
	public RecentFileNotFoundDelegate FileNotFoundDelegate
	{
		get
		{
			return _fileNotFoundDelegate;
		}
		set
		{
			_fileNotFoundDelegate = value;
		}
	}

	///// <summary>
	///// Access to the registry for storing data between program instances.
	///// </summary>
	//public FormWinRegistryAccess RegistryAccess
	//{
	//	get
	//	{
	//		return _registryAccess;
	//	}

	//	set
	//	{
	//		_registryAccess	= value;
	//		_paths			= _registryAccess.GetRecentlyUsedFiles(_maxSize);
	//		SetFileNames();
	//	}
	//}

	/// <summary>
	/// Get the number of controls that are shown.  Attempts to retrieve the value from the registry, if it
	/// fails, the maximum number of allowed files is returned.
	/// </summary>
	public uint MaxNumberOfItemsShown
	{
		get
		{
			// If we have a place to store the value we will attempt to retrieve it from there.
			if (_registryAccess != null)
			{
				uint storedsize = _registryAccess.Size;

				// Compare the two.
				if (storedsize <= _size)
				{
					_size = storedsize;

				}
			}

			System.Diagnostics.Debug.Assert(_size != 0, "The size of the Recently Used Files menu cannot be zero.");
			return _size;
		}

		set
		{
			uint validsize = value;

			if (value > _maxSize)
			{
				validsize = _maxSize;
			}

			// If we have a place to store the value we will attempt to retrieve it from there.
			if (_registryAccess != null)
			{
				_registryAccess.Size = validsize;
			}

			_size = validsize;

			SetFileNames();
		}
	}

	/// <summary>
	/// Gets or sets a value that states whether controls should be removed if they are clicked and the file does not exist.
	/// </summary>
	public bool RemoveNotFoundFiles
	{
		get
		{
			return _removeNotFoundFiles;
		}

		set
		{
			_removeNotFoundFiles = value;
		}
	}

	#endregion

	#region Public Functions

	/// <summary>
	/// Add a new file (path) to the top of the recently used files list.
	/// </summary>
	/// <param name="path">File (path) to add.</param>
	public void AddNewFilePath(string path)
	{
		// If the path provided is the same as the first entry on the list, we don't need to do anything.
		if (path == _paths[0])
		{
			return;
		}

		PushTop(path);
	}

	/// <summary>
	/// Open the file on the numbered menu.  Parameter is zero based.
	/// </summary>
	/// <param name="number">Zero based index of menu to open.</param>
	//public void OpenFile(int number)
	//{
	//	if (number < _maxSize)
	//	{
	//		OpenFile(mnuRecentFiles[number]);
	//	}
	//}

	#endregion

	#region Private Controls Manipulation Functions

	/// <summary>
	/// Setup the control.
	/// </summary>
	private void Initialize()
	{
		// Initialize the paths.  Either from the registry, or create blank ones.
		if (_registryAccess != null)
		{
			// Get values from the registry.
			_paths = _registryAccess.GetRecentlyUsedPaths();
		}
		else
		{
			// Initialize new string.
			for (int i = 0; i < _maxSize; i++)
			{
				_paths[i] = "";
			}
		}

		uint numberofitemsshown = MaxNumberOfItemsShown;

		// Generate all the menu item instances.
		for (int i = 0; i < _maxSize; i++)
		{
			string filenumber = (i+1).ToString();

			//this.mnuRecentFiles[i]          = new ToolStripMenuItem();
			//this.mnuRecentFiles[i].Name     = "mnuRecent" + filenumber;
			//this.mnuRecentFiles[i].Size     = new System.Drawing.Size(152, 22);
			//this.mnuRecentFiles[i].Click    += new System.EventHandler(this.RecentFileMenu_Clicked);

			//this._recentlyUsedSubMenu.DropDownItems.Add(this.mnuRecentFiles[i]);
		}

		// Establish the initial controls.
		SetFileNames();

		// Latch onto the openning event of the parent.  Then we can check to see if the number of items shown
		// has changed before the dropdown opens.  If it has we can update it to show the correct number.
		//this._recentlyUsedSubMenu.DropDownOpening += ParentMenu_DropDownOpening;
	}

	/// <summary>
	/// Open the file on the specified menu.
	/// </summary>
	/// <param name="menu">Menu to get the file from.</param>
	//private void OpenFile(ToolStripMenuItem menu)
	//{
	//	string path = menu.ToolTipText;

	//	// Ignore blank controls.
	//	if (path == "" || path == null)
	//	{
	//		return;
	//	}

	//	if (System.IO.File.Exists(path))
	//	{
	//		PushTop(path);
	//		_fileClickedDelegate(path);
	//	}
	//	else
	//	{
	//		if (_removeNotFoundFiles)
	//		{
	//			RemovePath(menu);
	//		}
	//		_fileNotFoundDelegate(path);
	//	}
	//}

	/// <summary>
	/// Creates a list of strings that the new list of paths, with the new path inserted at the top.  If the supplied
	/// path is located in the list at some other position, it is removed from that position and the other paths moved
	/// up to fill that slot.
	/// </summary>
	/// <param name="path">Path to insert at top of the list.</param>
	private void PushTop(string path)
	{
		// Copy the existing names from the menu items while at the same time moving all
		// the names down one slot so we can put the new name at the front.  If the name was already
		// in the list, then we only move the paths up to the point where the path was previously
		// located (in other words, move that entry to the top and push the others down).
		uint endindex = FindIndexOf(path);

		for (uint i = endindex; i > 0; i--)
		{
			_paths[i]	= _paths[i-1];
		}

		// New name at the front.
		_paths[0] = path;

		// Now update the registry and control.
		SetFileNames();
	}

	/// <summary>
	/// Finds the zeroth based index of the path in the list of existing paths.  If the path is not found, the last index is returned.
	/// </summary>
	/// <param name="path">Path to search for.</param>
	private uint FindIndexOf(string path)
	{
		uint position = _maxSize-1;

		for (uint i = 0; i < _maxSize; i++)
		{
			if (path == _paths[i])
			{
				position = i;
				break;
			}
		}

		return position;
	}

	/// <summary>
	/// Finds the zeroth based index of the ToolStripMenuItem in the list of menu items.  If the control is not found, the last index is returned.
	/// </summary>
	/// <param name="menuitem">ToolStripMenuItem to search for.</param>
	private uint FindIndexOf(IMenuFlyoutSubItem menuitem)
	{
		uint position = _maxSize;

		for (uint i = 0; i < _maxSize; i++)
		{
			//if (this.mnuRecentFiles[i] == menuitem)
			//{
			//	position = i;
			//}
		}
		return position;
	}

	/// <summary>
	/// Removes the path found on the ToolStripMenuItem from the list of paths.
	/// </summary>
	/// <param name="menuitem">ToolStripMenuItem which contains the path to be removed.</param>
	private void RemovePath(IMenuFlyoutSubItem menuitem)
	{
		uint position = FindIndexOf(menuitem);

		// Move the paths up one position.
		for (uint i = position; i < _maxSize-1; i++)
		{
			_paths[i] = _paths[i+1];
		}

		// Now update the registry and control.
		SetFileNames();
	}

	/// <summary>
	/// Sets the Text and ToolTipText of a ToolStripMenuitem.
	/// </summary>
	/// <param name="menuitem">ToolStripMenuItem to edit.</param>
	/// <param name="displayednumber">Number to display in front of path.  It's the position on the parent form.</param>
	/// <param name="path">Path to be displayed.</param>
	private void AddPathToMenuItem(IMenuFlyoutSubItem menuitem, int displayednumber, string path)
	{
		//menuitem.Text           = displayednumber.ToString() + " " + System.IO.Path.GetFileName(path);
		//menuitem.ToolTipText    = path;
	}

	/// <summary>
	/// Establishes a group of ToolStripMenuItems on the parent control.  The menu items are populate
	/// with the file names supplied as input.  The number of menu items added to the parent control
	/// is the same as the length of the input array.
	/// </summary>
	private void SetFileNames()
	{
		// Update the registry, if it exists.
		_registryAccess?.SetRecentlyUsedPaths(_paths);

		int pathsfound = 0;

		// Update the names of the menu items in our array of menu items, then add a reference
		// of that menu item to the new array.
		for (int i = 0; i < _maxSize; i++)
		{
			// Assume the control is not visiable.  It must pass all the tests before making it visable.
			//this.mnuRecentFiles[i].Visible = false;

			if (_paths[i] != "")
			{
				pathsfound++;

				if (pathsfound <= MaxNumberOfItemsShown)
				{
					//AddPathToMenuItem(mnuRecentFiles[i], pathsfound, _paths[i]);

					// The path is not blank and we have not yet filled all the allowed visable slots on the control,
					// so now we make it visible.
					//this.mnuRecentFiles[i].Visible = true;
				}
			}
		}

		// If there was no name specified, then there are no "Recent Files."  The sub menus (recent files) have
		// already been cleared, so now we disable the control and return.
		if (pathsfound == 0)
		{
			//this._recentlyUsedSubMenu.Enabled = false;
			return;
		}
		else
		{
			// We have sub menu items, so make sure the control is enabled.  If there were no previous menu items
			// it may have been disabled.
			//this._recentlyUsedSubMenu.Enabled = true;
		}
	}

	#endregion

	#region Event Handlers

	/// <summary>
	/// Event handler for when a recent file is clicked.  The path associated with the clicked ToolStripMenuItem
	/// is gotten from the ToolTip.  If the file exists the RecentFileClickedDelegate is called.  Otherwise the
	/// RecentFileNotFoundDelegate is called.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="eventArgs">Event arguments.</param>
	private void RecentFileMenu_Clicked(object sender, EventArgs eventArgs)
	{
		//ToolStripMenuItem clickedMenu = (ToolStripMenuItem)sender;
		//OpenFile(clickedMenu);
	}

	/// <summary>
	/// Event handler for the parent's drop down opening.  Used to update the number of items shown if it has changed.
	/// </summary>
	/// <param name="sender">Sender.</param>
	/// <param name="eventArgs">Event arguments.</param>
	void ParentMenu_DropDownOpening(object sender, EventArgs eventArgs)
	{
		if (_registryAccess != null)
		{
			uint newNumberOfItems = _registryAccess.Size;
			if (newNumberOfItems != _size)
			{
				MaxNumberOfItemsShown = newNumberOfItems;
			}
		}
	}

	#endregion

} // End class.