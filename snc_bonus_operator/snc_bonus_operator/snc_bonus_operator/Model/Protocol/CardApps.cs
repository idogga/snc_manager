using Newtonsoft.Json;
using System;
using System.Collections.Generic;


namespace snc_bonus_operator.Protocol
{
    public class CardApps : RequestResult
    {
        /// <summary>
        /// Номер карты в базе
        /// </summary>
        [JsonProperty("CK")]
        public int CardKey { get; set; } = 0;
        /// <summary>
        /// Список приложений на карте
        /// </summary>
        [JsonProperty("AP")]
        public List<CardApplication> Applications { get; set; } = new List<CardApplication>();
        /// <summary>
        /// Язык
        /// </summary>
        [JsonProperty("LG")]
        public string Language { get; set; }
    }

    public class CardApplication
    {
        /// <summary>
        /// Ключ
        /// </summary>
        [JsonProperty("AK")]
        public int AppKey { get; set; } = 0;

        /// <summary>
        /// Состояние приложения
        /// </summary>
        [JsonProperty("ST")]
        public int State { get; set; } = 0;
        /// <summary>
        /// Цена топлива
        /// </summary>
        [JsonProperty("PC")]
        public double ParcelCost { get; set; } = 0;
        /// <summary>
        /// Ограничение кошелька
        /// </summary>
        [JsonProperty("PL")]
        public double PurseLimit { get; set; } = 0;
        /// <summary>
        /// Остаток кошелька
        /// </summary>
        [JsonProperty("PE")]
        public double PurseExpense { get; set; } = 0;
        /// <summary>
        /// Доп ограничение
        /// </summary>
        [JsonProperty("DL")]
        public double DayLimit { get; set; } = 0;
        /// <summary>
        /// Доп остаток
        /// </summary>
        [JsonProperty("DE")]
        public double DayExpense { get; set; } = 0;
        /// <summary>
        /// Перечисление типов топлив
        /// </summary>
        [JsonProperty("FN")]
        public string FF_Name { get; set; } = "";
        /// <summary>
        /// Период действия
        /// </summary>
        [JsonProperty("PD")]
        public DateTime Period { get; set; } = DateTime.MinValue;
        /// <summary>
        /// Код набора CollectionKey и код товара FF_Cod
        /// </summary>
        [JsonProperty("FL")]
        public List<KeyValuePair<int, int>> FuelList { get; set; } = new List<KeyValuePair<int, int>>();
        /// <summary>
        /// Тип (статус) приложения
        /// </summary>
        [JsonProperty("SK")]
        public int StatusKey { get; set; } = 0;
        /// <summary>
        /// Значение типа (статуса) приложения
        /// </summary>
        [JsonProperty("SS")]
        public string StatusString { get; set; } = "";
    }
}
