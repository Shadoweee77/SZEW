namespace SZEW_MAUI.Views;

public partial class MechanicHomePage : ContentPage
{
	public MechanicHomePage()
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