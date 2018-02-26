using Newtonsoft.Json;
using snc_bonus_operator.Protocol;
using System;

namespace snc_bonus_operator.Model.QR_code
{
    public delegate void QRInfoAddedHandler();
    
    /// <summary>
    /// Содержимое QR-кода для АЗС
    /// </summary>
    public class ClientQR
    {
        public event QRInfoAddedHandler QRInfoAddedEvent;
        public event QRInfoAddedHandler BarInfoAddedEvent;

        public QRPrice Price { get; set; } = new QRPrice();
        public string ValueEAN13 { get; set; } = "";
        private bool _isQrRead = false;
        private bool _isEANRead = false;

        public void ResetQRInfoAddedEvent()
        {
            QRInfoAddedEvent = delegate
            {
                _isQrRead = false;
            };
            BarInfoAddedEvent = delegate
            {
                _isEANRead = false;
            };
        }
        /// <summary>
        /// Преобразовать класс в JSON строку
        /// </summary>
        /// <returns>Строка</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
        /// <summary>
        /// Получить свойства класса из JSON строки
        /// </summary>
        /// <param name="json">Строка</param>
        /// <returns>Успех</returns>
        public bool FromJson(string json)
        {
            try
            {
                Price = JsonConvert.DeserializeObject<QRPrice>(json);
                QRInfoAddedEvent?.Invoke();
                _isQrRead = true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                _isQrRead = false;
            }
            return _isQrRead;
        }

        /// <summary>
        /// Получить свойства класса из JSON строки
        /// </summary>
        /// <param name="json">Строка</param>
        /// <returns>Успех</returns>
        public bool FromString(string json)
        {
            try
            {
                ValueEAN13 = json;
                BarInfoAddedEvent?.Invoke();
                _isEANRead = true;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                _isEANRead = false;
            }
            return _isQrRead;
        }

        /// <summary>
        /// Был ли считан QR-код
        /// </summary>
        /// <returns></returns>
        public bool IsQRRead()
        {
            return _isQrRead;
        }

        public void SetQrRead(bool isQrRead)
        {
            _isQrRead = isQrRead;
        }

        public void SetBarRead(bool isQrRead)
        {
            _isEANRead = isQrRead;
        }
    }
}
