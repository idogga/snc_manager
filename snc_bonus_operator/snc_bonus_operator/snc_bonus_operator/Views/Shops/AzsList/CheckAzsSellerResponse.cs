using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace snc_bonus_operator
{
    /// <summary>
    /// Ответ со статусами всех пистолетов
    /// </summary>
    public class CheckAzsSellerResponse
    {
        /// <summary>
        /// Список конфигурации азс
        /// </summary>
        [JsonProperty("AzsListConfig")]
        public List<AzsConfigItem> AzsListConfig { get; set; } = new List<AzsConfigItem>();
    }
}
