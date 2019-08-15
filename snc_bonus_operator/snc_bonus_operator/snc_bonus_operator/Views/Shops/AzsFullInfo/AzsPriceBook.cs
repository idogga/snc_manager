using System;
using System.Collections.Generic;

namespace snc_bonus_operator
{
    /// <summary>
    /// Книга товаров и цен на АЗС
    /// </summary>
    public class AzsPriceBook
    {
        /// <summary>
        /// Код АЗС
        /// </summary>
        public int AzsId { get; set; } = 0;
        /// <summary>
        /// Код эмитента
        /// </summary>
        public int IssuerId { get; set; } = 0;

        /// <summary>
        /// Коды карт достыпных для заправки для пользователя
        /// </summary>
        public List<CardFuels> AvailableCardsAndFuels { get; set; } = new List<CardFuels>();

        /// <summary>
        /// Название АЗС
        /// </summary>
        public string AzsName { get; set; } = "";

        /// <summary>
        /// Список колонок, шлангов и их товаров
        /// </summary>
        public List<FuelDispenser> Book = new List<FuelDispenser>();

        /// <summary>
        /// ТРК - топливная колонка с шлангами и типами топлив
        /// </summary>
        public class FuelDispenser
        {
            /// <summary>
            /// Номер колонки
            /// </summary>
            public int DispenserNumber { get; set; } = 0;
            /// <summary>
            /// Номера шлангов и их типы топлив
            /// </summary>
            public List<NozzleDescription> NozzleFuelTypes { get; set; } = new List<NozzleDescription>();
        }

        /// <summary>
        /// Список цен на товары
        /// </summary>
        public List<FuelPrice> Prices { get; set; } = new List<FuelPrice>();

        /// <summary>
        /// Карты по которым можно расчитаться
        /// </summary>
        public class CardFuels
        {
            /// <summary>
            /// Номер карты
            /// </summary>
            public int CardKey { get; set; }
            /// <summary>
            /// Список топлив
            /// </summary>
            public List<int> Fuels { get; set; }
        }


        /// <summary>
        /// Цена топлива на колонке
        /// </summary>
        public class FuelPrice
        {
            /// <summary>
            /// Номер топлива
            /// </summary>
            public int F_Code { get; set; } = 0;
            /// <summary>
            /// Номер топлива
            /// </summary>
            public int FF_Code { get; set; } = 0;
            /// <summary>
            /// Название топлива
            /// </summary>
            public string FuelName { get; set; } = "";
            /// <summary>
            /// Цена за литр
            /// </summary>
            public double Price { get; set; } = 0;
            /// <summary>
            /// Дата последнего обновления цены
            /// </summary>
            public DateTime DateTimeChange { get; set; } = DateTime.MinValue;
        }

        /// <summary>
        /// Описание шланга
        /// </summary>
        public class NozzleDescription
        {
            /// <summary>
            /// Уникальный номер шланга
            /// </summary>
            public int Nozzle { get; set; } = 0;

            /// <summary>
            /// Номер 
            /// </summary>
            public int FF_Code { get; set; } = 0;

            /// <summary>
            /// Номер коллекции
            /// </summary>
            public int F_Code { get; set; } = 0;

            /// <summary>
            /// Статус пистолета
            /// </summary>
            public PumpStateEnum Status { get; set; } = PumpStateEnum.PUMP_STATE_UNDEFINED;
        }
    }
}