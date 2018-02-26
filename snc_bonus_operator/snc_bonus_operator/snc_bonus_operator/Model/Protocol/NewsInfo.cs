using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class NewsInfo
    {
        /// <summary>
        /// Количество новостей для загрузки
        /// </summary>
        [JsonProperty("AM")]
        public int Amount { get; set; } = 0;
        /// <summary>
        /// Количество загруженных новостей (сдвиг)
        /// </summary>
        [JsonProperty("OF")]
        public int Offset { get; set; } = 0;
        /// <summary>
        /// Список новостей
        /// </summary>
        [JsonProperty("NW")]
        public List<NewsDetail> News { get; set; } = new List<NewsDetail>();
    }

    public class NewsDetail
    {
        [JsonProperty("TT")]
        public string TitleText { get; set; } = "";
        [JsonProperty("DT")]
        public string DescriptionText { get; set; } = "";
        [JsonProperty("LN")]
        public string Link { get; set; } = "";
        [JsonProperty("IL")]
        public string ImageLink { get; set; } = "";
        [JsonProperty("CA")]
        public string Category { get; set; } = "";
        [JsonProperty("AU")]
        public string Author { get; set; } = "";
        [JsonProperty("ND")]
        public DateTime NewsDateTime { get; set; } = DateTime.MinValue;
    }
}
