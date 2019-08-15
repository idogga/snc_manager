using Newtonsoft.Json;

namespace snc_bonus_operator
{
    /// <summary>
    /// запись в таблице хэшей с азс
    /// </summary>
    public class AzsTableHashItem
    {

        /// <summary>
        /// Номер АЗС в сети эмитента
        /// </summary>
        [JsonProperty("AI")]
        public int AzsId { get; set; } = 0;

        /// <summary>
        /// Хэш записи азс
        /// </summary>
        [JsonProperty("Hash")]
        public string Hash { get; set; } = string.Empty;

        public override string ToString()
        {
            return string.Format("\tКЛЮЧ [{0}] ХЭШ [{1}]", AzsId, Hash);
        }
    }
}
