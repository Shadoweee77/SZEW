namespace SZEW_MAUI.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

    private async void LoginButton_Clicked(object sender, EventArgs e) {
        if(auth(Username.Text, Password.Text) == "admin") {
            await SecureStorage.SetAsync("usertype", "admin");
            await Shell.Current.GoToAsync("/adminhome");
        }
        else if(auth(Username.Text, Password.Text) == "user") {
            await SecureStorage.SetAsync("usertype", "user");
            await Shell.Current.GoToAsync("/mechanichome");
        }
        else {
            await DisplayAlert("Login failed", "Username or password if invalid", "Try again");
        }
    }

    String auth(string username, string password) {
        if(Username.Text == "admin" && Password.Text == "1234") {
            return "admin";
        }
        else if(Username.Text == "user" && Password.Text == "1234") {
            return "user";
        }
        return "";
    }
}