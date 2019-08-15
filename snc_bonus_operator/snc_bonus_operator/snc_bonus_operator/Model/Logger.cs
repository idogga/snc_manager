using System;
//using snc_bonus.Interfaces;
using System.Diagnostics;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public static class Logger
    {
        public static void WriteError(Exception ex)
        {

            try
            {
                string result = string.Format("LOG ERROR [{0:HH:mm:ss.ffff}] - {1}", DateTime.Now, ex.Message);
                Debug.WriteLine(result);
#if DEBUG
                //DependencyService.Get<IToastMessage>()?.LongToastAlert(ex.Message);
#endif
                //HockeyApp.MetricsManager.TrackEvent(ex.Message);
            }
            catch
            { }
#if __ANDROID__

#elif __IOS__

#elif __MOBILE__
            
#endif
        }

        public static void WriteLine(string message)
        {
            try
            {
                string result = string.Format("LOG [{0:HH:mm:ss.ffff}] - {1}", DateTime.Now, message);
                Debug.WriteLine(result);
            }
            catch
            { }
#if __ANDROID__
            
#elif __IOS__

#elif __MOBILE__
            
#endif
        }
    }
}
