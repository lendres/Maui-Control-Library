using DigitalProduction.Maui.Controls;
using Microsoft.Maui.Handlers;
using UIKit;

namespace DigitalProduction.Maui.Platforms.iOS;

internal class RxButtonHandler : ButtonHandler
{
	public override void UpdateValue(string property)
	{
		base.UpdateValue(property);
		if (property == RxButton.HorizontalTextAlignmentProperty.PropertyName)
			OnTextAlignmentPropertyChanged();
	}

	private void OnTextAlignmentPropertyChanged()
	{
		if (VirtualView is RxButton virtualButton)
		{
			//PlatformView.HorizontalAlignment = virtualButton.HorizontalTextAlignment switch
			//{
			//	TextAlignment.Start => UIKit.UIControlContentHorizontalAlignment.Left,
			//	TextAlignment.Center => UIKit.UIControlContentHorizontalAlignment.Center,
			//	TextAlignment.End => UIKit.UIControlContentHorizontalAlignment.Right,
			//	_ => UIKit.UIControlContentHorizontalAlignment.Center,
			//};
			PlatformView.TitleLabel.TextAlignment = virtualButton.HorizontalTextAlignment switch
			{
				TextAlignment.Start => UITextAlignment.Left,
				TextAlignment.Center => UITextAlignment.Center,
				TextAlignment.End => UITextAlignment.Right,
				_ => UITextAlignment.Center,
			};
			PlatformView.VerticalAlignment = virtualButton.HorizontalTextAlignment switch
			{
				TextAlignment.Start => UIKit.UIControlContentVerticalAlignment.Top,
				TextAlignment.Center => UIKit.UIControlContentVerticalAlignment.Center,
				TextAlignment.End => UIKit.UIControlContentVerticalAlignment.Bottom,
				_ => UIKit.UIControlContentVerticalAlignment.Center,
			};
		}
	}
}