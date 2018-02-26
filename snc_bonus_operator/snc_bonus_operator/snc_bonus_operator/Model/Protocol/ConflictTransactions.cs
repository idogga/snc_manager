using Newtonsoft.Json;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class ConflictTransactions : RequestResult
    {
        /// <summary>
        /// Коды странзакций
        /// </summary>
        [JsonProperty("TK")]
        public List<SellerTransaction> TransactionKeys { get; set; } = new List<SellerTransaction>();
    }
}
