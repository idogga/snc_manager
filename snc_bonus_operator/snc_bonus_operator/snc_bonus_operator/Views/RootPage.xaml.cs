using snc_bonus_operator.Accounting;
using snc_bonus_operator.Cash;
using snc_bonus_operator.Confirmation;
using snc_bonus_operator.Interfaces;
using snc_bonus_operator.Shops;
using System;
using System.Diagnostics;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public partial class RootPage : MasterDetailPage
    {
        #region 
        Stopwatch _exit_timer = new Stopwatch();
        bool _exit_app = false;
        public RootPage()
        {
            InitializeComponent();
            IsPresented = false;
            Detail = new NavigationPage(new Stuff.StuffPage());
            myStuffLayout.BackgroundColor = (Color)Application.Current.Resources["SelectionColor"];
            notificationLayout.IsVisible = MobileStaticVariables.UserInfo.UserType == Protocol.UserTypes.Admin;
        }

        #endregion

        #region 

        private void OnMyProfileButtonClicked(object sender, EventArgs e)
        {
            try
            {
                mainLayout.IsEnabled = false;
                RefreshMenuItem();
                myProfileLayout.BackgroundColor = (Color)App.Current.Resources["SelectionColor"];
                Logger.WriteLine("Переход на MyProfilePage");
                Detail = new NavigationPage(new MyProfilePage());
                IsPresented = false;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            finally
            {
                mainLayout.IsEnabled = true;
            }
        }

        private void OnAllPurchesLayoutClicked(object sender, EventArgs e)
        {
            try
            {
                mainLayout.IsEnabled = false;
                RefreshMenuItem();
                allPurchesLayout.BackgroundColor = (Color)App.Current.Resources["SelectionColor"];
                Logger.WriteLine("Переход на TrasactionsPage");
                Detail = new NavigationPage(new TransactionPage());
                IsPresented = false;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            finally
            {
                mainLayout.IsEnabled = true;
            }
        }

        private void OnNotificationLayoutClicked(object sender, EventArgs e)
        {
            try
            {
                mainLayout.IsEnabled = false;
                RefreshMenuItem();
                notificationLayout.BackgroundColor = (Color)App.Current.Resources["SelectionColor"];
                Logger.WriteLine("Переход на NewsPage");
                Detail = new NavigationPage(new ConfirmPage());
                IsPresented = false;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            finally
            {
                mainLayout.IsEnabled = true;
            }
        }

        private void OnSettingsLayoutClicked(object sender, EventArgs e)
        {
            try
            {
                mainLayout.IsEnabled = false;
                RefreshMenuItem();
                settingsLayout.BackgroundColor = (Color)App.Current.Resources["SelectionColor"];
                Logger.WriteLine("Переход на SettingsPage");
                Detail = new NavigationPage(new SettingsPage());
                IsPresented = false;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            finally
            {
                mainLayout.IsEnabled = true;
            }
        }

        private void OnMyStuffButtonClicked(object sender, EventArgs e)
        {
            try
            {
                mainLayout.IsEnabled = false;
                RefreshMenuItem();
                myStuffLayout.BackgroundColor = (Color)App.Current.Resources["SelectionColor"];
                Logger.WriteLine("Переход на UserCardsPage");
                Detail = new NavigationPage(new Stuff.StuffPage());
                IsPresented = false;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            finally
            {
                mainLayout.IsEnabled = true;
            }
        }
        
        private void OnAboutLayoutnClicked(object sender, EventArgs e)
        {
            try
            {
                mainLayout.IsEnabled = false;
                RefreshMenuItem();
                aboutLayout.BackgroundColor = (Color)App.Current.Resources["SelectionColor"];
                Logger.WriteLine("Переход на AboutPage");
                Detail = new NavigationPage(new AboutPage());
                IsPresented = false;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            finally
            {
                mainLayout.IsEnabled = true;
            }
        }

        private void OnCloseButtonClicked(object sender, EventArgs e)
        {
            try
            {
                mainLayout.IsEnabled = false;
                RefreshMenuItem();
                closeLayout.BackgroundColor = (Color)App.Current.Resources["SelectionColor"];
                Logger.WriteLine("Close session");
                try
                {

                    if (MobileStaticVariables.UserAppSettings.UseVibration)
                    {
                        Vibration.Vibrate();
                    }
                    IsPresented = false;
                }
                catch { }

                MobileStaticVariables.UserInfo.DeleteInfo();
                MobileStaticVariables.UserStatus = UserStatusEnum.UnRegister;
                App.Current.MainPage = new NavigationPage(new Login.LoginPage());
                //MobileStaticVariables.UserInfo.UnAuthorized();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            finally
            {
                mainLayout.IsEnabled = true;
            }
        }

        protected override bool OnBackButtonPressed()
        {
            try
            {
                if (_exit_timer.ElapsedMilliseconds > 2000)
                {
                    _exit_app = false;

                    _exit_timer.Stop();
                }

                var page = (Detail as NavigationPage).CurrentPage;
                if (page is Stuff.StuffPage)
                {
                    if (_exit_app)
                    {
                        DependencyService.Get<IDevice>()?.CloseApp();
                    }
                    else
                    {
                        _exit_app = true;
                        DependencyService.Get<IToastMessage>()?.ShortToastAlert("Нажмите еще раз для выхода");
                        Logger.WriteLine("Нажмите еще раз и выйдите");
                        _exit_timer.Restart();
                    }
                }
                else if (page is ConfirmPage ||
                         page is ShopListPage ||
                         page is TransactionPage ||
                         page is AboutPage ||
                         page is MyProfilePage ||
                         page is SettingsPage)
                {
                    Detail = new NavigationPage(new Stuff.StuffPage());
                    IsPresented = false;
                    RefreshMenuItem();
                    myStuffLayout.BackgroundColor = (Color)App.Current.Resources["SelectionColor"];
                }
                else
                {
                    return base.OnBackButtonPressed();
                }

            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            return true;
        }
        #endregion

        #region 
        /// <summary>
        /// Убрать все выделеные кнопки
        /// </summary>
        private void RefreshMenuItem()
        {
            allPurchesLayout.BackgroundColor = Color.Transparent;
            notificationLayout.BackgroundColor = Color.Transparent;
            settingsLayout.BackgroundColor = Color.Transparent;
            myProfileLayout.BackgroundColor = Color.Transparent;
            //shopsLayout.BackgroundColor = Color.Transparent;
            myStuffLayout.BackgroundColor = Color.Transparent;
            aboutLayout.BackgroundColor = Color.Transparent;
            closeLayout.BackgroundColor = Color.Transparent;
        }
        #endregion
    }
}