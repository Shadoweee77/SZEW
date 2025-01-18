using SZEW_MAUI.Views;
namespace SZEW_MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Register all routes
            Routing.RegisterRoute("mechanicshell", typeof(MechanicShell));
            Routing.RegisterRoute("adminshell", typeof(AdminShell));
            Routing.RegisterRoute("login", typeof(LoginPage));
            Routing.RegisterRoute("loading", typeof(LoadingPage));
        }
    }
}
