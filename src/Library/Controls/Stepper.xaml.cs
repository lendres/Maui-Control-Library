using Microsoft.Maui.Controls.PlatformConfiguration.TizenSpecific;
using System.Globalization;
using System.Windows.Input;

namespace DigitalProduction.Maui.Controls;

public partial class Stepper : ContentView
{
	#region Fields
	#endregion

	#region Construction
 
    public Stepper()
    {
       InitializeComponent();
        ValueLabel.Text = "0";
    }

	#endregion

	#region Bindable Properties

	public static readonly BindableProperty IncrementProperty =
		BindableProperty.Create(nameof(Increment), typeof(double), typeof(Stepper), null,
		propertyChanged: (bindable, oldObject, newObject) =>
        {
            if (newObject == oldObject || bindable is not Stepper self)
            {
                return;
            }

			if (newObject != null)
			{
				self.RoundValue();
				if (self.BoundValue())
				{
					self.UpdateText();
				}
				self.UpdateButtonEnabled();
			}
        }
	);

	private void RoundValue()
	{
		Value = Math.Round(Value / Increment) * Increment;
		UpdateText();
	}

	public double Increment
	{
		get => (double)GetValue(IncrementProperty);
		set => SetValue(IncrementProperty, value);
	}

	public static readonly BindableProperty MaximumProperty =
		BindableProperty.Create(nameof(Maximum), typeof(double), typeof(Stepper), null,
		propertyChanged: (bindable, oldObject, newObject) =>
        {
            if (newObject == oldObject || bindable is not Stepper self)
            {
                return;
            }

			if (newObject != null)
			{
				if (self.BoundValue())
				{
					self.UpdateText();
				}
				self.UpdateButtonEnabled();
			}
        }
	);

	public double Maximum
	{
		get => (double)GetValue(MaximumProperty);
		set => SetValue(MaximumProperty, value);
	}

	public static readonly BindableProperty MinimumProperty =
		BindableProperty.Create(nameof(Minimum), typeof(double), typeof(Stepper), null,
		propertyChanged: (bindable, oldObject, newObject) =>
        {
            if (newObject == oldObject || bindable is not Stepper self)
            {
                return;
            }

			if (newObject != null)
			{
				if (self.BoundValue())
				{
					self.UpdateText();
				}
				self.UpdateButtonEnabled();
			}
		}
	);

	public double Minimum
	{
		get => (double)GetValue(MinimumProperty);
		set => SetValue(MinimumProperty, value);
	}

	public static readonly BindableProperty ValueProperty =
		BindableProperty.Create(nameof(Value), typeof(double), typeof(Stepper), null);

	public double Value { get; set; }

	public static readonly BindableProperty ButtonStyleProperty =
		BindableProperty.Create(nameof(ButtonStyle), typeof(Style), typeof(Stepper), null);

	public Style ButtonStyle { get; set; }

	#endregion

	#region Events
 
    private void MinusButton_Clicked(object sender, EventArgs eventArgs)
    {
        Value -= Increment;
		BoundValue();
		UpdateText();
		UpdateButtonEnabled();
    }
 
    private void PlusButton_Clicked(object sender, EventArgs eventArgs)
    {
        Value += Increment;
		BoundValue();
		UpdateText();
		UpdateButtonEnabled();
    }

	private bool BoundValue()
	{
		if (Value < Minimum)
		{
			Value = Minimum;
			return true;
		}
		else
		{
			if (Value > Maximum)
			{
				Value = Maximum;
				return true;
			}
		}

		return false;
	}

	private void UpdateText()
	{
		int decimalPlaces	= BitConverter.GetBytes(decimal.GetBits((decimal)Increment)[3])[2];
		ValueLabel.Text		= Value.ToString("F"+decimalPlaces, CultureInfo.CurrentCulture);
	}

	private void UpdateButtonEnabled()
	{
		if (Value <= Minimum + Increment )
		{
			MinusButton.IsEnabled = false;
		}
		else
		{
			if (Value > Minimum)
			{
				MinusButton.IsEnabled = true;
			}
		}

		if (Value >= Maximum - Increment)
		{
			PlusButton.IsEnabled = false;
		}
		else
		{
			if (Value < Maximum)
			{
				PlusButton.IsEnabled = true;
			}
		}
	}

	#endregion
}