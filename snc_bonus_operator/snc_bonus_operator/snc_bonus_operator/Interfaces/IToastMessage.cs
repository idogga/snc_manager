using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using Plugin.Toasts;

namespace snc_bonus_operator.Interfaces
{
    public interface IToastMessage
    {
        void LongToastAlert(string message);
        void ShortToastAlert(string message);
    }

    public static class Messages
    {
        //public static async void ShowNotification(INotificationOptions options)
        //{
        //    var notificator = DependencyService.Get<IToastNotificator>();
        //    if (notificator != null && options != null)
        //    {
        //        var result = await notificator.Notify(options);
        //    }
        //}
    }
}
