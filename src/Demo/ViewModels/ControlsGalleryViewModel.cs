using DPMauiDemo.Models;
using DPMauiDemo.Pages;

namespace DPMauiDemo.ViewModels;

public partial class ControlsGalleryViewModel() : BaseGalleryViewModel(
[
	SectionModel.Create<DynamicMenusPageViewModel>("Dynamic Menus", "Add menu flyouts dynamically.")
]);