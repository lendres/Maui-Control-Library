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
		BindableProperty.Create(nameof(Increment), typeof(double), typeof(Stepper), null);

	public double Increment { get; set; }

	public static readonly BindableProperty MaximumProperty =
		BindableProperty.Create(nameof(Maximum), typeof(double), typeof(Stepper), null);

	public double Maximum { get; set; }

	public static readonly BindableProperty MinimumProperty =
		BindableProperty.Create(nameof(Minimum), typeof(double), typeof(Stepper), null);

	public double Minimum { get; set; }

	public static readonly BindableProperty ValueProperty =
		BindableProperty.Create(nameof(Value), typeof(int), typeof(Stepper), null);

	public int Value { get; set; }

	#endregion

	#region Events
 
    private void MinusButton_Clicked(object sender, EventArgs eventArgs)
    {
        Value--;
        ValueLabel.Text = Value.ToString();
    }
 
    private void PlusButton_Clicked(object sender, EventArgs eventArgs)
    {
        Value++;
        ValueLabel.Text = Value.ToString();
    }
 
    private void ValueEntry_TextChanged(object sender, TextChangedEventArgs eventArgs)
    {
        if (int.TryParse(eventArgs.NewTextValue, out var value))
        {
            Value = value;
        }
    }

	#endregion
}