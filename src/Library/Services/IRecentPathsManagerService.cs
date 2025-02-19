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

	/// <summary>
	/// Gets all the recently used files.
	/// </summary>
	/// <returns>
	/// An array of strings.  Blank strings are returned for any entries that do not exist.
	/// </returns>
	public List<string> GetRecentPaths();


	void PushTop(string path);
	void RemovePath(string path);
	void RemovePath(int position);
	void ClearAllPaths();

	#endregion
}