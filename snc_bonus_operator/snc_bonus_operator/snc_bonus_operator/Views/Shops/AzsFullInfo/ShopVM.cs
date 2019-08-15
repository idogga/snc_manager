using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public class ShopVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        #region Propertyes

        private AzsInfo _info = new AzsInfo();
        public AzsInfo Info
        {
            get => _info;
            set
            {
                _info = value;
                OnPropertyChanged(nameof(Info));
            }
        }
        #endregion
        private void OnPropertyChanged(string obj)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
        }

        #region private fields
        private ShopView shop;
        Timer _timer;
        private const int _timerDelay = 2000;
        #endregion
        public void SetContext(ShopView selected)
        {
            shop = selected;
            Task.Factory.StartNew(() =>
            {
                var request = new GetAzsInfoRequest(selected.ShopModel.ShopKey, MobileStaticVariables.UserInfo.MobileUserKey);
                var response = MobileStaticVariables.WebUtils.SendMobileRequest<AzsInfo>(RequestTagEnum.GetAzsInfo, request);
                if(response!=null)
                {
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Info = response;
                    });
                }
            });
        }
    }
}
