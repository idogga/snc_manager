using System.ComponentModel;

namespace snc_bonus_operator
{
    public class ShopItemView : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string obj)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(obj));
        }

        string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }

        string _value;


        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                OnPropertyChanged("Value");
            }
        }


        public ShopItemView(string key, string value)
        {
            Name = key;
            Value = value;
        }
    }
}
