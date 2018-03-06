using System;
using Xamarin.Forms;

namespace snc_bonus_operator.Settings
{
    public enum InternetStatus
    {
        /// <summary>
        /// Все работает
        /// </summary>
        Online,
        /// <summary>
        /// Сервер не работает
        /// </summary>
        ServerBroke,
        /// <summary>
        /// У пользователя нет интернета
        /// </summary>
        UserBroke
    }
    public class AppSettings
    {
        /// <summary>
        /// Отображение оповещений
        /// </summary>
        public bool ShowNotifications { get; set; } = true;
        /// <summary>
        /// Язык интерфейса
        /// </summary>
        public string CurrentLanguage { get; set; } = "";

        /// <summary>
        /// Колличeство запусков
        /// </summary>
        public int NumberOfLoadApp { get; set; } = 0;

        /// <summary>
        /// Показывалось ли введение
        /// </summary>
        public bool IsIntroShown { get; set; } = false;

        /// <summary>
        /// Загружалось ли база данных
        /// </summary>
        public bool IsDataBaseLoad { get; set; } = false;

        /// <summary>
        /// Стоит ли сохранять введеные пользователем даные в заказе
        /// </summary>
        public bool SaveOrderMemory { get; set; } = true;

        /// <summary>
        /// Использовать как основную карту гугл
        /// </summary>
        public bool IsGoogleMapSelect { get; set; } = true;


        /// <summary>
        /// Дата последнего обновления данных
        /// </summary>
        public DateTime LastUpdate { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Использование считывателя пальцев.
        /// </summary>
        public bool IsUseFingerprint { get; set; } = true;

        /// <summary>
        /// Дата последнего обновления SupportPack
        /// </summary>
        public DateTime LastUpdateSupportPack { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Высота экрана
        /// </summary>
        public double ScreenHeight { get; set; } = 0;

        /// <summary>
        /// Ширина экрана
        /// </summary>
        public double ScreenWidth { get; set; } = 0;

        /// <summary>
        /// Исползование вибрации
        /// </summary>
        public bool UseVibration { get; set; } = true;

        /// <summary>
        /// Использование кнопки новая заявки
        /// </summary>
        public bool OrderButtonWork { get; set; } = true;

        /// <summary>
        /// Использовании кнопки доступа информации к картам.
        /// </summary>
        public bool CardButtonWork { get; set; } = true;

        /// <summary>
        /// Доступ к новостям
        /// </summary>
        public bool NewsButtonWork { get; set; } = true;

        /// <summary>
        /// Доступ к новостям
        /// </summary>
        public bool UseSMSIssuer { get; set; } = false;

        /// <summary>
        /// Доступ к новостям
        /// </summary>
        public bool UseSMSUser { get; set; } = false;

        /// <summary>
        /// Доступ к новостям
        /// </summary>
        public bool UseEmailUser { get; set; } = false;

        /// <summary>
        /// Блокировка устройства
        /// </summary>
        public bool IsDeviceBlock { get; set; } = false;

        /// <summary>
        /// Доступность интернета
        /// </summary>
        public InternetStatus IsInetAvaliable { get; set; } = InternetStatus.Online;

        
         

        public void SaveSetting(int index, string v)
        {
            var db = new SettingsDB();
            db.SaveSetting(index, v);
        }

        public void LoadSettings()
        {
            var db = new SettingsDB();
            db.LoadSettings();
        }

        /// <summary>
        /// Текущий шрифт
        /// </summary>
        public string CurrenFont(bool isBold)
        {
            switch(Device.RuntimePlatform)
            {
                case Device.iOS:
                    return "";
                case Device.Android:
                default:
                    if (isBold)
                    {
                        return "Ubuntu-Bold.ttf#Ubuntu-Bold";
                    }
                    else
                    {
                        return "Ubuntu-Regular.ttf#Ubuntu-Regular";
                    }
            }
        }
    }
}
