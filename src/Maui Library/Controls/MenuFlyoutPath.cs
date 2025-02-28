using DigitalProduction.Maui.Services;
using System.Windows.Input;

namespace DigitalProduction.Maui.Controls;

public partial class MenuFlyoutPath : MenuFlyoutItem
{
	#region Fields

	private int			_number		= 0;
	private string		_path		= string.Empty;

	#endregion

	#region Construction

	public MenuFlyoutPath()
	{
		Command = new Command(ClickedCommand);
	}

	#endregion

	#region Properties

	public ICommand?					PathCommand { get; set; }
	public ICommand?					PathNotFoundCommand  { get; set; }
	public IRecentPathsManagerService?	RecentPathsManagerService { private get; set; }

	public int Number
	{
        set
		{
			_number = value;
			UpdateDisplayedText();
		}
    }

    public string Path
	{
		set
		{
			_path = value;
			UpdateDisplayedText();
		}
	}

	#endregion

	#region Methods

	private void UpdateDisplayedText()
	{
		string fileNumber	= (_number+1).ToString();
		Text				= fileNumber + " " + System.IO.Path.GetFileName(_path)  + " (" + _path + ")";
	}

	private void ClickedCommand()
	{
		if (System.IO.Path.Exists(_path))
		{
			PathCommand?.Execute(_path);
			RecentPathsManagerService?.PushTop(_path);
		}
		else
		{
			PathNotFoundCommand?.Execute(_path);
			if (RecentPathsManagerService?.RemoveNotFoundPaths ?? false)
			{
				RecentPathsManagerService.RemovePath(_path);
			}
		}
	}

	#endregion
}