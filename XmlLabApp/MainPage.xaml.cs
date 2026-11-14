using XmlLabApp.ViewModels;

namespace XmlLabApp;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
		BindingContext = new MainViewModel();
	}
}
