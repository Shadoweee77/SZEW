namespace SZEW_MAUI.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
	}
    protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
        var usertype = await SecureStorage.GetAsync("usertype");
        if(usertype == "mechanic") {
            await Shell.Current.GoToAsync("/mechanichome");
        }
        else if(usertype == "admin") {
            await Shell.Current.GoToAsync("/adminhome");
        }
        else {
            await Shell.Current.GoToAsync("/login");
        }
        base.OnNavigatedTo(args);
    }
}