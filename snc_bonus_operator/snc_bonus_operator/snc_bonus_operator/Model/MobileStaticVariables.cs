using snc_bonus_operator.Model.QR_code;
using snc_bonus_operator.Protocol;
using snc_bonus_operator.Settings;

namespace snc_bonus_operator
{
    public enum UserStatusEnum
    {
        UnRegister = 0,
        Unlogged = 1,
        Logged = 2
    }

    public static class MobileStaticVariables
    {
        public static AppSettings UserAppSettings = new AppSettings();

        /// <summary>
        /// Пользовательские данные
        /// </summary>
        public static UserAccount UserInfo = new UserAccount();

        /// <summary>
        /// Настройки соединения с сервером
        /// </summary>
        public static ConnectionSettings ConectSettings { get; set; } = new ConnectionSettings();
        /// <summary>
        /// Запросы к HTTP службе
        /// </summary>
        public static WebRequestUtils WebUtils { get; set; } = new WebRequestUtils();

        /// <summary>
        /// Состояние пользователя
        /// </summary>
        public static UserStatusEnum UserStatus { get; set; } = UserStatusEnum.UnRegister;

        public static ClientQR QRClient { get; set; } = new ClientQR();


        /// <summary>
        /// Информация о владельце клуба
        /// </summary>
        public static Issuer MainIssuer { get; set; } = new Issuer();
    }
}
