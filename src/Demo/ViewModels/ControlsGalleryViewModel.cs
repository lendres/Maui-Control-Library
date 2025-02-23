using DigitalProduction.Demo.Models;

namespace DigitalProduction.Demo.ViewModels;

public partial class ControlsGalleryViewModel() : BaseGalleryViewModel(
[
	SectionModel.Create<DynamicMenusPageViewModel>("Dynamic Menus", "Add menu flyouts dynamically."),
	SectionModel.Create<RecentlyUsedMenuPageViewModel>("Recently Used Menu", "A recently used menu flyout.")
]);