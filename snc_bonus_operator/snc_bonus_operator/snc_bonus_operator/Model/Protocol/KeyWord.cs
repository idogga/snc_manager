using Newtonsoft.Json;

namespace snc_bonus_operator.Protocol
{
    public class KeyWordInfo : RequestResult
    {
        /// <summary>
        /// Уникальный номер устройства
        /// </summary>
        [JsonProperty("MDK")]
        public int MobileDeviceKey { get; set; } = 0;
        /// <summary>
        /// Уникальный номер пользователя
        /// </summary>
        [JsonProperty("MUK")]
        public int MobileUserKey { get; set; } = 0;
        /// <summary>
        /// Кодовое слово
        /// </summary>
        [JsonProperty("KW")]
        public string KeyWord { get; set; } = "";
    }
}
