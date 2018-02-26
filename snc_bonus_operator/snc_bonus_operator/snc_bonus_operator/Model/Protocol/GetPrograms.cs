using Newtonsoft.Json;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class GetPrograms : RequestResult
    {
        [JsonProperty("PS")]
        public List<ProgramInfo> Programs { get; set; } = new List<ProgramInfo>();
        [JsonProperty("AK")]
        public RegisterInfo.AppKeys AppKey { get; set; } = RegisterInfo.AppKeys.Fuel;
    }

    public class ProgramInfo
    {
        [JsonProperty("CDA")]
        public int COD_A { get; set; } = 0;

        [JsonProperty("NA")]
        public string Name { get; set; } = "";

        [JsonProperty("DSC")]
        public string Description { get; set; } = "";
    }
}
