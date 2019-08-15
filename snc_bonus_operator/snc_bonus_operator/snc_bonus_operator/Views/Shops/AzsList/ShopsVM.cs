using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public class ShopsVM : INotifyPropertyChanged
    {
        #region Properties
        #region Load Visibility
        private bool _isLoadVisible = false;
        public bool IsLoadVisible
        {
            get
            {
                return _isLoadVisible;
            }
            set
            {
                _isLoadVisible = value;
                OnPropertyChanged("IsLoadVisible");
            }
        }
        #endregion

        #region Shop List
        private ObservableCollection<ShopView> _shops = new ObservableCollection<ShopView>();
        public ObservableCollection<ShopView> ShopsList { get => _shops;
            set
            {
                _shops = value;
                OnPropertyChanged("ShopsList");
            }
        }
        #endregion

        #endregion
        ShopCollection _shopCollection;
        Timer _timer;
        bool _isFirstLoad = true;
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string obj)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
        }

        public ShopsVM()
        {
            _shopCollection = new ShopCollection();
            IsLoadVisible = true;
            
            foreach (var azs in MobileStaticVariables.UserInfo.ShopList)
            {
                var a = new ShopView(azs);
                ShopsList.Add(a);
            }
        }

        public void StartUpdate()
        {
            _timer = new Timer(LoadStauts, null, 0, 2000);
        }

        public void StopUpdate()
        {
            _timer.Dispose();
        }

        private void LoadStauts(object obj)
        {
            try
            {
                var response = new CheckAzsSellerRequest();
                response.MobileDeviceKey = MobileStaticVariables.UserInfo.MobileDeviceKey;
                response.AzsList = MobileStaticVariables.UserInfo.ShopList.Select(x => x.ShopKey).ToList();
                var req = MobileStaticVariables.WebUtils.SendMobileRequest<CheckAzsSellerResponse>(RequestTagEnum.CheckAzsSeller, response);
                UpdateAzs(req);
            }
            catch(Exception e)
            {
                Logger.WriteError(e);
            }
            if (_isFirstLoad)
            {
                Device.BeginInvokeOnMainThread(() => IsLoadVisible = false);
                _isFirstLoad = false;
            }
        }

        private void UpdateAzs(CheckAzsSellerResponse req)
        {
            foreach(var azs in ShopsList)
            {
                var requsite = req.AzsListConfig.FirstOrDefault(x => x.Cod_Azs == azs.ShopModel.ShopKey);
                if (requsite == null)
                {
                    azs.UpdateStatus(AzsOnlineStatus.Offline);
                    continue;
                }
                azs.UpdateStatus(requsite);
            }
        }
    }
}
