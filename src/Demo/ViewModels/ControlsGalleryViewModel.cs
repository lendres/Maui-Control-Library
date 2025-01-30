using DigitalProduction.Demo.Models;
using DigitalProduction.Demo.Pages;

namespace DigitalProduction.Demo.ViewModels;

public partial class ControlsGalleryViewModel() : BaseGalleryViewModel(
[
	SectionModel.Create<DynamicMenusPageViewModel>("Dynamic Menus", "Add menu flyouts dynamically.")
]);