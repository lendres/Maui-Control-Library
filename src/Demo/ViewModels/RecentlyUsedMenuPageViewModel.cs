using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;

namespace DigitalProduction.Demo.ViewModels;

public partial class RecentlyUsedMenuPageViewModel : BaseViewModel, IDisposable
{
	#region Fields

	private readonly IDialogService     _dialogService;
	private string						_fileDirectory;
	private int                         _fileCounter        = 0;

	// Track if Dispose has been called. 
	private bool						_disposed			= false;

	#endregion

	#region Construction

	public RecentlyUsedMenuPageViewModel(IRecentPathsManagerService recentPathsManagerService, IDialogService dialogService)
	{
		RecentPathsManagerService	= recentPathsManagerService;
		_dialogService				= dialogService;

		_fileDirectory				= DigitalProduction.Reflection.Assembly.Path()!;

		// During the construction of the RecentPathsManagerService, it will restore any saved values.  If
		// No values were saved, then we will add some here for demonstration purposed (so we don't get a blank
		// control).  Normally, you would not do this.
		if (RecentPathsManagerService.GetRecentPaths().Count == 0)
		{
			ResetPaths();
		}
	}

	/// <summary>
	/// Implement IDisposable.
	/// Do not make this method virtual.
	/// A derived class should not be able to override this method.
	/// </summary>
	public void Dispose()
	{
		Dispose(true);

		// This object will be cleaned up by the Dispose method.  Therefore, you should call GC.SupressFinalize to
		// take this object off the finalization queue and prevent finalization code for this object from executing
		// a second time.
		GC.SuppressFinalize(this);
	}

	/// <summary>
	/// Dispose(bool disposing) executes in two distinct scenarios.
	/// If disposing equals true, the method has been called directly or indirectly by a user's code. Managed and
	/// unmanaged resources can be disposed.  If disposing equals false, the method has been called by the runtime
	/// from inside the finalizer and you should not reference other objects. Only unmanaged resources can be disposed.
	/// </summary>
	/// <param name="disposing">Disposing.</param>
	protected virtual void Dispose(bool disposing)
	{
		// Check to see if Dispose has already been called.
		if (!_disposed)
		{
			// If disposing equals true, also dispose of managed resources.
			if (disposing)
			{
				// Dispose managed resources.
			}
			RemoveTempFiles();
		}
	}

	/// <summary>
	/// Use C# destructor syntax for finalization code.  This destructor will run only if the Dispose method
	/// does not get called.  It gives your base class the opportunity to finalize.  Do not provide
	/// destructors in types derived from this class.
	/// </summary>
	~RecentlyUsedMenuPageViewModel()
	{
		// Do not re-create Dispose clean-up code here.  Calling Dispose(false) is optimal in terms of
		// readability and maintainability.
		Dispose(false);
	}

	#endregion

	#region Properties

	[ObservableProperty]
	public partial IRecentPathsManagerService		RecentPathsManagerService { get; set; }

	public Page? MenuHostingPage
	{
		get => _dialogService.HostingPage;
		set => _dialogService.HostingPage = value;
	}

	#endregion

	#region Methods

	private string CreateTempFile()
	{
		string tempFile = System.IO.Path.Combine(_fileDirectory, "File That Exists" + (++_fileCounter).ToString() + ".txt");
		System.IO.File.WriteAllText(tempFile, "This file is for testing only."+Environment.NewLine);
		return tempFile;
	}

	private void RemoveTempFiles()
	{
		for (int i = 0; i < _fileCounter; i++)
		{
			System.IO.File.Delete( "File That Exists" + (i+1).ToString() + ".txt");
		}
	}

	[RelayCommand]
	void ShowSelectedMessage(string message)
	{
		_dialogService.ShowMessage("Menu clicked", $"The path \"{message}\" was selected.", "OK");
	}

	[RelayCommand]
	void ShowRemovedMessage(string message)
	{
		_dialogService.ShowMessage("Menu clicked", $"The path \"{message}\" was was not found.", "OK");
	}

	[RelayCommand]
	void AddNewPath()
	{
		RecentPathsManagerService.PushTop(CreateTempFile());
	}

	[RelayCommand]
	void ResetPaths()
	{
		RemoveTempFiles();
		RecentPathsManagerService.ClearAllPaths();
		RecentPathsManagerService.PushTop(CreateTempFile());
		RecentPathsManagerService.PushTop(@"C:\Temp\Does Not Exist File 2.txt");
		RecentPathsManagerService.PushTop(@"C:\Temp\Does Not Exist File 1.txt");
	}

	#endregion
}