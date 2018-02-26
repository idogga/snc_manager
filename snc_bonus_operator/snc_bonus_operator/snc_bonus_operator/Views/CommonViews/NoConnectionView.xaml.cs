using Xamarin.Forms;

namespace snc_bonus_operator
{
    public partial class NoConnectionView : ContentView
    {
        public NoConnectionView()
        {
            InitializeComponent();
            switch(MobileStaticVariables.UserAppSettings.IsInetAvaliable)
            {
                case Settings.InternetStatus.ServerBroke:
                    descriptionLabel.Text = "Сервер сейчас недоступен";
                    break;
                default:
                case Settings.InternetStatus.UserBroke:
                    descriptionLabel.Text = "Упс, нет интернет- соединения";
                    break;
            }
        }
    }
}