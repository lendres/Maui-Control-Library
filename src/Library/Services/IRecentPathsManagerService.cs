namespace DigitalProduction.Maui.Services;

/// <summary>
/// Registry access and setting storage.
/// </summary>

public interface IRecentPathsManagerService
{
	#region Events

	public event Action<uint>?				OnMaxSizeChanged;
	public event Action<uint>?				OnNumberOfItemsShownChanged;
	public event Action<bool>?				OnRemoveNotFoundPathsChanged;
	public event Action<List<string>>		OnPathsChanged;

	#endregion

	#region Properties

	/// <summary>
	/// Unique name used for identifcation and storage.
	/// </summary>
	public uint Name { get; set; }

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

	///// <summary>
	///// Gets the recently used path specified.
	///// </summary>
	///// <returns>
	///// The path at the provided position.  Blank strings are returned for any entries that do not exist.
	///// </returns>
	//public string GetRecentPath(uint pathNumber);

	///// <summary>
	///// Sets (saves) the recently used path specified.
	///// </summary>
	//public void SetRecentPath(uint pathNumber, string path);

	/// <summary>
	/// Gets all the recently used files.
	/// </summary>
	/// <returns>
	/// An array of strings.  Blank strings are returned for any entries that do not exist.
	/// </returns>
	public List<string> GetRecentPaths();

	///// <summary>
	///// Sets (saves) all the recently used files.
	///// </summary>
	//public void SetRecentPaths(IEnumerable<string> paths);

	void PushTop(string path);
	void RemovePath(string path);
	void RemovePath(int position);

	#endregion
}