using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace snc_bonus_operator.Protocol
{
    /// <summary>
    /// Состояния карты
    /// </summary>
    public enum CardStateEnum
    {
        /// <summary>
        /// 1 - NEW
        /// </summary>
        NEW = 1,
        /// <summary>
        /// 2 - CHANGED
        /// </summary>
        CHANGED = 2,
        /// <summary>
        /// 3 - WRITTEN (не требует никаких действия, работает)
        /// </summary>
        WRITTEN = 3,
        /// <summary>
        /// 4 - READ
        /// </summary>
        READ = 4,
        /// <summary>
        /// 5 - ERROR
        /// </summary>
        ERROR = 5,
        /// <summary>
        /// 6 - RemoteUpdate - на карту имеется неисполненный элемент корректировки/ пополнения
        /// </summary>
        RemoteUpdate = 6,
        /// <summary>
        /// 7 - CLEARED
        /// </summary>
        CLEARED = 7,
        /// <summary>
        /// 8 - Требуется очистка
        /// </summary>
        ClearRequire = 8,
        /// <summary>
        /// 9 - Остановлена по состоянию счета
        /// </summary>
        STOP = 9,
        /// <summary>
        /// 10 - Блокирована (утеряна)
        /// </summary>
        BLOCK = 10,
        /// <summary>
        /// 11 - ReturnRemoteUpdate - на карту имеется неисполненный просроченный элемент корректировки/ пополнения
        /// </summary>
        ReturnRemoteUpdate = 11,
        /// <summary>
        /// 12 - Требуется запись для изменения PIN
        /// </summary>
        PINChangeRequire = 12,
        /// <summary>
        /// 13 - Сдана
        /// </summary>
        TurmCard = 13,
        /// <summary>
        /// 14 - Начало записи (передача данных на ридер) - Если этот статус остался, значит запись завершилась некорректно (Отображать пользователю как: Сбой в момент записи карты)
        /// </summary>
        BeginWrite = 14,
        /// <summary>
        /// Карта удерживается для On-Line обслуживания
        /// </summary>
        Hold = 15
    }

    public enum CardTypeEnum
    {
        /// <summary>
        /// 0 - Магнитная карта
        /// </summary>
        MC = 0,
        /// <summary>
        /// 1 - Таблетка IButton
        /// </summary>
        EC = 1,
        /// <summary>
        /// 2 - Радиокарта Mifare
        /// </summary>
        RC = 2,
        /// <summary>
        /// 3 - Смарт-карта
        /// </summary>
        SC = 3,
        /// <summary>
        /// 4 - Штрих-кодовая карта
        /// </summary>
        CC = 4,
        /// <summary>
        /// 5 - Карта PetrolPlus
        /// </summary>
        PP = 5,
        /// <summary>
        /// 6 - Ведомость
        /// </summary>
        BI = 6,
        /// <summary>
        /// 7 - Талоны
        /// </summary>
        CI = 7
    }

    /// <summary>
    /// Тип статуса (группировка статусов) по топливным схемам
    /// </summary>
    public enum FuelStatusType
    {
        /// <summary>
        /// Неизвестный (еще не используется в системе)
        /// </summary>
        UnknownStatus = 0,
        /// <summary>
        /// Партии топлива
        /// </summary>
        FuelParcel = 1,
        /// <summary>
        /// Дебит стоимостной (денежные кошельки)
        /// </summary>
        DebitCost = 2,
        /// <summary>
        /// Дебит литровый (литровые кошельки)
        /// </summary>
        DebitVolume = 3,
        /// <summary>
        /// Кредит стоимостной (лимитные денежные)
        /// </summary>
        CreditCost = 4,
        /// <summary>
        /// Кредит литровый (лимитные литровые)
        /// </summary>
        CreditVolume = 5,
    }

    public class UserCards : RequestResult
    {
        [JsonProperty("CA")]
        public List<UserCard> Cards = new List<UserCard>();
        [JsonProperty("MUK")]
        public int MobileUserKey { get; set; } = 0;
        [JsonProperty("MDK")]
        public int MobileDeviceKey { get; set; } = 0;
    }

    public class UserCard : ProgramInfo
    {
        [JsonProperty("CK")]
        public int CardKey { get; set; } = 0;
        [JsonProperty("GN")]
        public string GraphicalNumber { get; set; } = string.Empty;
        [JsonProperty("CS")]
        public CardStateEnum CardState { get; set; } = CardStateEnum.NEW;
        [JsonProperty("CT")]
        public CardTypeEnum CardType { get; set; } = CardTypeEnum.RC;
        [JsonProperty("SCD")]
        public DateTime StateChangeDatetime { get; set; } = DateTime.MinValue;
        /// <summary>
        /// Псевдоним держателя карты (название карты в мобильном приложении)
        /// </summary>
        [JsonProperty("DC")]
        public string DescriptionCard { get; set; } = "";
        /// <summary>
        /// Перевод типа карты
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string GetCardTypeTranslation(CardTypeEnum type)
        {
            string result = "";
            switch (type)
            {
                case (CardTypeEnum.MC):
                    result = "Магнитная карта";
                    break;
                case (CardTypeEnum.EC):
                    result = "Таблетка";
                    break;
                case (CardTypeEnum.RC):
                    result = "Радиокарта";
                    break;
                case (CardTypeEnum.SC):
                    result = "Смарт-карта";
                    break;
                case (CardTypeEnum.CC):
                    result = "Штрихкодовая";
                    break;
                case (CardTypeEnum.PP):
                    result = "Карта PetrolPlus";
                    break;
                case (CardTypeEnum.BI):
                    result = "Ведомость";
                    break;
                case (CardTypeEnum.CI):
                    result = "Талон";
                    break;
            }
            return result;
        }

        public string GetCardStateTranslation(CardStateEnum state)
        {
            string result = "";
            switch (state)
            {
                case (CardStateEnum.WRITTEN):
                case (CardStateEnum.READ):
                case (CardStateEnum.RemoteUpdate):
                case (CardStateEnum.ReturnRemoteUpdate):
                case (CardStateEnum.Hold):
                case (CardStateEnum.ERROR):
                case (CardStateEnum.CLEARED):
                case (CardStateEnum.BeginWrite):
                    result = "В работе";
                    break;
                case (CardStateEnum.STOP):
                case (CardStateEnum.BLOCK):
                    result = "Заблокирована";
                    break;

                case (CardStateEnum.TurmCard):
                    result = "Сдана";
                    break;
                case (CardStateEnum.NEW):
                case (CardStateEnum.CHANGED):
                case (CardStateEnum.ClearRequire):
                case (CardStateEnum.PINChangeRequire):
                    result = "Требуется запись";
                    break;
                default:
                     result = "Не верный тип карты";
                    break;
                    //throw new Exception("Не понятный номер состояния");
            }
            return result;
        }

        public string GetWrightGraphicalNumber()
        {
            string result = "";
            long tempNumber = long.Parse(GraphicalNumber);
            var format = "0000 0000 0000 0000";
            result = tempNumber.ToString(format);
            return result;
        }

        public string GetWrightName()
        {
            string result = "";
            if (DescriptionCard.Length == 0)
            {
                result = GetWrightGraphicalNumber();
            }
            else
            {
                result = DescriptionCard;
            }
            return result;
        }
    }
}
