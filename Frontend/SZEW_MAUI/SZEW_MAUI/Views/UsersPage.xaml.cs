namespace SZEW_MAUI.Views;

public partial class UsersPage : ContentPage
{
	List<User> users;
	public UsersPage()
	{
		InitializeComponent();
		users = API.getUserList();
		if(users == null) {
			errorMessage.IsVisible = true;
		}
		else {
			usersListView.ItemsSource = users;
		}
	}
}