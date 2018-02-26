using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class AzsInfo
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
        /// Номер АЗС в сети эмитента
        /// </summary>
        [JsonProperty("AI")]
        public int AzsId { get; set; } = 0;
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
        /// Способы оплаты покупки на АЗС (наличка,карта,мобильное приложение)
        /// </summary>
        [JsonProperty("PS")]
        public string PaymentServices { get; set; } = string.Empty;
        /// <summary>
        /// Перечень сервисов и их описаний
        /// </summary>
        [JsonProperty("SS")]
        public Dictionary<string, string> Services = new Dictionary<string, string>();
        /// <summary>
        /// Дата и время последнего обновления
        /// </summary>
        [JsonProperty("LU")]
        public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.MinValue;
        ///// <summary>
        ///// Доступные колонки на АЗС
        ///// </summary>
        //[JsonProperty("CS")]
        //public IList<int> Columns { get; set; } = new List<int>();
        ///// <summary>
        ///// Типы топлив
        ///// </summary>
        //[JsonProperty("FT")]
        //public IList<string> FuelTypes { get; set; } = new List<string>();
    }
}
