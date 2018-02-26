using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    /// <summary>
    /// Регистрационные данные
    /// </summary>
    public class RegisterInfo
    {
        public enum RegisterState
        {
            /// <summary>
            /// Успешное действие
            /// </summary>
            Success,
            /// <summary>
            /// Ошибка или некорректные данные для регистрации
            /// </summary>
            Fail,
            /// <summary>
            /// Не найден пользователь по указанному ключу (логину)
            /// </summary>
            UserNotFound,
            /// <summary>
            /// Некорректный или отсутствующий в базе адрес почты
            /// </summary>
            WrongEmail,
            /// <summary>
            /// Некорректный или отсутствующий в базе номер карты
            /// </summary>
            WrongCardNumber,
            /// <summary>
            /// Несовпадение пароля (хэша) пользователя
            /// </summary>
            WrongHash,
            /// <summary>
            /// Запрещенное действие (например по состоянию пользователя)
            /// </summary>
            Restricted,
            /// <summary>
            /// Успешное действие, но кодовое слово еще не задано
            /// </summary>
            SuccessNoKeyWord,
            /// <summary>
            /// Успешное действие, но карт у пользователя еще нет
            /// </summary>
            SuccessNoCards,
            /// <summary>
            /// Успешное действие, но ни кодовое слово еще не задано, ни карт еще нет
            /// </summary>
            SuccessNoKeyWordNoCards,
        }

        public enum AppKeys
        {
            /// <summary>
            /// Мобильное приложение Заправка
            /// </summary>
            Fuel = 0,
            /// <summary>
            /// Мобильное приложение Бонус для клиентов
            /// </summary>
            BonusClient = 1,
            /// <summary>
            /// Мобильное приложение Бонус для операторов и администраторов ТО
            /// </summary>
            BonusSeller = 2,
        }

        private Dictionary<RegisterState, string> RegisterStateTranslate = new Dictionary<RegisterState, string>();

        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        [JsonProperty("MUK")]
        public int MobileUserKey { get; set; } = 0;
        /// <summary>
        /// Уникальных код клиента (хэш)
        /// </summary>
        [JsonProperty("HS")]
        public string Hash { get; set; } = "";
        /// <summary>
        /// Запрос регистрации на почту
        /// </summary>
        [JsonProperty("EM")]
        public string Email { get; set; } = "";
        /// <summary>
        /// Уникальный номер устройства для привязки при регистрации пользователя
        /// </summary>
        [JsonProperty("IM")]
        public string Imei { get; set; } = "";
        /// <summary>
        /// Идентификатор клиента
        /// </summary>
        [JsonProperty("DK")]
        public int DeviceKey { get; set; } = 0;
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
        /// Номер карты
        /// </summary>
        [JsonProperty("CN")]
        public string CardNumber { get; set; } = "";
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
        /// Состояние регистрации
        /// </summary>
        [JsonProperty("ST")]
        public RegisterState State { get; set; }
        /// <summary>
        /// Уникальный идентификатор устройства для рассылки оповещений
        /// </summary>
        [JsonProperty("NF")]
        public string NotificationToken { get; set; } = "";

        /// <summary>
        /// Название топика для рассылки оповещений для устройств пользователя
        /// </summary>
        [JsonProperty("NT")]
        public string NotificationTopic { get; set; } = "";
        /// <summary>
        /// Телефоный номер пользователя
        /// </summary>
        [JsonProperty("PN")]
        public string PhoneNumber { get; set; } = string.Empty;
        /// <summary>
        /// Тип мобильного приложения (Заправка, Бонусы и т.д.)
        /// </summary>
        [JsonProperty("AK")]
        public AppKeys AppKey { get; set; } = AppKeys.Fuel;


        public RegisterInfo()
        {
            RegisterStateTranslate.Add(RegisterState.WrongHash, "Неверный код");
            RegisterStateTranslate.Add(RegisterState.WrongEmail, "Неверный эмейл");
            RegisterStateTranslate.Add(RegisterState.WrongCardNumber, "Неверный номер карты");
            RegisterStateTranslate.Add(RegisterState.UserNotFound, "Пользователь не найден");
            RegisterStateTranslate.Add(RegisterState.Success, "Успех");
            RegisterStateTranslate.Add(RegisterState.Restricted, "Так нельзя делать");
            RegisterStateTranslate.Add(RegisterState.Fail, "Некорректные данные для регистрации");
        }

        public void ParseJson(string res)
        {
            try
            {
                var deserialized = JsonConvert.DeserializeObject<RegisterInfo>(res);
                MobileUserKey = deserialized.MobileUserKey;
                DeviceKey = deserialized.DeviceKey;
                Hash = deserialized.Hash;
                Email = deserialized.Email;
                Imei = deserialized.Imei;
                CardNumber = deserialized.CardNumber;
                PublicKey = deserialized.PublicKey;
                PrivateKey = deserialized.PrivateKey;
                State = deserialized.State;
                Port = deserialized.Port;
                IP = deserialized.IP;
                NotificationToken = deserialized.NotificationToken;
                NotificationTopic = deserialized.NotificationTopic;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        public string TranslateRegisterState(RegisterState state)
        {
            return RegisterStateTranslate[state];
        }
    }
}
