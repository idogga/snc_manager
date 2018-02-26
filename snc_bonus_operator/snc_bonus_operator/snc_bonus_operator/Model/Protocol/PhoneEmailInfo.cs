using Newtonsoft.Json;

namespace snc_bonus_operator.Protocol
{
    public class PhoneEmailInfo : RequestResult
    {
        /// <summary>
        /// Уникальный номер устройства
        /// </summary>
        [JsonProperty("MDK")]
        public int MobileDeviceKey { get; set; } = 0;

        /// <summary>
        /// Адрес электронной почты
        /// </summary>
        [JsonProperty("EM")]
        public string Email { get; set; } = "";

        /// <summary>
        /// Номер телефона
        /// </summary>
        [JsonProperty("PH")]
        public string Phone { get; set; } = "";

        /// <summary>
        /// Уникальных код клиента (хэш)
        /// </summary>
        [JsonProperty("HS")]
        public string Hash { get; set; } = "";
    }
}
