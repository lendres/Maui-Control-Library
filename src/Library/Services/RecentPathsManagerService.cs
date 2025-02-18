namespace DigitalProduction.Maui.Services;

/// <summary>
/// Registry access and setting storage.
/// </summary>

public class RecentPathsManagerService : IRecentPathsManagerService
{
	#region Events

	public event Action<uint>?				OnMaxSizeChanged;
	public event Action<uint>?				OnNumberOfItemsShownChanged;
	public event Action<bool>?				OnRemoveNotFoundPathsChanged;
	//public event Action?			OnChanged;
	public event Action<List<string>>		OnPathsChanged;

	#endregion

	#region Fields

	// Files.
	private List<string>					_paths			= [];

	#endregion

	#region Construction

	public RecentPathsManagerService()
	{
		RestoreRecentPaths();
		OnPathsChanged	+= SaveRecentPaths;
	}

	#endregion

	#region Properties

	/// <summary>
	/// Unique name used for identifcation and storage.
	/// </summary>
	public uint Name { get; set; }

	/// <summary>
	/// Maximum size stored.
	/// </summary>
	public uint MaxSize
	{
		get => Preferences.Default.Get(StorageName("Max Size"), 10u);
		set
		{
			Preferences.Default.Set(StorageName("Max Size"), value);
			OnMaxSizeChanged?.Invoke(value);
		}
	}

	/// <summary>
	/// Current number of items.
	/// </summary>
	public uint NumberOfItemsShown
	{
		get => Preferences.Default.Get(StorageName("Size"), 10u);
		set
		{
			uint size = Math.Min(value, MaxSize);
			Preferences.Default.Set(StorageName("Size"), size);
			OnNumberOfItemsShownChanged?.Invoke(size);
		}
	}

	/// <summary>
	/// Remove paths if they no longer exist.
	/// </summary>
	public bool RemoveNotFoundPaths
	{
		get => Preferences.Default.Get(StorageName("Remove Not Found Paths"), true);
		set
		{
			Preferences.Default.Set(StorageName("Remove Not Found Paths"), value);
			OnRemoveNotFoundPathsChanged?.Invoke(value);
		}
	}

	#endregion

	#region Private Methods

	private string StorageName(string name) => Name + " " + name;

	#endregion

	#region Private Methods

	/// <summary>
	/// Gets the recently used path specified.
	/// </summary>
	/// <returns>
	/// The path at the provided position.  Blank strings are returned for any entries that do not exist.
	/// </returns>
	private string GetStoredPath(uint pathNumber)
	{
		System.Diagnostics.Debug.Assert(pathNumber < MaxSize);
		return Preferences.Default.Get(StorageName("Path "+pathNumber.ToString()), "");
	}

	/// <summary>
	/// Sets (saves) the recently used path specified.
	/// </summary>
	private void SaveRecentPath(uint pathNumber, string path)
	{
		System.Diagnostics.Debug.Assert(pathNumber < MaxSize);
		Preferences.Default.Set(StorageName("Path "+pathNumber.ToString()), path);
	}

	/// <summary>
	/// Gets all the recently used files.
	/// </summary>
	/// <returns>
	/// An array of strings.  Blank strings are returned for any entries that do not exist.
	/// </returns>
	public void RestoreRecentPaths()
	{
		_paths.Clear();

		for (uint i = 0; i < MaxSize; i++)
		{
			string path = GetStoredPath(i);
			if (path != string.Empty)
			{
				_paths.Add(path);
			}
		}
	}

	/// <summary>
	/// Sets (saves) all the recently used files.
	/// </summary>
	private void SaveRecentPaths(List<string> paths)
	{
		uint i = 0;
		foreach (string path in paths)
		{
			SaveRecentPath(i, path);
		}
	}

	#endregion

	#region Path Management

	/// <summary>
	/// Gets all the recently used files.
	/// </summary>
	/// <returns>
	/// An array of strings.  Blank strings are returned for any entries that do not exist.
	/// </returns>
	public List<string> GetRecentPaths()
	{
		List<string> paths = [];

		int numberOfFlyouts = Math.Min(_paths.Count, (int)NumberOfItemsShown);
		for (int i = 0; i < numberOfFlyouts; i++)
		{
			paths.Add(_paths[i]);
		}

		return paths;
	}

	/// <summary>
	/// Puts the specified path at the top of the entries.  If the supplied path is already located in the
	///	list at some other position, it is removed from that position put on top, moving the other paths down.
	/// </summary>
	/// <param name="path">Path to insert at top of the list.</param>
	public void PushTop(string path)
	{
		// We won't add empty strings.
		// If the path provided is the same as the first entry on the list, we don't need to do anything.
		if (string.IsNullOrEmpty(path) || (_paths.Count > 0 &&  path == _paths.First()))
		{
			return;
		}

		// We need to account for 2 situations.
		// 1 - The path already exists.  In this case we need to remove it from its current position and move it to the
		//     top.  In this case, the number of paths remains the same and we don't need to worry about the size.
		// 2 - The path does not already exist.  In this case, the size of the list will increase when we add the new path
		//     so we need to check to see if we have exceeded the maximum number of paths we are saving.

		// Remove the path if it exists and then put it at the front.
		bool found = _paths.Remove(path);
		_paths.Insert(0, path);

		if (!found && _paths.Count > MaxSize)
		{
			// Our allowable size has been exceeded, so remove the last entry.
			_paths.RemoveAt(_paths.Count-1);
		}

		// Now update the registry and controls.
		NotifyPathsChanged();
	}

	/// <summary>
	/// Removes the specified path from the list of paths.
	/// </summary>
	/// <param name="path">The path to be removed.</param>
	public void RemovePath(string path)
	{
		_paths.Remove(path);
		NotifyPathsChanged();
	}

	/// <summary>
	/// Removes the path found the specified position from the list of paths.
	/// </summary>
	/// <param name="position">The position of the path to be removed.</param>
	public void RemovePath(int position)
	{
		_paths.RemoveAt(position);
		NotifyPathsChanged();
	}

	private void NotifyPathsChanged()
	{
		OnPathsChanged.Invoke(GetRecentPaths());
	}

	#endregion
}