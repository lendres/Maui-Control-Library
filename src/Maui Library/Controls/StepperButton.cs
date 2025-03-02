using CommunityToolkit.Maui.Behaviors;

namespace DigitalProduction.Maui.Controls;

public partial class StepperButton : ImageButton
{
	public StepperButton()
	{
		Behaviors.Add(
			new IconTintColorBehavior()
			{
				TintColor = new Color(255, 255, 255)
			}
		);
	}

	public static readonly BindableProperty IconColorProperty =
		BindableProperty.Create(nameof(IconColor), typeof(Color), typeof(Stepper), null,
		propertyChanged: (bindable, oldObject, newObject) =>
        {
            if (newObject == oldObject || newObject is not Color newColor || bindable is not StepperButton self)
            {
                return;
            }
			if (self.Behaviors.Count > 0)
			{
				IconTintColorBehavior iconTintColorBehavior = (IconTintColorBehavior)self.Behaviors.First();
				iconTintColorBehavior.TintColor = newColor;
			}
		}
	);

	public Color IconColor
	{
		get => (Color)GetValue(IconColorProperty);
		set => SetValue(IconColorProperty, value);
	}
}