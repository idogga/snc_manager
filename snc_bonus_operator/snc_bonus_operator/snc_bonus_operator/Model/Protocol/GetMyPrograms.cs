using Newtonsoft.Json;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class GetMyPrograms : RequestResult
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

        [JsonProperty("PS")]
        public List<UserCard> Programs { get; set; } = new List<UserCard>();
    }
}
