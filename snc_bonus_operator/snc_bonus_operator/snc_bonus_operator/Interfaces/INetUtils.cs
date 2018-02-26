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
        bool Open(int port, string ip, CertificateType type, int resiveTimeOut = -1);

        /// <summary>
        /// Отправить данные на службу
        /// </summary>
        /// <param name="str">Строка содержащая xml</param>
        /// <param name="timeOut">Время ожидания в мс (по умолчанию должно быть 10 минут)</param>
        void SendData(string str, CertificateType type, string timeOut = null);

        /// <summary>
        /// Получение ответа от службы в xml
        /// </summary>
        /// <returns></returns>
        string Receive();

        /// <summary>
        /// Закрыть соединение
        /// </summary>
        void Close();

        /// <summary>
        /// Получение последней ошибки
        /// </summary>
        /// <returns></returns>
        string GetLastError();

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
