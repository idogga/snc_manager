using Newtonsoft.Json;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class AcceptSelling : RequestResult
    {
        [JsonProperty("AC")]
        public bool Accept { get; set; } = false;

        /// <summary>
        /// Имя акцептовавшего
        /// </summary>
        [JsonProperty("ASN")]
        public string AcceptSellerName { get; set; } = "";

        [JsonProperty("TK")]
        public List<int> TransactionKey { get; set; } = new List<int>();
    }
}
