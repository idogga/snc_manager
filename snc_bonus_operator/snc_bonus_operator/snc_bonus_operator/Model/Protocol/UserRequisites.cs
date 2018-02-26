using Newtonsoft.Json;
using System;

namespace snc_bonus_operator.Protocol
{
    public class UserRequisites : RequestResult
    {
        /// <summary>
        /// Уникальный номер пользователя
        /// </summary>
        [JsonProperty("MUK")]
        public int MobileUserKey { get; set; } = 0;
        /// <summary>
        /// Запрос регистрации на почту
        /// </summary>
        [JsonProperty("EM")]
        public string Email { get; set; } = "";
        /// <summary>
        /// Телефоный номер пользователя
        /// </summary>
        [JsonProperty("PN")]
        public string PhoneNumber { get; set; } = string.Empty;
        /// <summary>
        /// Дата рождения
        /// </summary>
        [JsonProperty("BD")]
        public DateTime BirthDate { get; set; } = DateTime.MinValue;
        /// <summary>
        /// Имя пользователя
        /// </summary>
        [JsonProperty("UN")]
        public string UserNickName { get; set; } = string.Empty;
        /// <summary>
        /// Имя из ФИО
        /// </summary>
        [JsonProperty("NA")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// Фамилия из ФИО
        /// </summary>
        [JsonProperty("SN")]
        public string Surname { get; set; } = string.Empty;
        /// <summary>
        /// Отчество из ФИО
        /// </summary>
        [JsonProperty("MN")]
        public string MiddleName { get; set; } = string.Empty;
        /// <summary>
        /// Пол (0 - мужчина, 1 - девушка
        /// </summary>
        [JsonProperty("GD")]
        public int Gender { get; set; } = 0;
        /// <summary>
        /// Гос номер машины
        /// </summary>
        [JsonProperty("CI")]
        public string CarId { get; set; } = string.Empty;
    }
}
