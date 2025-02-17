namespace DigitalProduction.Maui.Services;

/// <summary>
/// Registry access and setting storage.
/// </summary>

public class PathStorageService : IPathStorageService
{
	#region Fields
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
		set => Preferences.Default.Set(StorageName("Max Size"), value);
	}

	/// <summary>
	/// Current number of items.
	/// </summary>
	public uint Size
	{
		get => Preferences.Default.Get(StorageName("Size"), 10u);
		set => Preferences.Default.Set(StorageName("Size"), value);
	}

	/// <summary>
	/// Remove paths if they no longer exist.
	/// </summary>
	public bool RemoveNotFoundPaths
	{
		get => Preferences.Default.Get(StorageName("Remove Not Found Paths"), true);
		set => Preferences.Default.Set(StorageName("Remove Not Found Paths"), value);
	}

	#endregion

	#region Private Methods

	private string StorageName(string name) => Name + " " + name;

	#endregion

	#region Public Methods

	/// <summary>
	/// Gets the recently used path specified.
	/// </summary>
	/// <returns>
	/// The path at the provided position.  Blank strings are returned for any entries that do not exist.
	/// </returns>
	public string GetRecentPath(uint pathNumber)
	{
		System.Diagnostics.Debug.Assert(pathNumber < MaxSize);
		return Preferences.Default.Get(StorageName("Path "+pathNumber.ToString()), "");
	}

	/// <summary>
	/// Sets (saves) the recently used path specified.
	/// </summary>
	public void SetRecentPath(uint pathNumber, string path)
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
	public string[] GetRecentPaths()
	{
		string[] files = new string[MaxSize];

		for (uint i = 0; i < MaxSize; i++)
		{
			files[i] = GetRecentPath(i);
		}

		return files;
	}

	/// <summary>
	/// Sets (saves) all the recently used files.
	/// </summary>
	public void SetRecentPaths(IEnumerable<string> paths)
	{
		uint i = 0;
		foreach (string path in paths)
		{
			SetRecentPath(i, path);
		}
	}

	#endregion
}