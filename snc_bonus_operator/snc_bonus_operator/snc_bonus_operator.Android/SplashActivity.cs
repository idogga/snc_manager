using Android.App;
using Android.Content;
//using Android.Gms.Common;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using System.Threading.Tasks;

namespace snc_bonus_operator.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", Label = "СНК-Менеджер", NoHistory = true, MainLauncher = true,
           ScreenOrientation = global::Android.Content.PM.ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {
        public override void OnCreate(Bundle savedInstanceState, PersistableBundle persistentState)
        {
            base.OnCreate(savedInstanceState, persistentState);
        }

        protected override void OnResume()
        {
            base.OnResume();
            LoadData();
        }

        private void LoadData()
        {
            StarMainActivity();
        }

        private void StarMainActivity()
        {
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }
}