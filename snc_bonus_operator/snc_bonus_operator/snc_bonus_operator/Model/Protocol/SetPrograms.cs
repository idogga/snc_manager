using Newtonsoft.Json;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class SetPrograms : RequestResult
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
        /// Номера выбранных программ
        /// </summary>
        [JsonProperty("PS")]
        public List<int> Programs { get; set; } = new List<int>();
    }
}
