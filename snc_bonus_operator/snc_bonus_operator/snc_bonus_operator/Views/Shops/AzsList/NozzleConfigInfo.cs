using System;

namespace snc_bonus_operator
{
    /// <summary>
    /// Запись с параметрами АЗС
    /// </summary>
    public class NozzleConfigInfo
    {
        /// <summary>
        /// Номер стороны колонки
        /// </summary>
        public int Pump = -1;

        /// <summary>
        /// Номер пистолета
        /// </summary>
        public int Nozzle = -1;

        /// <summary>
        /// Статус колонки и пистолета
        /// </summary>
        public PumpStateEnum Status = PumpStateEnum.PUMP_STATE_UNDEFINED;

        /// <summary>
        /// Код группы товаров
        /// </summary>
        public int F_code = -1;

        /// <summary>
        /// Код товара
        /// </summary>
        public int FF_code = -1;

        /// <summary>
        /// Цена товара
        /// </summary>
        public double Price = -1;

        /// <summary>
        /// Название топлива
        /// </summary>
        public string FuelName = "";

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime LastUpdate = DateTime.MinValue;

        public override string ToString()
        {
            return string.Format("ШЛАНГ : [{0}] | СТАТУС : [{1}] | ЦЕНА : [{2}] | НАЗВАНИЕ : [{3}]",
                Nozzle, (PumpStateEnum)Status, Price, FuelName);
        }
    }
}
