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
            await Shell.Current.GoToAsync("/adminshell");
        }
        else if(userType == 2) {
            await Shell.Current.GoToAsync("/mechanicshell");
        }
        else {
            await DisplayAlert("Nie uda³o siê zalogowaæ", "Login lub has³o s¹ nieprawid³owe.", "Spróbuj ponownie");
        }
    }
}