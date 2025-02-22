namespace DigitalProduction.Maui.Services;

/// <summary>
/// Registry access and setting storage.
/// </summary>

public interface IRecentPathsManagerService
{
	#region Events

	public event Action<uint>?				OnMaxSizeChanged;
	public event Action<uint>?				OnNumberOfItemsShownChanged;
	public event Action<List<string>>		OnPathsChanged;

	#endregion

	#region Properties

	/// <summary>
	/// Unique name used for identifcation and storage.
	/// </summary>
	public string Name { get; set; }

	/// <summary>
	/// Maximum size stored.
	/// </summary>
	public uint MaxSize { get; set; }

	/// <summary>
	/// Current number of items.
	/// </summary>
	public uint NumberOfItemsShown { get; set; }

	/// <summary>
	/// Remove paths if they no longer exist.
	/// </summary>
	public bool RemoveNotFoundPaths { get; set; }

	#endregion

	#region Methods

	/// <summary>
	/// Gets all the recently used files.
	/// </summary>
	/// <returns>
	/// An array of strings.  Blank strings are returned for any entries that do not exist.
	/// </returns>
	public List<string> GetRecentPaths();

	/// <summary>
	/// Puts the specified path at the top of the entries.  If the supplied path is already located in the
	///	list at some other position, it is removed from that position put on top, moving the other paths down.
	/// </summary>
	/// <param name="path">Path to insert at top of the list.</param>
	void PushTop(string path);

	/// <summary>
	/// Removes the specified path from the list of paths.
	/// </summary>
	/// <param name="path">The path to be removed.</param>
	void RemovePath(string path);

	/// <summary>
	/// Removes the path found the specified position from the list of paths.
	/// </summary>
	/// <param name="position">The position of the path to be removed.</param>
	void RemovePath(int position);

	/// <summary>
	/// Resets all the paths to be blank (empty string).
	/// </summary>
	void ClearAllPaths();

	#endregion
}