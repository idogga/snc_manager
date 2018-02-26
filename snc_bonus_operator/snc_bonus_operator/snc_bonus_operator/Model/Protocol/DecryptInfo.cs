using Newtonsoft.Json;

namespace snc_bonus_operator.Protocol
{
    public class DecryptInfo : RequestResult
    {
        /// <summary>
        /// Код устройства продавца
        /// </summary>
        [JsonProperty("MD")]
        public int MobileDeviceKey { get; set; } = 0;

        /// <summary>
        /// Код устройства клиента
        /// </summary>
        [JsonProperty("D")]
        public int DeviceKey { get; set; } = 0;

        /// <summary>
        /// Строка для дешифрования или ответ
        /// </summary>
        [JsonProperty("I")]
        public string Info { get; set; } = "";

        /// <summary>
        /// Цена без учета скидок
        /// </summary>
        [JsonProperty("TP")]
        public double TotalPrice { get; set; } = 0;
    }
}
