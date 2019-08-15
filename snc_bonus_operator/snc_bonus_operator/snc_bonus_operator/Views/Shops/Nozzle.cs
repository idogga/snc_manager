using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    /// <summary>
    /// Представление пистолета
    /// </summary>
    public class Nozzle
    {
        /// <summary>
        /// Идентификатор колонки
        /// </summary>
        public int Id;
        /// <summary>
        /// Код коллекции
        /// </summary>
        public int F_Cod;

        /// <summary>
        /// Код товара
        /// </summary>
        public int FF_Cod;

        /// <summary>
        /// Мнемоника топлива
        /// </summary>
        public string FuelName;

        /// <summary>
        /// Состояние пистолета
        /// </summary>
        public PumpStateEnum State;

        /// <summary>
        /// Цена
        /// </summary>
        public double Price;

        public Nozzle(int id, int f_cod, int ff_cod, string fuelName, PumpStateEnum state, double price)
        {
            Id = id;
            F_Cod = f_cod;
            FF_Cod = ff_cod;
            FuelName = fuelName;
            State = state;
            Price = price;
        }

        public static implicit operator Nozzle(List<Frame> v)
        {
            throw new NotImplementedException();
        }
    }
}