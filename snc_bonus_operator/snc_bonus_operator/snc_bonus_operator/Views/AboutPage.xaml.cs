using Plugin.Messaging;
using snc_bonus_operator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace snc_bonus_operator
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            try
            {
                InitializeComponent();
                version.Text = string.Format("Версия приложени : {0}", DependencyService.Get<IDevice>().GetVersion());
                string copy = string.Format("© 2018 {0}", "\"Группа компаний Сибнефтекарт\"");
                copyright.Text = copy;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        private void wrightToDeveloper_Clicked(object sender, EventArgs e)
        {
            try
            {
                var emailMessanger = CrossMessaging.Current.EmailMessenger;
                var builder = new EmailMessageBuilder()
                    .To("develop.snc@gmail.com")
                    .Subject("Письмо разработчикам от пользователя-СНК Менеджер")
                    .Body("")
                    .Build();

                if (emailMessanger.CanSendEmail)
                {
                    emailMessanger.SendEmail(builder);
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
    }
}