namespace SZEW_MAUI.Views;

public partial class LoadingPage : ContentPage
{
	public LoadingPage()
	{
		InitializeComponent();
	}
    protected override async void OnNavigatedTo(NavigatedToEventArgs args) {
        int usertype = API.loginStatus();
        if(usertype == 0) {
            await Shell.Current.GoToAsync("/mechanicshell");
        }
        else if(usertype == 1) {
            await Shell.Current.GoToAsync("/adminshell");
        }
        else {
            await Shell.Current.GoToAsync("/login");
        }
        base.OnNavigatedTo(args);
    }
}