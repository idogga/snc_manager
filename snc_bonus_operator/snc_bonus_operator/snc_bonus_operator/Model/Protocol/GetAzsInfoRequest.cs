using Newtonsoft.Json;

namespace sncmanager.Protocol
{
    public class GetAzsInfoRequest
    {
        public GetAzsInfoRequest(int shopKey, int mobileUserKey)
        {
            CodAzs = shopKey;
            MobileUserKey = mobileUserKey;
        }

        /// <summary>
        /// Код АЗС
        /// </summary>
        [JsonProperty("CODAZS")]
        public int CodAzs { get; internal set; }

        /// <summary>
        /// Код устройства пользователя
        /// </summary>
        [JsonProperty("MDK")]
        public int MobileUserKey { get; internal set; }
    }
}
