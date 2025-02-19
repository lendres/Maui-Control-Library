using DigitalProduction.Maui.Services;
namespace DigitalProduction.Maui.Controls;

public partial class MenuFlyoutPath : MenuFlyoutItem
{
	private int			_number		= 0;
	private string		_path		= string.Empty;

	public MenuFlyoutPath()
	{
		Clicked += OnClickedUpdate;
	}

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
			_path				= value;
			CommandParameter	= value;
			UpdateDisplayedText();
		}
	}

	public IRecentPathsManagerService? RecentPathsManagerService { private get; set; }

	private void UpdateDisplayedText()
	{
		string fileNumber	= (_number+1).ToString();
		Text				= fileNumber + " " + System.IO.Path.GetFileName(_path)  + " (" + _path + ")";
	}

	private void OnClickedUpdate(object? sender, EventArgs eventArgs)
	{
		RecentPathsManagerService?.PushTop(_path);
	}
}