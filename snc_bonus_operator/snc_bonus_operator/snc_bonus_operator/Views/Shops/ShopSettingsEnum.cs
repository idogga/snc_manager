using System;

namespace snc_bonus_operator
{
    /// <summary>
    /// Возможности ТО
    /// </summary>
    [Flags]
    public enum ShopSettingsEnum
    {
        /// <summary>
        /// Без типов
        /// </summary>
        None = 0,
        /// <summary>
        /// Расчет наличными
        /// </summary>
        Cash = 1 << 0,
        /// <summary>
        /// Расчет банковской картой
        /// </summary>
        BankCard = 1 << 1,
        /// <summary>
        /// Расчет топливной картой
        /// </summary>
        FuelCard = 1 << 2,
        /// <summary>
        /// Расчет через мобильное приложение
        /// </summary>
        MobileApp = 1 << 3,
        /// <summary>
        /// Заправка топливом
        /// </summary>
        FuelSellingService = 1 << 4,
        /// <summary>
        /// Продажа товаров
        /// </summary>
        GoodSellingService = 1 << 5,
        /// <summary>
        /// Питание
        /// </summary>
        FoodService = 1 << 6,
        /// <summary>
        /// Автомойка
        /// </summary>
        CarWashService = 1 << 7,
        /// <summary>
        /// Гостиница
        /// </summary>
        HotelService = 1 << 8,
        /// <summary>
        /// Автостоянка
        /// </summary>
        CarParkService = 1 << 9,
        /// <summary>
        /// Автостоянка
        /// </summary>
        GasSellingService = 1 << 10,
    }
}
