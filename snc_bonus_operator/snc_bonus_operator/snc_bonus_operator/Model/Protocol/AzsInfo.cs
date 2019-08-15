using Newtonsoft.Json;
using System;

namespace snc_bonus_operator
{
    public class AzsInfo : AzsTableHashItem
    {
        /// <summary>
        /// Уникальный код для базы
        /// </summary>
        [JsonProperty("AK")]
        public int AzsKey { get; set; } = 0;
        
        /// <summary>
        /// Номер эмитента
        /// </summary>
        [JsonProperty("IK")]
        public int IssuerKey { get; set; } = 0;
        /// <summary>
        /// Широта
        /// </summary>
        [JsonProperty("LA")]
        public double Latitude { get; set; } = 0;
        /// <summary>
        /// Долгота
        /// </summary>
        [JsonProperty("LO")]
        public double Longitude { get; set; } = 0;
        /// <summary>
        /// Название
        /// </summary>
        [JsonProperty("TT")]
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Примечание
        /// </summary>
        [JsonProperty("DC")]
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Полное название
        /// </summary>
        [JsonProperty("FN")]
        public string FullName { get; set; } = string.Empty;
        /// <summary>
        /// Короткое название
        /// </summary>
        [JsonProperty("SN")]
        public string ShortName { get; set; } = string.Empty;
        /// <summary>
        /// Физический адрес
        /// </summary>
        [JsonProperty("AD")]
        public string Address { get; set; } = string.Empty;

        /// <summary>
        /// Телефон
        /// </summary>
        [JsonProperty("PH")]
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// Способы оплаты на АЗС
        /// </summary>
        [JsonProperty("PT")]
        public ShopSettingsEnum ShopSettings { get; set; } =  ShopSettingsEnum.None;

        /// <summary>
        /// Дата и время последнего обновления
        /// </summary>
        [JsonProperty("LU")]
        public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.MinValue;

        /// <summary>
        /// Дата и время последнего обновления
        /// </summary>
        [JsonProperty("ST")]
        public int ShopType { get; set; } = 0; // 0 - азс

        /// <summary>
        /// Логотип
        /// </summary>
        [JsonProperty("LG")]
        public string LogoPath { get; set; } = "";

        /// <summary>
        /// Режим отображения колонок и пистолетов АЗС
        /// </summary>
        [JsonProperty("NVT")]
        public AzsNozzleViewTypes NozzlesViewType { get; set; } = AzsNozzleViewTypes.ShowColumnsNozzlesAndFuelTypes;

        /// <summary>
        /// Режим проверки пистолета перед заявкой
        /// </summary>
        [JsonProperty("NCM")]
        public NozzleConfirmModes NozzleConfirmMode { get; set; } = NozzleConfirmModes.OrderConfirmByNozzle;

        /// <summary>
        /// Выбрана как любимая
        /// </summary>
        public bool IsFavourite = false;

        /// <summary>
        /// Коэффицент сортировки
        /// </summary>
        [JsonIgnore]
        public double Koeff { get; set; }

        /// <summary>
        /// Показывать ли станцию
        /// </summary>
        public bool IsVisible { get; set; }
        public string Services
        {
            get
            {
                var result = "";
                if (IsVariant(ShopSettings, ShopSettingsEnum.None))
                    return "Нет сервисов";
                if (IsVariant(ShopSettings, ShopSettingsEnum.BankCard))
                    result += "Расчет банковскими картами" + Environment.NewLine;
                if (IsVariant(ShopSettings, ShopSettingsEnum.CarParkService))
                    result += "Автостоянка" + Environment.NewLine;
                if (IsVariant(ShopSettings, ShopSettingsEnum.CarWashService))
                    result += "Автомойка" + Environment.NewLine;
                if (IsVariant(ShopSettings, ShopSettingsEnum.Cash))
                    result += "Расчет наличными" + Environment.NewLine;
                if (IsVariant(ShopSettings, ShopSettingsEnum.FoodService))
                    result += "Покупка еды" + Environment.NewLine;
                if (IsVariant(ShopSettings, ShopSettingsEnum.FuelSellingService))
                    result += "Заправка бензином" + Environment.NewLine;
                if (IsVariant(ShopSettings, ShopSettingsEnum.GasSellingService))
                    result += "Заправка газом" + Environment.NewLine;
                if (IsVariant(ShopSettings, ShopSettingsEnum.GoodSellingService))
                    result += "Продажа товаров" + Environment.NewLine;
                if (IsVariant(ShopSettings, ShopSettingsEnum.HotelService))
                    result += "Гостиница" + Environment.NewLine;
                if (IsVariant(ShopSettings, ShopSettingsEnum.MobileApp))
                    result += "Расчет через мобильное приложение" + Environment.NewLine;
                result.Remove(result.Length - 2);
                return result;
            }
        }
        private bool IsVariant(ShopSettingsEnum value, ShopSettingsEnum bankCard)
        {
            return (value & bankCard) == (int)ShopSettingsEnum.None;
        }
    }
}
