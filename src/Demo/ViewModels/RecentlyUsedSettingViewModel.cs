using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalProduction.Demo.ViewModels;

public partial class RecentlyUsedSettingViewModel : ObservableObject
{
//OnNumberOfItemsShownChanged
	[ObservableProperty]
	public partial int		NumberOfItemsShown { get; set; }
}
