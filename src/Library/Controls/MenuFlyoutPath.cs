namespace DigitalProduction.Maui.Controls;

public partial class MenuFlyoutPath : MenuFlyoutItem
{
	private int			_number		= 0;
	private string		_path		= string.Empty;

	public int Number
	{
		get => _number;
        set
		{
			_number = value;
			UpdateDisplayedText();
		}
    }

    public string Path
	{
		get => _path;
		set
		{
			_path				= value;
			CommandParameter	= value;
			UpdateDisplayedText();
		}
	}

	private void UpdateDisplayedText()
	{
		string fileNumber	= (_number+1).ToString();
		Text				= fileNumber + " " + System.IO.Path.GetFileName(_path)  + " (" + _path + ")";
	}
}