namespace SZEW_MAUI.Views;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}
    private async void LogoutButton_Clicked(object sender, EventArgs e) {
        if(await DisplayAlert("Czy na pewno?", "Zostaniesz wylogowany.", "Tak", "Nie")) {
            SecureStorage.RemoveAll();
            await Shell.Current.GoToAsync("///login");
        }
    }
}