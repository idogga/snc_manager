using Newtonsoft.Json;
using System;


namespace snc_bonus_operator.Protocol
{
    public enum UserStates
    {
        Ungeristered = 0,
        Registered = 1,
        Blocked = 2,
        Deleted = 3,
    }
    public enum UserTypes
    {
        Client = 0,
        Admin = 1,
        Seller = 2,
    }
    public class MobileUser : UserRequisites
    {
        /// <summary>
        /// Уникальный номер устройства
        /// </summary>
        [JsonProperty("MDK")]
        public int MobileDeviceKey { get; set; } = 0;

        /// <summary>
        /// Статус пользователя
        /// </summary>
        [JsonProperty("US")]
        public UserStates UserState { get; set; } = UserStates.Ungeristered;

        /// <summary>
        /// Дата регистрации
        /// </summary>
        [JsonProperty("RD")]
        public DateTime RegisterDateTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Дата последнего действия
        /// </summary>
        [JsonProperty("LA")]
        public DateTime LastActionDateTime { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Роль пользователя
        /// </summary>
        [JsonProperty("UT")]
        public UserTypes UserType { get; set; } = UserTypes.Client;

        /// <summary>
        /// Хэш
        /// </summary>
        [JsonProperty("HS")]
        public string Hash { get; set; } = string.Empty;


        /// <summary>
        /// Ключи привязанных карт
        /// </summary>
        [JsonProperty("CK")]
        public string CardKeys { get; set; } = string.Empty;

        /// <summary>
        /// Графический номер
        /// </summary>
        [JsonProperty("GN")]
        public string GraphicalNumbers { get; set; } = string.Empty;

        /// <summary>
        /// Пароль
        /// </summary>
        [JsonProperty("PAS")]
        public string Password { get; set; } = string.Empty;

        public void Parse(string json)
        {
            var deserialized = JsonConvert.DeserializeObject<MobileUser>(json);
            MobileUserKey = deserialized.MobileUserKey;
            Hash = deserialized.Hash;
            Email = deserialized.Email;
            MobileDeviceKey = deserialized.MobileDeviceKey;
            UserNickName = deserialized.UserNickName;
            PhoneNumber = deserialized.PhoneNumber;
            UserState = deserialized.UserState;
            RegisterDateTime = deserialized.RegisterDateTime;
            LastActionDateTime = deserialized.LastActionDateTime;
            UserType = deserialized.UserType;
            CardKeys = deserialized.CardKeys;
            GraphicalNumbers = deserialized.GraphicalNumbers;
            ResultState = deserialized.ResultState;
            ResultInfo = deserialized.ResultInfo;
            Password = deserialized.Password;
        }
    }
}
