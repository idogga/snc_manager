using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class SellerTransactionInfo : RequestResult
    {
        public enum SELLER_STATUS_ENUM
        {
            Under_Consideration = 0,
            Accepted = 1,
            Not_Accepted = 2,
            Not_NeedAccept = 3
        }

        /// <summary>
        /// Дата начала выборки
        /// </summary>
        [JsonProperty("FM")]
        public DateTime From { get; set; } = DateTime.MinValue;

        /// <summary>
        /// Дата окончания выборки
        /// </summary>
        [JsonProperty("TO")]
        public DateTime To { get; set; } = DateTime.MaxValue;

        /// <summary>
        /// Номер мобильного устройства
        /// </summary>
        [JsonProperty("DK")]
        public int DeviceKey { get; set; } = 0;

        /// <summary>
        /// Количество транзакций для загрузки
        /// </summary>
        [JsonProperty("AM")]
        public int Amount { get; set; } = 10;
        /// <summary>
        /// Количество загруженных транзакций (сдвиг)
        /// </summary>
        [JsonProperty("OF")]
        public int Offset { get; set; } = 0;

        /// <summary>
        /// Список статусов транзакций
        /// </summary>
        [JsonProperty("TSS")]
        public List<SELLER_STATUS_ENUM> TransactionStatuses = new List<SELLER_STATUS_ENUM>();

        /// <summary>
        /// Список продавцов
        /// </summary>
        [JsonProperty("SL")]
        public List<int> SellerList = new List<int>();

        /// <summary>
        /// Список транзакций
        /// </summary>
        [JsonProperty("TR")]
        public List<SellerTransaction> Transactions = new List<SellerTransaction>();
    }

    public class SellerTransaction
    {
        [JsonProperty("TK")]
        public int TransactionKey { get; set; } = 0;

        [JsonProperty("SK")]
        public int ShopKey { get; set; } = 0;

        [JsonProperty("SPN")]
        public string ShopName { get; set; } = string.Empty;

        [JsonProperty("SMUK")]
        public int SellerMobileUserKey { get; set; } = 0;

        [JsonProperty("SN")]
        public string SellerName { get; set; } = "";

        [JsonProperty("BI")]
        public double BonusIn { get; set; } = 0;

        [JsonProperty("PC")]
        public double PersonCost { get; set; } = 0;

        [JsonProperty("CDT")]
        public DateTime CompleteDatetime { get; set; } = DateTime.MinValue;

        [JsonProperty("GN")]
        public string GraphicalNumber { get; set; } = "";

        [JsonProperty("STR")]
        public int StatusTransaction { get; set; } = 0;

        [JsonProperty("BO")]
        public double BonusOut { get; set; } = 0;

        /// <summary>
        /// Скидка в валюте
        /// </summary>
        [JsonProperty("DI")]
        public double Discount { get; set; } = 0;

        [JsonProperty("SBC")]
        public double ShopBaseCost { get; set; } = 0;

        [JsonProperty("UPN")]
        public string UserProgrammName { get; set; } = string.Empty;

        [JsonProperty("ASN")]
        public string AcceptedSellerName { get; set; } = "";

        [JsonProperty("AD")]
        public string AcceptedDate { get; set; } = "";
    }
}
