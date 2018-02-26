using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class StuffModel : RequestResult
    {
        /// <summary>
        /// Номер устройства текущего оператора
        /// </summary>
        [JsonProperty("MDK")]
        public int MobileDevicKey { get; set; } = 0;

        /// <summary>
        /// Список сотрудников одной организации
        /// </summary>
        [JsonProperty("CL")]
        public List<Colegue> ColegueList { get; set; } = new List<Colegue>();
    }

    public class Colegue
    {
        /// <summary>
        /// Номер менеджера
        /// </summary>
        [JsonProperty("MK")]
        public int MobileManagerKey { get; set; } = 0;

        /// <summary>
        /// Номер менеджера
        /// </summary>
        [JsonProperty("E")]
        public string Email { get; set; } = "";

        /// <summary>
        /// имя менеджера
        /// </summary>
        [JsonProperty("N")]
        public string Name { get; set; } = "";

        /// <summary>
        /// Название магазина
        /// </summary>
        [JsonProperty("SN")]
        public string ShopName { get; set; } = "";

        /// <summary>
        /// Должность
        /// </summary>
        [JsonProperty("P")]
        public string Position { get; set; } = "";

        /// <summary>
        /// Номер магазина
        /// </summary>
        [JsonProperty("SK")]
        public int ShopKey { get; set; } = 0;

        /// <summary>
        /// Кто привел менеджера (ключ)
        /// </summary>
        [JsonProperty("AK")]
        public int AdminUserKey { get; set; } = 0;

        /// <summary>
        /// Кто привел менеджера (имя)
        /// </summary>
        [JsonProperty("AN")]
        public string AdminName { get; set; } = string.Empty;

        /// <summary>
        /// Состояние коллеги
        /// </summary>
        [JsonProperty("US")]
        public UserStates UserState { get; set; } = UserStates.Deleted;
    }
}