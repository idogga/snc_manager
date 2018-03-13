using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class RequestResult
    {
        public enum Results
        {
            /// <summary>
            /// Провал (общий)
            /// </summary>
            Error = 0,
            /// <summary>
            /// Успех (общий)
            /// </summary>
            Success = 1,
            /// <summary>
            /// Не корректные данные
            /// </summary>
            WrongData = 2,
            /// <summary>
            /// Не удалось внести в базу новую запись пользователя
            /// </summary>
            UserWasNotAdded = 3,
            /// <summary>
            /// Не удалось отправить регистрационные данные (Email, sms)
            /// </summary>
            RegisterDataNotSent = 4,
            /// <summary>
            /// Успех регистарции
            /// </summary>
            RegisterSuccess = 5,

            /// <summary>
            /// Ошибка или некорректные данные для регистрации
            /// </summary>
            RegisterFail = 6,
            /// <summary>
            /// Не найден пользователь по указанному ключу (логину)
            /// </summary>
            RegisterUserNotFound = 7,
            /// <summary>
            /// Некорректный или отсутствующий в базе адрес почты
            /// </summary>
            RegisterWrongEmail = 8,
            /// <summary>
            /// Некорректный или отсутствующий в базе номер карты
            /// </summary>
            RegisterWrongCardNumber = 9,
            /// <summary>
            /// Несовпадение пароля (хэша) пользователя
            /// </summary>
            RegisterWrongHash = 10,
            /// <summary>
            /// Запрещенное действие (например по состоянию пользователя)
            /// </summary>
            RegisterRestricted = 11,

            /// <summary>
            /// Код устройства не найден в базе
            /// </summary>
            WrongDeviceKey = 12,
            /// <summary>
            /// Устройство заблокировано или находится в некорректном состоянии
            /// </summary>
            WrongDeviceState = 13,
            /// <summary>
            /// Пользователь отсутствует в базе (не найден по ключу)
            /// </summary>
            WrongUserKey = 14,
            /// <summary>
            /// Объект уже существует
            /// </summary>
            AlreadyExists = 15,
            /// <summary>
            /// Объект уже существует
            /// </summary>
            UserBlocked = 16,
            /// <summary>
            /// Объект уже существует
            /// </summary>
            UserDeleted = 17,
            /// <summary>
            /// QR-код не является уникальным
            /// </summary>
            QRCodeNotUnique = 18
        }

        /// <summary>
        /// Результат запроса
        /// </summary>
        [JsonProperty("RS")]
        public Results ResultState { get; set; } = Results.Error;

        /// <summary>
        /// Описание состояния или сопутствующие сообщение
        /// </summary>
        [JsonProperty("RF")]
        public string ResultInfo { get; set; } = string.Empty;

        /// <summary>
        /// Словарь с переводом результа запроса
        /// </summary>
        private Dictionary<Results, string> ResultsTranslate = new Dictionary<Results, string>();



        public void ParseJson(string json)
        {
            try
            {
                var deserialized = JsonConvert.DeserializeObject<RequestResult>(json);
                ResultState = deserialized.ResultState;
                ResultInfo = deserialized.ResultInfo;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        public RequestResult()
        {
            ResultsTranslate.Add(Results.Success, "Успешно");
            ResultsTranslate.Add(Results.WrongData, "Некоректные данные");
            ResultsTranslate.Add(Results.Error, "Произошла ошибка");
            ResultsTranslate.Add(Results.UserWasNotAdded, "Не удалось ввести в базу данных нового пользователя");
            ResultsTranslate.Add(Results.RegisterDataNotSent, "Не удалось отправить сообщения на указаный адрес");

            ResultsTranslate.Add(Results.RegisterWrongHash, "Не верный код");
            ResultsTranslate.Add(Results.RegisterWrongEmail, "Пользователь с такйо почтой не найден");
            ResultsTranslate.Add(Results.RegisterWrongCardNumber, "Пользоавтель с такой картой не был найден");
            ResultsTranslate.Add(Results.RegisterUserNotFound, "Не найден пользователь с такими реквизитами");
            ResultsTranslate.Add(Results.RegisterSuccess, "Регистрация прошла успешно");
            ResultsTranslate.Add(Results.RegisterRestricted, "Не удалось выполнить");
            ResultsTranslate.Add(Results.RegisterFail, "Данные были введены некоректно");
            ResultsTranslate.Add(Results.WrongDeviceKey, "Не удается совершить операцию с данного устройства. Обратитесь в поддержку");
            ResultsTranslate.Add(Results.WrongDeviceState, "Данное устройство заблокировано, обратитесь в поддежку.");
            ResultsTranslate.Add(Results.AlreadyExists, "Данное устройство заблокировано, обратитесь в поддежку.");
            ResultsTranslate.Add(Results.WrongUserKey, "Такой пользователь отстутвует в базе.");
            ResultsTranslate.Add(Results.QRCodeNotUnique, "Данный QR-код уже был использован. Попробуйте другой.");
        }

        public string TranslateResult(Results state)
        {
            return ResultsTranslate[state];
        }
    }
}
