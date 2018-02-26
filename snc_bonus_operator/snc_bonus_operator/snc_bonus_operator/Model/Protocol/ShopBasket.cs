using Newtonsoft.Json;
using System;

namespace snc_bonus_operator.Protocol
{
    public class ShopBasket : RequestResult
    {
        /// <summary>
        /// Уникальный номер устройства
        /// </summary>
        [JsonProperty("MDK")]
        public int MobileDeviceKey { get; set; } = 0;
        /// <summary>
        /// Уникальный номер устройства клиента
        /// </summary>
        [JsonProperty("CMDK")]
        public int ClientMobileDeviceKey { get; set; } = 0;

        /// <summary>
        /// Цена без учета скидок и бонусов
        /// </summary>
        [JsonProperty("TP")]
        public double TotalPrice { get; set; } = .0;

        /// <summary>
        /// Цена с учетом скидок и бонусов
        /// </summary>
        [JsonProperty("FP")]
        public double FinalPrice { get; set; } = .0;
        
        /// <summary>
        /// Предоженое колличество бонусов для списания
        /// </summary>
        [JsonProperty("BP")]
        public double BonusProposed { get; set; } = .0;

        /// <summary>
        /// Колличество бонусов для списания
        /// </summary>
        [JsonProperty("BCO")]
        public double BonusCountOut { get; set; } = .0;

        /// <summary>
        /// Колличество бонусов для начисления
        /// </summary>
        [JsonProperty("BCI")]
        public double BonusCountIn { get; set; } = .0;

        /// <summary>
        /// Скидка
        /// </summary>
        [JsonProperty("D")]
        public double Discount { get; set; } = .0;

        /// <summary>
        /// Количество бонусов для списания
        /// </summary>
        [JsonProperty("T")]
        public DateTime TimeGeneration { get; set; } = DateTime.Now;

        /// <summary>
        /// Количество бонусов для списания
        /// </summary>
        [JsonProperty("UPN")]
        public string UserProgrammName { get; set; } = "";


        public int COD_A { get; set; } = 0;
        public int COD_OWN { get; set; } = 0;
        public int COD_O { get; set; } = 0;

        [JsonProperty("GN")]
        public string GraphicalNumber { get; set; } = string.Empty;
        
        /// <summary>
        /// Списывание или начисление
        /// </summary>
        [JsonProperty("BD")]
        public bool IsBonusDeletion { get; set; } = false;

        [JsonProperty("TC")]
        public DateTime TimeComplete { get; set; } = DateTime.MinValue;


        /// <summary>
        /// Строка для дешифрования или ответ
        /// </summary>
        [JsonProperty("I")]
        public string Info { get; set; } = "";
    }
}
