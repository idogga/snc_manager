using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using snc_bonus_operator.Ios.DependencyServices;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(IosDevice))]
namespace snc_bonus_operator.Ios.DependencyServices
{
    public class IosDevice : snc_bonus_operator.Interfaces.IDevice
    {
#warning Осторожно, метод не работает
        public string GetIdentifier()
        {
            var result = "2653545687678";

            return result;
        }

#warning Осторожно, метод не работает
        public string GetVersion()
        {
            var result = "01";
            return result;
        }

#warning Осторожно, метод не работает
        public double CalculateWidth(string text)
        {
            var length = 333;
            return length;
        }

#warning Осторожно, метод не работает
        public void CloseApp()
        {

        }
    }
}
