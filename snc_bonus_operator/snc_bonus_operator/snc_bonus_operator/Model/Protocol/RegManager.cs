using Newtonsoft.Json;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class RegManager : UserRequisites
    {
        /// <summary>
        /// Уникальный номер устройства
        /// </summary>
        [JsonProperty("MDK")]
        public int MobileDeviceKey { get; set; } = 0;
        /// <summary>
        /// Роль пользователя
        /// </summary>
        [JsonProperty("UT")]
        public UserTypes UserType { get; set; } = UserTypes.Client;

        /// <summary>
        /// Пароль
        /// </summary>
        [JsonProperty("PAS")]
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Тип мобильного приложения (Заправка, Бонусы и т.д.)
        /// </summary>
        [JsonProperty("AK")]
        public RegisterInfo.AppKeys AppKey { get; set; } = RegisterInfo.AppKeys.Fuel;

        /// <summary>
        /// Уникальный номер устройства для привязки при регистрации пользователя
        /// </summary>
        [JsonProperty("IM")]
        public string Imei { get; set; } = "";

        /// <summary>
        /// Название устройства
        /// </summary>
        [JsonProperty("DN")]
        public string DeviceName { get; set; } = "";

        /// <summary>
        /// Версия приложения
        /// </summary>
        [JsonProperty("AV")]
        public string AppVersion { get; set; } = "";

        /// <summary>
        /// Личный публичный ключ зарегистрированного клиента
        /// </summary>
        [JsonProperty("PB")]
        public string PublicKey { get; set; } = "";
        /// <summary>
        /// Личный приватный ключ зарегистрированного клиента
        /// </summary>
        [JsonProperty("PR")]
        public string PrivateKey { get; set; } = "";
        /// <summary>
        /// IP эмитента для приватных команд
        /// </summary>
        [JsonProperty("IP")]
        public string IP { get; set; } = string.Empty;
        /// <summary>
        /// Port эмитента для приватных команд
        /// </summary>
        [JsonProperty("PO")]
        public int Port { get; set; } = 0;
        /// <summary>
        /// Уникальный идентификатор устройства для рассылки оповещений
        /// </summary>
        [JsonProperty("NF")]
        public string NotificationToken { get; set; } = "";

        /// <summary>
        /// Название топика для рассылки оповещений для устройств пользователя
        /// </summary>
        [JsonProperty("NT")]
        public string NotificationTopic { get; set; } = string.Empty;

        /// <summary>
        /// Список Администрируемых магазинов
        /// </summary>
        [JsonProperty("SL")]
        public List<ShopModel> ShopList { get; set; } = new List<ShopModel>();

        [JsonProperty("St")]
        public List<Colegue> Stuff { get; set; } = new List<Colegue>();
    }

    public class ShopModel
    {
        [JsonProperty("SK")]
        public int ShopKey { get; set; } = 0;
        [JsonProperty("SN")]
        public string Name { get; set; } = "";
    }
}
