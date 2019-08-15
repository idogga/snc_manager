using Newtonsoft.Json;
using System.Collections.Generic;

namespace snc_bonus_operator
{
    /// <summary>
    /// Запрос о текущем состоянии колонок
    /// </summary>
    public class CheckAzsSellerRequest
    {
        [JsonProperty("Azses")]
        public List<int> AzsList { get; set; } = new List<int>();
        /// <summary>
        /// Уникальный номер устройства
        /// </summary>
        [JsonProperty("MDK")]
        public int MobileDeviceKey { get; set; } = 0;
    }
}
