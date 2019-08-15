using System;
using UIKit;

namespace snc_bonus_operator.iOS
{
    public class Application
    {
        static void Main(string[] args)
        {
            try
            {
                UIApplication.Main(args, null, "AppDelegate");
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
    }
}
