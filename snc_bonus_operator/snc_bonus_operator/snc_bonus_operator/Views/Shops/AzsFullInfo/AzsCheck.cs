using Newtonsoft.Json;

namespace snc_bonus_operator
{
    public class AzsCheck
    {
        /// <summary>
        /// Уникальный номер эмитента
        /// </summary>
        [JsonProperty("MDK")]
        public int MobileDeviceKey { get; set; } = 0;

        /// <summary>
        /// Название эмитента
        /// </summary>
        [JsonProperty("AZS")]
        public int CodAzs { get; set; } = 0;

        /// <summary>
        /// Номер эмитента
        /// </summary>
        [JsonProperty("IK")]
        public int IssuerKey { get; set; } = 0;
    }
}