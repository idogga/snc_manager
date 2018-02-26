using Realms;
//using snc_bonus.Model.Protocol;
using System;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public class Azs : RealmObject
    {
        /// <summary>
        /// Уникальный код для базы
        /// </summary>
        public int AzsKey { get; set; } = 0;
        /// <summary>
        /// Номер эмитента
        /// </summary>
        public int IssuerKey { get; set; } = 0;
        /// <summary>
        /// Номер АЗС в сети эмитента
        /// </summary>
        public int AzsId { get; set; } = 0;
        /// <summary>
        /// Широта
        /// </summary>
        public double Latitude { get; set; } = 0;
        /// <summary>
        /// Долгота
        /// </summary>
        public double Longitude { get; set; } = 0;
        /// <summary>
        /// Название
        /// </summary>
        public string Title { get; set; } = string.Empty;
        /// <summary>
        /// Примечание
        /// </summary>
        public string Description { get; set; } = string.Empty;
        /// <summary>
        /// Полное название
        /// </summary>
        public string FullName { get; set; } = string.Empty;
        /// <summary>
        /// Короткое название
        /// </summary>
        public string ShortName { get; set; } = string.Empty;
        /// <summary>
        /// Физический адрес
        /// </summary>
        public string Address { get; set; } = string.Empty;
        /// <summary>
        /// Телефон
        /// </summary>
        public string Phone { get; set; } = string.Empty;
        /// <summary>
        /// Способы оплаты покупки на АЗС (наличка,карта,мобильное приложение)
        /// </summary>
        public string PaymentServices { get; set; } = string.Empty;
        /// <summary>
        /// Перечень сервисов и их описаний
        /// </summary>
        public Dictionary<string, string> Services = new Dictionary<string, string>();
        /// <summary>
        /// Дата и время последнего обновления
        /// </summary>
        public DateTimeOffset LastUpdate { get; set; } = DateTimeOffset.MinValue;
        ///// <summary>
        ///// Доступные колонки на АЗС
        ///// </summary>
        //public IList<int> Columns { get; set; } = new List<int>();
        ///// <summary>
        ///// Типы топлив
        ///// </summary>
        //public IList<string> FuelTypes { get; set; } = new List<string>();

        public Azs()
        {

        }

        public Azs(AzsInfo azs)
        {
            AzsKey = azs.AzsKey;
            IssuerKey = azs.IssuerKey;
            AzsId = azs.AzsId;
            Latitude = azs.Latitude;
            Longitude = azs.Longitude;
            Title = azs.Title;
            Description = azs.Description;
            FullName = azs.FullName;
            ShortName = azs.ShortName;
            Address = azs.Address;
            Phone = azs.Phone;
            PaymentServices = azs.PaymentServices;
            Services = azs.Services;
            LastUpdate = azs.LastUpdate;
            //Columns = azs.Columns;
            //FuelTypes = azs.FuelTypes;
        }
    }
}
