﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DigitalProduction.Maui.Services;
using System.Collections.ObjectModel;

namespace DigitalProduction.Demo.ViewModels;

public partial class StepperPageViewModel() : BaseViewModel
{
	#region Fields
	#endregion

	#region Properties

	[ObservableProperty]
	public partial double				Increment { get; set; }				= 1;

	[ObservableProperty]
	public partial double				Maximum { get; set; }				= 20;

	[ObservableProperty]
	public partial double				Minimum { get; set; }				= 0;

	[ObservableProperty]
	public partial double				Value { get; set; }					= 0;

	#endregion

	#region Methods

	partial void OnIncrementChanged(double value)
	{
		System.Diagnostics.Debug.WriteLine("Increment value: "+value.ToString());
	}

	partial void OnMaximumChanged(double value)
	{
		System.Diagnostics.Debug.WriteLine("Maximum value: "+value.ToString());
	}

	partial void OnMinimumChanged(double value)
	{
		System.Diagnostics.Debug.WriteLine("Minimum value: "+value.ToString());
	}


	#endregion
}