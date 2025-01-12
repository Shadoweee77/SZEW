namespace SZEW_MAUI.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void LoginButton_Clicked(object sender, EventArgs e) {
        API.setCredentials(Username.Text, Password.Text);
        int userType = API.loginStatus();
        if(userType == 1) {
            await Shell.Current.GoToAsync("/adminhome");
        }
        else if(userType == 2) {
            await Shell.Current.GoToAsync("/mechanichome");
        }
        else {
            await DisplayAlert("Login failed", "Username or password if invalid", "Try again");
        }
    }
}