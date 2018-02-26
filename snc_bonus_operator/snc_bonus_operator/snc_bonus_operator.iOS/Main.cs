using System;
using UIKit;

namespace snc_bonus_operator.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        static void Main(string[] args)
        {
            try
            {
                Logger.WriteLine("Main start");
                UIApplication.Main(args, null, "AppDelegate");
                Logger.WriteLine("Main start");
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
    }
}
