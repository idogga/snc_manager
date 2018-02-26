namespace snc_bonus_operator.Interfaces
{
    public interface IDevice
    {
        /// <summary>
        /// Получение IMEI
        /// </summary>
        /// <returns>строка</returns>
        string GetIdentifier();

        /// <summary>
        /// Получение номера версии
        /// </summary>
        string GetVersion();

        /// <summary>
        /// Расчет длины строки
        /// </summary>
        /// <param name="text">Текст, длина которого высчитывается</param>
        /// <returns></returns>
        double CalculateWidth(string text);

        /// <summary>
        /// Закрытие приложения
        /// </summary>
        void CloseApp();
    }
}
