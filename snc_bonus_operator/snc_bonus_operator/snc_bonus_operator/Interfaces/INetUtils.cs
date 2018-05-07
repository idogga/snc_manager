using snc_bonus_operator.Settings;

namespace snc_bonus_operator.Interfaces
{
    public interface INetUtils
    {
        /// <summary>
        /// Открыть соединение
        /// </summary>
        /// <param name="port">Порт службы</param>
        /// <param name="ip">адрес службы</param>
        /// <param name="resiveTimeOut">Время ожидания в мс (по умолчанию должно быть 10 минут)</param>
        /// <returns></returns>
        object Open(int port, string Adress, CertificateType type, int ResiveTimeOut = -1);

        /// <summary>
        /// Отправить данные на службу
        /// </summary>
        /// <param name="str">Строка содержащая xml</param>
        /// <param name="timeOut">Время ожидания в мс (по умолчанию должно быть 10 минут)</param>
        void SendData(object connector, string str, CertificateType type, string TimeOut = null);

        /// <summary>
        /// Получение ответа от службы в xml
        /// </summary>
        /// <returns></returns>
        string Receive(object connector);

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        void Close(object connector);

        /// <summary>
        /// Получение последней ошибки
        /// </summary>
        /// <returns></returns>
        string GetLastError(object connector);

        /// <summary>
        /// Опрос интернета
        /// </summary>
        /// <param name="url">Сайт для опроса</param>
        /// <returns>Наличие интернета</returns>
        bool CheckInternetConnection(string url = "http://sncard.ru");

        /// <summary>
        /// Пингование сервера
        /// </summary>
        /// <param name="Adress">адрес</param>
        /// <returns></returns>
        InternetStatus IsServerPing(string Adress);
    }
}
