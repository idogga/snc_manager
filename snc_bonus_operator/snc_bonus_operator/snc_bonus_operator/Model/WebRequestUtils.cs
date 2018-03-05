using Newtonsoft.Json;
using snc_bonus_operator.Interfaces;
using snc_bonus_operator.Protocol;
using snc_bonus_operator.Settings;
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Xamarin.Forms;

namespace snc_bonus_operator
{
    public class WebRequestUtils
    {
        #region Типизированные запросы к службе
        /// <summary>
        /// Запрос на получение новостей RSS
        /// </summary>
        /// <returns>Json объект</returns>
        public string SendRssRequest(string typeTag, string newsInfo)
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.RssTries; i++)
            {
                try
                {
                    return SendRequest(Request.GetRssRequestDSxml(typeTag, newsInfo), CertificateType.BASE_ISSUER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            throw new Exception("SendRssRequest : Can't connect to server");
        }

        /// <summary>
        /// Запрос на получение информации по эмитентам
        /// </summary>
        /// <returns>Json объект</returns>
        public string SendIssuerRequest(string tag, string detailValue)
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.IssuerTries; i++)
            {
                try
                {
                    return SendRequest(Request.GetIssuerXml(tag, detailValue), CertificateType.BASE_ISSUER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            throw new Exception("SendIssuerRequest : Can't connect to server");

        }

        /// <summary>
        /// Запрос на получение информации по АЗС на карте
        /// </summary>
        /// <returns>Json объект</returns>
        public string SendMapRequest()
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.MapTries; i++)
            {
                try
                {
                    return SendRequest(Request.mapRequestDSxml, CertificateType.BASE_ISSUER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            throw new Exception("SendMapRequest : Can't connect to server");
        }

        /// <summary>
        /// Запрос на получение информации по товарам и ценам на АЗС
        /// </summary>
        /// <returns>Json объект</returns>
        public string SendPriceRequest(int issuerId, int azsId, string userId = "")
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.MapTries; i++)
            {
                try
                {
                    return SendRequest(Request.GetPriceXml(issuerId, azsId, userId), CertificateType.BASE_ISSUER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            throw new Exception("SendMapRequest : Can't connect to server");
        }

        /// <summary>
        /// Запрос на словарей
        /// </summary>
        /// <returns>Json объект</returns>
        public string SendSystemRequest(string typeTag, string detailValue)
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.MapTries; i++) // TODO добавить количество попыток системного запроса
            {
                try
                {
                    return SendRequest(Request.GetSystemInfoXml(typeTag, detailValue), CertificateType.BASE_ISSUER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            throw new Exception("SendSystemRequest : Can't connect to server");
        }

        /// <summary>
        /// Запрос на получение информации 
        /// </summary>
        /// <param name="typeTag">Profile - данные о пользователе, класс MobileUser
        /// GetDevice - запросить данные об устройстве, класс DeviceInfo
        /// SetDevice - передать данные об устройстве, класс DeviceInfo</param>
        /// <returns>Json объект</returns>
        public string SendMobileInfoRequest(string typeTag, string detailValue)
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.OrderTries; i++) // TODO добавить количество попыток системного запроса
            {
                try
                {
                    return SendRequest(Request.GetMobileInfoXml(typeTag, detailValue), CertificateType.PRIVATE_USER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            throw new Exception("Can't connect to server");
        }

        public string SendVerifyTransaction(string typeTag, string detailValue)
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.OrderTries; i++) 
            {
                try
                {
                    return SendRequest(Request.GetVerifyTransactionXml(typeTag, detailValue), CertificateType.PRIVATE_USER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            throw new Exception("Can't connect to server");
        }
        

        /// <summary>
        /// Запрос на получение карт пользователя
        /// </summary>
        /// <param name="typeTag">Card - запрос карт пользователя
        /// App - запрос приложений на карте</param>
        /// <returns>Json объект</returns>
        public string SendCardRequest(string typeTag, string userId)
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.CardTries; i++)
            {
                try
                {
                    return SendRequest(Request.GetCardRequestDSxml(typeTag, userId), CertificateType.PRIVATE_USER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            throw new Exception("SendCardRequest : Can't connect to server");
        }

        /// <summary>
        /// Запрос на заправку
        /// </summary>
        /// <param name="graphicalNumber"></param>
        /// <param name="detailsKey"></param>
        /// <param name="detailValue"></param>
        /// <param name="issuerId"></param>
        /// <param name="azsId"></param>
        /// <param name="requestKey"></param>
        /// <returns></returns>
        public string SendOrderRequest(string graphicalNumber, int detailsKey, string detailValue, string issuerId, string azsId, string requestKey)
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.OrderTries; i++)
            {
                try
                {
                    return SendRequest(Request.GetOrderXml(graphicalNumber, detailsKey, detailValue, issuerId, azsId, requestKey), CertificateType.PRIVATE_USER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            Logger.WriteLine("SendOrderRequest : Can't connect to server");
            throw new Exception("ErrorCannotConnectToSever");
        }

        /// <summary>
        /// Запрос для регистрации пользователя в системе
        /// </summary>
        /// <param name="typeTag">Register - Регистрация пользователя
        /// RegisterInfo - Запрос информации для регистрации пользователя по Email</param>
        /// <param name="detailValue">Регистрационные данные</param>
        /// <returns></returns>
        public string SendAuthRequest(string typeTag, RegManager detailValue)
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.OrderTries; i++) // TODO добавить количество попыток запроса авторизации
            {
                try
                {
                    return SendRequest(Request.GetAuthXml(typeTag, Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(detailValue)))), CertificateType.BASE_ISSUER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            Logger.WriteLine("SendAuthRequest : Can't connect to server");
            throw new Exception("ErrorCannotConnectToSever");
        }

        /// <summary>
        /// Запрос для регистрации пользователя в системе
        /// </summary>
        /// <param name="typeTag">Register - Регистрация пользователя
        /// RegisterInfo - Запрос информации для регистрации пользователя по Email</param>
        /// <param name="detailValue">Регистрационные данные</param>
        /// <returns></returns>
        public string SendAuthRequest(string typeTag, string detailValue)
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.OrderTries; i++) // TODO добавить количество попыток запроса авторизации
            {
                try
                {
                    return SendRequest(Request.GetAuthXml(typeTag, detailValue), CertificateType.BASE_ISSUER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            Logger.WriteLine("SendAuthRequest : Can't connect to server");
            throw new Exception("ErrorCannotConnectToSever");
        }

        /// <summary>
        /// Запрос для регистрации пользователя в системе
        /// </summary>
        /// <param name="typeTag">Register - Регистрация пользователя
        /// RegisterInfo - Запрос информации для регистрации пользователя по Email</param>
        /// <param name="detailValue">Регистрационные данные</param>
        /// <returns></returns>
        public string SendAuthRequest(string typeTag, StuffModel detailValue)
        {
            for (int i = 0; i < MobileStaticVariables.ConectSettings.OrderTries; i++) // TODO добавить количество попыток запроса авторизации
            {
                try
                {
                    return SendRequest(Request.GetAuthXml(typeTag, Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(detailValue)))), CertificateType.BASE_ISSUER);
                }
                catch (Exception ex)
                {
                    Logger.WriteError(ex);
                }
            }
            Logger.WriteLine("SendAuthRequest : Can't connect to server");
            throw new Exception("ErrorCannotConnectToSever");
        }

        /// <summary>
        /// Запрос для регистрации пользователя в системе
        /// </summary>
        /// <param name="typeTag">Register - Регистрация пользователя
        /// RegisterInfo - Запрос информации для регистрации пользователя по Email</param>
        /// <param name="detailValue">Регистрационные данные</param>
        /// <returns></returns>
        //public string SendAuthRequest(string typeTag, GetPrograms detailValue)
        //{
        //    for (int i = 0; i < MobileStaticVariables.ConectSettings.OrderTries; i++) // TODO добавить количество попыток запроса авторизации
        //    {
        //        try
        //        {
        //            return SendRequest(Request.GetAuthXml(typeTag, Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(detailValue)))), CertificateType.BASE_ISSUER);
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.WriteError(ex);
        //        }
        //    }
        //    Logger.WriteLine("SendAuthRequest : Can't connect to server");
        //    throw new Exception("ErrorCannotConnectToSever");
        //}

        /// <summary>
        /// Запрос для регистрации нового пользователя в системе
        /// </summary>
        /// <param name="typeTag">Register - Регистрация пользователя
        /// RegisterInfo - Запрос информации для регистрации пользователя по Email</param>
        /// <param name="detailValue">Регистрационные данные</param>
        /// <returns></returns>
        //public string SendRegistrationRequest(string typeTag, UserRequisites detailValue)
        //{
        //    for (int i = 0; i < MobileStaticVariables.ConectSettings.OrderTries; i++) // TODO добавить количество попыток запроса авторизации
        //    {
        //        try
        //        {
        //            return SendRequest(Request.GetAuthXml(typeTag, Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(detailValue)))), CertificateType.BASE_ISSUER);
        //        }
        //        catch (Exception ex)
        //        {
        //            Logger.WriteError(ex);
        //        }
        //    }
        //    Logger.WriteLine("SendAuthRequest : Can't connect to server");
        //    throw new Exception("ErrorCannotConnectToSever");
        //}
        #endregion

        #region Общий запрос
        private string SendRequest(string xml, CertificateType type)
        {
            if (!DependencyService.Get<INetUtils>().CheckInternetConnection())
            {
                Logger.WriteLine("SendRequest : No internet connection");
                throw new Exception("ErrorCannotConnectToSever");
            }
            //timer.Start();
            var res = SendMobileRequest(xml, type);
            if (res/*.Result*/ != "")
            {
                try
                {
                    using (XmlReader reader = XmlReader.Create(new StringReader(res/*.Result*/)))
                    {
                        reader.MoveToContent();
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                if (reader.Name == "DetailsValue")
                                {
                                    if (XNode.ReadFrom(reader) is XElement el)
                                    {
                                        var bytes = Convert.FromBase64String(el.Value);
                                        return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    //timer.Stop();
                    Logger.WriteError(ex);
                    Logger.WriteLine("Parse response: " + ex.Message);
                    throw new Exception("RequestErrorDescription");
                    //isNeedException = true;
                }
            }
            else
            {
                //timer.Stop();
                Logger.WriteLine("Empty response");
                throw new Exception("RequestErrorDescription");
                //isNeedException = true;

            }
            //timer.Stop();
            //if(isNeedException)
            //    throw new NoInternetException("Отсутсвует подключение к серверу");
            return "";
        }

        public string SendMobileRequest(string xml_ds, CertificateType type)
        {
            string ip = MobileStaticVariables.ConectSettings.Certificates[(int)type].IP;
            int port = MobileStaticVariables.ConectSettings.Certificates[(int)type].Port;
#if DEBUGARTYOM
            port = MobileStaticVariables.ConectSettings.DebugPort;
#endif
            var res = "";
            try
            {
                if (DependencyService.Get<INetUtils>().Open(port, ip, type, 60000))
                {
                    DependencyService.Get<INetUtils>().SendData(xml_ds, type);
                    if (DependencyService.Get<INetUtils>().GetLastError() == "")
                    {
                        res = DependencyService.Get<INetUtils>().Receive();
                        if (DependencyService.Get<INetUtils>().GetLastError() == "")
                            return res;
                        else
                            throw new Exception("ErrorEmptyString");
                    }
                }
                if (res == "")
                    throw new Exception("ErrorEmptyString");
                DependencyService.Get<INetUtils>().Close();
                return res;
            }
            catch
            {
                ip = "192.168.1.2";
                if (DependencyService.Get<INetUtils>().Open(port, ip, type, 15000))
                {
                    DependencyService.Get<INetUtils>().SendData(xml_ds, type);
                    if (DependencyService.Get<INetUtils>().GetLastError() == "")
                    {
                        res = DependencyService.Get<INetUtils>().Receive();
                        if (DependencyService.Get<INetUtils>().GetLastError() == "")
                            return res;
                        else
                        {
                            DependencyService.Get<INetUtils>().Close();
                            return "";
                        }
                    }
                }
                DependencyService.Get<INetUtils>().Close();
                return res;
            }
        }
#endregion
    }
}
