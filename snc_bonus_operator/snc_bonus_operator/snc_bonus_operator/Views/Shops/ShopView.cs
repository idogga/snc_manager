using snc_bonus_operator.Protocol;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public class ShopView :  INotifyPropertyChanged
    {
        private string azsname = "";
        public string AzsName
        {
            get => azsname;
            set
            {
                azsname = value;
                OnPropertyChanged("AzsName");
            }
        }
        private string key;
        public string Key {
            get => key;
            set
            {
                key = value;
                OnPropertyChanged("Key");
            }
        }
        private Color status;
        public Color Status {
            get => status;
            set
            {
                status = value;
                OnPropertyChanged("Status");
            }
        }
        private AzsOnlineStatus _status;
        public ShopModel ShopModel;
        private AzsConfigItem requsite;
        private const int _warningMinuteDelay = 5;
        private const int _errorMinuteDelay = 15;



        public ShopView(ShopModel azs)
        {
            requsite = new AzsConfigItem(azs.ShopKey);
            ShopModel = azs;
               AzsName = azs.Name;
            Key = azs.ShopKey.ToString();
            UpdateStatus(AzsOnlineStatus.Warning);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string obj)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));

        }
        public void UpdateStatus(AzsOnlineStatus newstatus)
        {
            if(newstatus!=_status)
            {
                _status = newstatus;
                Status = GetColor(_status);
            }
        }

        private Color GetColor(AzsOnlineStatus pumpState)
        {
            switch (pumpState)
            {
                case AzsOnlineStatus.Offline:
                    return (Color)Application.Current.Resources["DeclineColor"];
                case AzsOnlineStatus.Online:
                    return (Color)Application.Current.Resources["ObjectBackgroundColor"];
                case AzsOnlineStatus.Warning:
                default:
                    return (Color)App.Current.Resources["ConsiderationColor"];
            }
        }

        /// <summary>
        /// Обновление статуса АЗС
        /// </summary>
        /// <param name="requsite"></param>
        internal void UpdateStatus(AzsConfigItem requsite)
        {
            if (requsite.LastUpdate > DateTime.Now.AddMinutes(-_warningMinuteDelay))
            {
                UpdateStatus(AzsOnlineStatus.Online);
                return;
            }
            if (requsite.LastUpdate > DateTime.Now.AddMinutes(-_errorMinuteDelay))
            {
                UpdateStatus(AzsOnlineStatus.Warning);
                return;
            }
            UpdateStatus(AzsOnlineStatus.Offline);
        }
    }
}