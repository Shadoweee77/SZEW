using SZEW_MAUI.Views;
namespace SZEW_MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Register all routes
            Routing.RegisterRoute("mechanichome", typeof(MechanicHomePage));
            Routing.RegisterRoute("adminhome", typeof(AdminHomePage));
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("loading", typeof(LoadingPage));
        }
    }
}
