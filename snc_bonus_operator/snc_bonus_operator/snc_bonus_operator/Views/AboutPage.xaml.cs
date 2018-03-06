using Plugin.Messaging;
using snc_bonus_operator.Interfaces;
using System;

using Xamarin.Forms;

namespace snc_bonus_operator
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            try
            {
                InitializeComponent();
                version.Text = string.Format("Версия приложения : {0}", DependencyService.Get<IDevice>().GetVersion());
                string copy = string.Format("© {0} {1}", DateTime.Now.Year, "\"Группа компаний Сибнефтекарт\"");
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