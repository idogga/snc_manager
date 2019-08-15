using snc_bonus_operator.Settings;
using System;
using Xamarin.Forms;

namespace snc_bonus_operator.Protocol
{
    public class Issuer
    {
        /// <summary>
        /// Уникальный номер эмитента
        /// </summary>
        public int IssuerId { get; set; } = 1;

        /// <summary>
        /// Название эмитента
        /// </summary>
        public string IssuerTitle { get; set; } = string.Empty;

        /// <summary>
        /// Почтовый адрес  эмитента
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Адрес эмитента
        /// </summary>
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// IP эмитента для базовых команд
        /// </summary>
        public string IP { get; set; } = string.Empty;

        /// <summary>
        /// Port эмитента для базовых команд
        /// </summary>
        public int Port { get; set; } = 0;

        /// <summary>
        /// Дополнительная информация об эмитенте
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Сайт эмитента
        /// </summary>
        public string Link { get; set; } = string.Empty;

        /// <summary>
        /// Ссылка на сайт с информацией профиля пользователя
        /// </summary>
        public string MyProfileLink { get; set; } = string.Empty;

        /// <summary>
        /// Основной телефоный номер эмитента
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Телефон поддержки эмитента
        /// </summary>
        public string PhoneSupport { get; set; } = string.Empty;

        /// <summary>
        /// Основной цвет  эмитента
        /// </summary>
        public string MainHexString { get; set; } = "#f62020";

        /// <summary>
        /// Вторичный цвет эмитента
        /// </summary>
        public string BackgroundHexString { get; set; } = "#c5c5c5";

        // <summary>
        /// Цвет букв эмитента
        /// </summary>
        public string LettersHexString { get; set; } = "#000000";

        /// <summary>
        /// Цвет ссылок эмитента
        /// </summary>
        public string LinkHexString { get; set; } = "#0900ff";

        // <summary>
        /// Цвет заголовка эмитента
        /// </summary>
        public string HeaderHexString { get; set; } = "#eae22c";

        // <summary>
        /// Цвет обозначения выделения эмитента
        /// </summary>
        public string SelectHexString { get; set; } = "#c1c1c0";

        /// <summary>
        /// Цвет дополнительной информации эмитента
        /// </summary>
        public string SublettersHexString { get; set; } = "#a1a6aa";

        // <summary>
        /// Дополнительный цвет
        /// </summary>
        public string ExtendHexString { get; set; } = "#a1a6aa";

        // <summary>
        /// Цвет подложки
        /// </summary>
        public string ObjectBackgroundHexString { get; set; } = "#ffffff";

        /// <summary>
        /// Ссылка на изображение эмитента
        /// </summary>
        public string ImagePath { get; set; } = string.Empty;

        /// <summary>
        /// Валюта эмитента
        /// </summary>
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Последнее обновление поля
        /// </summary>
        public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.Now;

        public Issuer()
        {

        }


        /// <summary>
        /// Сохранение настройки
        /// </summary>
        /// <param name="index">Индекс настройки</param>
        /// <param name="v">Значение настройки</param>
        public void SaveSetting(int index, string v)
        {
            var db = new SettingsDB();
            db.SaveSetting(index, v);
        }

        public Color LighterColor(Color color)
        {
            double percent = 0.6;
            Color _color = color;
            double R = _color.R;
            double G = _color.G;
            double B = _color.B;
            _color = Color.FromRgba(R, G, B, percent);
            return _color;
        }

        public String HexConverter(Color c)
        {
            String rtn = String.Empty;
            try
            {
                short R = (short)(c.R * 256);
                short G = (short)(c.G * 256);
                short B = (short)(c.B * 256);
                short A = (short)(c.A * 256);
                rtn = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", A, R, G, B);
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }

            return rtn;
        }

        public void LoadDictionary()
        {
            MobileStaticVariables.MainIssuer.SelectHexString = MobileStaticVariables.MainIssuer.HexConverter(MobileStaticVariables.MainIssuer.LighterColor(Color.FromHex(MobileStaticVariables.MainIssuer.MainHexString)));
            MobileStaticVariables.MainIssuer.SublettersHexString = MobileStaticVariables.MainIssuer.HexConverter(MobileStaticVariables.MainIssuer.LighterColor(Color.FromHex(MobileStaticVariables.MainIssuer.LettersHexString)));

            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.Issuer, MobileStaticVariables.MainIssuer.IssuerId.ToString());
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.IssuerTitle, MobileStaticVariables.MainIssuer.IssuerTitle);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.Address, MobileStaticVariables.MainIssuer.Address);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.Description, MobileStaticVariables.MainIssuer.Description);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.Link, MobileStaticVariables.MainIssuer.Link);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.MyProfileLink, MobileStaticVariables.MainIssuer.MyProfileLink);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.Phone, MobileStaticVariables.MainIssuer.Phone);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.EmailIssuer, MobileStaticVariables.MainIssuer.Email);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.MainHexColor, MobileStaticVariables.MainIssuer.MainHexString);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.Currency, MobileStaticVariables.MainIssuer.Currency);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.BackgroundHexString, MobileStaticVariables.MainIssuer.BackgroundHexString);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.LettersHexString, MobileStaticVariables.MainIssuer.LettersHexString);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.LinkHexString, MobileStaticVariables.MainIssuer.LinkHexString);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.HeaderHexString, MobileStaticVariables.MainIssuer.HeaderHexString);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.SelectHexString, MobileStaticVariables.MainIssuer.SelectHexString);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.ExtendHexString, MobileStaticVariables.MainIssuer.ExtendHexString);
            MobileStaticVariables.MainIssuer.SaveSetting((int)SettingsEnum.ObjectBackgroundHexString, MobileStaticVariables.MainIssuer.ObjectBackgroundHexString);

            App.Current.Resources["LinkColor"] = Color.FromHex(MobileStaticVariables.MainIssuer.LinkHexString);
            App.Current.Resources["MainColor"] = Color.FromHex(MobileStaticVariables.MainIssuer.MainHexString);
            App.Current.Resources["BackgroundColors"] = Color.FromHex(MobileStaticVariables.MainIssuer.BackgroundHexString);
            App.Current.Resources["LetterColor"] = Color.FromHex(MobileStaticVariables.MainIssuer.LettersHexString);
            App.Current.Resources["HeaderColor"] = Color.FromHex(MobileStaticVariables.MainIssuer.HeaderHexString);
            App.Current.Resources["SelectionColor"] = Color.FromHex(MobileStaticVariables.MainIssuer.SelectHexString);
            App.Current.Resources["SublettersColor"] = Color.FromHex(MobileStaticVariables.MainIssuer.SublettersHexString);
            App.Current.Resources["ObjectBackgroundColor"] = Color.FromHex(MobileStaticVariables.MainIssuer.ObjectBackgroundHexString);
        }
    }
}
