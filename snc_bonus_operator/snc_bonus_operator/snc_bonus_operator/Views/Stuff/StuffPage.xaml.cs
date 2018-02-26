using Newtonsoft.Json;
using snc_bonus_operator.Protocol;
using snc_bonus_operator.Settings;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace snc_bonus_operator.Stuff
{
    public partial class StuffPage : ContentPage
    {
        ObservableCollection<CollegueModel> StuffList = new ObservableCollection<CollegueModel>();
        public StuffPage()
        {
            InitializeComponent();
            if(MobileStaticVariables.UserInfo.UserType == UserTypes.Admin)
            {
                var toolbar = new ToolbarItem()
                {
                    Icon = "ic_add_user.png",
                    Command = new Command(AddColegue_Clicked)
                };
                this.ToolbarItems.Add(toolbar);
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            StartLoading();
            LoadPage();
        }

        private async void LoadPage()
        {
            StartLoading();
            var cancellationTokenSource = new CancellationTokenSource();
            StuffList.Clear();
            await Task.Factory.StartNew(() =>
            {
                LoadStuff();
            });
            EndLoading();
        }

        private void LoadStuff()
        {
            try
            {
                string crewInfo = "";
                StuffModel crew = new StuffModel();
                crew.MobileDevicKey = MobileStaticVariables.UserInfo.MobileDeviceKey;
                crewInfo = MobileStaticVariables.WebUtils.SendAuthRequest("GetMyCrew", crew);
                
                Logger.WriteLine("crewInfo : " + crewInfo);
                if (crewInfo == "")
                    throw new Exception("crewInfo не получены");
                crew = JsonConvert.DeserializeObject<StuffModel>(crewInfo);
                if (crew.ResultState == RequestResult.Results.Success)
                {
                    StuffList.Clear();
                    MobileStaticVariables.UserInfo.Stuff = crew.ColegueList;
                    MobileStaticVariables.UserInfo.SaveSetting((int)SettingsEnum.Stuff, JsonConvert.SerializeObject(MobileStaticVariables.UserInfo.Stuff));
                    foreach (var item in crew.ColegueList)
                    {
                        var view = new CollegueModel();
                        view.Name = item.Name;
                        view.Shop = item.ShopName;
                        view.Posision = item.Position;
                        view.Data = item;
                        if(item.UserState == UserStates.Blocked)
                        {
                            view.Name += " (заблокирован)";
                        }
                        StuffList.Add(view);
                    }
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        listView.ItemsSource = StuffList;
                        EndLoading();
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (IsVisible)
                        {
                            await DisplayAlert("Внимание", crew.TranslateResult(crew.ResultState), "Хорошо");
                            EndLoading();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await DisplayAlert("Внимание", "Не удается соедениться с сервером", "Повторить");
                    EndLoading();
                });
            }
            finally
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    EndLoading();
                });
            }
        }

        private async void AddColegue_Clicked()
        {
            Logger.WriteLine("On add colegue clicked");
            await Navigation.PushAsync(new NewColleguePage());
        }

        void StartLoading()
        {
            backgroundDark.BackgroundColor = new Color(0, 0, 0, 0.5);
            backgroundDark.IsVisible = true;
            mainLayout.IsEnabled = false;
            IndicatorLayout.Start();
        }

        void EndLoading()
        {
            backgroundDark.IsVisible = false;

            mainLayout.IsEnabled = true;
            IndicatorLayout.Stop();
        }

        class CollegueModel
        {
            public Colegue Data { get; set; } = new Colegue();
            public string Name { get; set; } = "";
            public string Shop { get; set; } = "";
            public string Posision { get; set; } = "";
        }

        private async void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selected = (CollegueModel)e.SelectedItem;
            Logger.WriteLine("User select " + selected.Name);
            await Navigation.PushAsync(new DetailColeguePage(selected.Data));
        }
    }
}