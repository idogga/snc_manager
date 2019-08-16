using System.ComponentModel;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public abstract class CommonVM:INotifyPropertyChanged
    {


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


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propName)
        {
            Device.BeginInvokeOnMainThread(() => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName)));
        }
    }
}
