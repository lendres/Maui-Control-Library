using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalProduction.Maui.Controls;

public class RxButton : Button
{
	public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(RxButton));
	public static readonly BindableProperty VerticalTextAlignmentProperty = BindableProperty.Create(nameof(VerticalTextAlignment), typeof(TextAlignment), typeof(RxButton));


	public TextAlignment HorizontalTextAlignment
	{
		get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty);
		set => SetValue(HorizontalTextAlignmentProperty, value);
	}

	public TextAlignment VerticalTextAlignment
	{
		get => (TextAlignment)GetValue(VerticalTextAlignmentProperty);
		set => SetValue(VerticalTextAlignmentProperty, value);
	}
}
