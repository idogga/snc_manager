using snc_bonus_operator.Interfaces;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
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

        private async void wrightToDeveloper_Clicked(object sender, EventArgs e)
        {
            try
            {
                var message = new EmailMessage
                {
                    Subject = "Письмо разработчикам от пользователя-СНК Менеджер",
                    Body = "",
                    To = new List<string>() { "develop.snc@gmail.com" },
                    //Cc = ccRecipients,
                    //Bcc = bccRecipients
                };
                await Email.ComposeAsync(message);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
    }
}