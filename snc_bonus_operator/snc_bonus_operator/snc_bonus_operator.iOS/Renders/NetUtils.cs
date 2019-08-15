using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using snc_bonus_operator.Settings;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Xamarin.Forms;

[assembly: Dependency(typeof(snc_bonus_operator.iOS.Implementation.NetUtils))]
namespace snc_bonus_operator.iOS.Implementation
{
    public class NetUtils : Interfaces.INetUtils
    {
        private SenderConnector _sc;

        public void SendData(string str)
        {
            _sc.SendData(str);
        }
        public bool Open(int port, string Adress, int ResiveTimeOut = -1)
        {
            _sc = new SenderConnector();
            return _sc.Open(port, Adress, ResiveTimeOut);
        }
        public void Close()
        {
            _sc.Close();
        }
        public string Receive()
        {
            return _sc.Receive();
        }
        public string GetLastError()
        {
            return _sc.LastError;
        }
        /// <summary>
        /// Опрос интернета
        /// </summary>
        /// <param name="url">Сайт для опроса</param>
        /// <returns>Наличие интернета</returns>
        public bool CheckInternetConnection(string url = "http://sncard.ru")
        {
            try
            {
                HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(url);
                iNetRequest.Timeout = 5000;
                WebResponse iNetResponse = iNetRequest.GetResponse();

                iNetResponse.Close();
            }
            catch //(WebException ex)
            {
                return false;
            }
            return true;
        }

        public InternetStatus IsServerPing(string Adress)
        {
            return InternetStatus.Online;
        }

        /// <summary>
        /// Класс для работы со службой
        /// </summary>
        public class SenderConnector : IDisposable
        {
            private MainSender _sender = new MainSender();
            private string _error = "";
            private string _msg;
            private bool _disposed = false;
            /// <summary>
            /// Сообщение
            /// </summary>
            public string Message
            {
                get
                {
                    return _msg;
                }
            }
            /// <summary>
            /// Состояние SSL
            /// </summary>
            public bool IsSsl
            {
                get
                {
                    return _sender.IsSSL;
                }
            }
            /// <summary>
            /// Возможные ошибки
            /// </summary>
            public string LastError
            {
                get
                {
                    return _error;
                }
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public SenderConnector()
            {
            }
            /// <summary>
            /// Отправить запрос
            /// </summary>
            /// <param name="ds">Датасет RequestDS</param>
            /// <param name="TimeOut">Время выполнения запроса</param>
            public void SendData(string ds, double timeOut = double.NaN)
            {
                try
                {
                    _error = "";
                    string name = Guid.NewGuid().ToString();
                    if (!double.IsNaN(timeOut))
                    {
                        name += ";TimeOut =" + timeOut;
                    }

                    _sender.SendPost(GetMemory(ds), name);
                    if (_sender.LastError != "")
                    {
                    }
                }
                catch (Exception ex)
                {
                    _error = ex.Message;
                }
            }

            /// <summary>
            /// Открыть сокед
            /// </summary>
            /// <param name="port">Порт</param>
            /// <param name="Adress">Ip адрес</param>
            /// <param name="ResiveTimeOut">Время ожидания получаемых данных</param>
            /// <returns></returns>
            public bool Open(int port, string adress, int ResiveTimeOut = -1)
            {
                return _sender.Open(port, adress, ResiveTimeOut);
            }
            /// <summary>
            /// Получить ответ на запрос
            /// </summary>
            /// <returns>При успешном получении RequestDS</returns>
            public string Receive()
            {
                _msg = "";
                string ds = "";
                try
                {
                    List<object> values = (List<object>)_sender.ReceivePost();
                    foreach (object val in values)
                    {
                        if (val != null)
                        {
                            if (val is MemoryStream)
                            {
                                MemoryStream file = val as MemoryStream;
                                file.Position = 0;
                                ds = new StreamReader(file).ReadToEnd();
                            }
                            else
                                if (val is string)
                            {
                                if (!val.ToString().Contains("Guid"))
                                {
                                    _msg += val.ToString();
                                }
                            }
                        }
                    }
                    if (_sender.LastError != "")
                    {
                        _error = _sender.LastError;
                    }
                }
                catch (Exception ex)
                {
                    _error = ex.Message;
                }
                return ds;
            }
            /// <summary>
            /// Закрыть соединение
            /// </summary>
            public void Close()
            {
                _sender.Close();
            }
            /// <summary>
            /// Очистить
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                //GC.SuppressFinalize(this);
            }
            /// <summary>
            /// Диспосе
            /// </summary>
            /// <param name="disposing"></param>
            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                {
                    return;
                }

                if (disposing)
                {
                    _sender.Close();
                }

                _disposed = true;
            }
            ///// <summary>
            ///// Диспосе
            ///// </summary>
            //~SenderConnector()
            //{
            //    Dispose(false);
            //}

            private MemoryStream GetMemory(string wds)
            {
                return new MemoryStream(Encoding.UTF8.GetBytes(wds));
            }
        }

        public class MainSender
        {
            public string LastError { get; set; }
            public Hashtable httpHeaders { get; private set; } = new Hashtable();
            public TcpClient tcp { get; set; }
            public Socket Sender { get; set; }
            public bool IsSSL { get { return true; } }
            public bool IsOpen { get; private set; }

            private Stream _netStream;

            public bool Open(int port, string Adress, int ResiveTimeOut = -1)
            {
                //_ip = Adress;
                tcp = new TcpClient();
                System.Net.IPAddress ipAddr = null;
                if (!IPAddress.TryParse(Adress, out ipAddr))
                {
                    ipAddr = Dns.GetHostAddresses(Adress)[0];
                }
                Sender = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                tcp.Client = Sender;
                Sender.ReceiveBufferSize = 128000;
                Sender.SendBufferSize = 128000;
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
                Sender.ReceiveTimeout = 600000;
                if (ResiveTimeOut != -1)
                {
                    Sender.ReceiveTimeout = ResiveTimeOut;
                }

                IAsyncResult result = Sender.BeginConnect(ipEndPoint, null, null);

                bool success = result.AsyncWaitHandle.WaitOne(5000, true);

                if (!Sender.Connected)
                {
                    IsOpen = false;
                    Sender.Close();
                    return false;
                }
                else
                {
                    IsOpen = true;
                    return true;
                }
            }


            public void Close()
            {
                // Освобождаем сокет
                try
                {
                    IsOpen = false;
                    Sender.Shutdown(SocketShutdown.Both);
                    Sender.Close();
                }
                catch
                {
                }
            }

            private byte[] ReadFully(Stream input)
            {
                byte[] buffer = new byte[16 * 1024];
                using (MemoryStream ms = new MemoryStream())
                {
                    int read;
                    while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        ms.Write(buffer, 0, read);
                    }
                    return ms.ToArray();
                }
            }

            public static X509CertificateCollection GetX509CertificateCollection()
            {
                X509Certificate2 certificate1 = new X509Certificate2();
                X509CertificateCollection collection1 = new X509CertificateCollection();
                collection1.Add(certificate1);
                return collection1;
            }
            public bool SendPost(MemoryStream memoryData, string cookie)
            {
                LastError = "";
                try
                {
                    string Headers = "POST HTTP/1.0 \r\nContent-type: text/xml \r\nCookie: Guid=" + cookie +
                        "; \r\nContent-length: " + memoryData.Length +
                        " \r\nContent-transfer-encoding: text \r\nConnection: open \r\n\r\n";
                    memoryData.Position = 0;
                    var data = memoryData.ToArray();
                    var str = new MemoryStream();
                    byte[] msg = Encoding.UTF8.GetBytes(Headers);
                    str.Write(msg, 0, msg.Length);
                    str.Write(data, 0, data.Length);
                    _netStream = GetStream(null);
                    _netStream.Write(str.ToArray(), 0, Convert.ToInt32(str.Length));
                    return true;
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                    return false;
                }
            }

            private X509CertificateCollection GetCertificate()
            {
                X509CertificateCollection outCertificate = new X509CertificateCollection();
                X509Certificate2 x509 = null;
                AsymmetricCipherKeyPair privateKey = null;
                x509 = new X509Certificate2(Encoding.ASCII.GetBytes(MobileStaticVariables.ConectSettings.Certificates[0].Certificate));
                var stringReader = new StringReader(MobileStaticVariables.ConectSettings.Certificates[0].PrivateKey);
                var key = new PemReader(stringReader).ReadObject();
                //as AsymmetricCipherKeyPair;
                if (key != null)
                {
                    privateKey = (AsymmetricCipherKeyPair)key;
                }

                if (privateKey != null)
                {
                    PrivateKeyInfo info = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey.Private);
                    var seq = (Asn1Sequence)Asn1Object.FromByteArray(info.ParsePrivateKey().GetDerEncoded());
                    if (seq.Count != 9)
                    {
                        throw new PemException("malformed sequence in RSA private key");
                    }

                    var rsa = RsaPrivateKeyStructure.GetInstance(seq);
                    RsaPrivateCrtKeyParameters rsaparams = new RsaPrivateCrtKeyParameters(
                        rsa.Modulus, rsa.PublicExponent, rsa.PrivateExponent, rsa.Prime1, rsa.Prime2, rsa.Exponent1, rsa.Exponent2, rsa.Coefficient);

                    x509.PrivateKey = DotNetUtilities.ToRSA(rsaparams);
                    //Logger.WriteLine("Certificates : " + x509.GetCertHashString() + " hash class : " + this.GetHashCode());
                    outCertificate.Add(x509);
                }
                return outCertificate;
            }

            private Stream GetStream(Stream stream)
            {
                Stream outstream = stream;
                if (stream == null)
                {
                    if (IsSSL)
                    {
                        outstream = new SslStream(tcp.GetStream(), false, App_CertificateValidation, CertificateSelectionCallback);
                        ((SslStream)outstream).AuthenticateAsClient((tcp.Client.RemoteEndPoint as IPEndPoint).Address.ToString(), GetCertificate(), SslProtocols.Tls, false);
                    }
                    else
                    {
                        outstream = tcp.GetStream();
                    }
                }
                return outstream;
            }

            private static bool App_CertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
            {
                return true;
            }

            private static X509Certificate CertificateSelectionCallback(object sender, string targetHost,
                    X509CertificateCollection localCertificates, X509Certificate remoteCertificate, string[] acceptableIssuers)
            {
                return localCertificates[0];
            }

            private void ReadHeaders(Stream netStream)
            {
                bool end = true;
                int wh = 0;
                while (end)
                {
                    if (httpHeaders.Count == 0)
                    {
                        try
                        {
                            var header = GetTypeRequest(netStream);
                            if (header != null)
                            {
                                ParseHeaders(netStream);
                            }
                        }
                        catch
                        {
                            wh++;
                            if (wh > 100)
                            {
                                throw new System.InvalidOperationException("Не найден Content-Length в заголовках");
                            }
                        }
                    }
                    else
                    {
                        wh++;
                        if (wh > 100)
                        {
                            throw new System.InvalidOperationException("Не найден Content-Length в заголовках");
                        }
                    }
                    if (httpHeaders.Contains("Content-Length".ToUpper()))
                    {
                        break;
                    }
                    //if (httpHeaders.Count > 0)
                    //{
                    //    if (httpHeaders["CONTENT-TYPE"].ToString().Replace(";", "").ToLower() == "text/html")
                    //        break;
                    //}
                }
            }

            private void ReadData(MemoryStream outData, Stream inData, int contentLeng)
            {
                byte[] buffer = new byte[contentLeng];
                int position = 0;
                if (inData.CanRead)
                {
                    int next_char;
                    int fail = 0;
                    while (position < contentLeng)
                    {
                        if (fail > 100)
                        {
                            try
                            {
                                inData.Write(new byte[0], 0, 0);
                            }
                            catch
                            {
                                return;
                            }
                        }
                        next_char = inData.ReadByte();
                        if (next_char == -1) { Thread.Sleep(1); fail++; continue; };
                        buffer[position] = Convert.ToByte(next_char);
                        position++;
                    }
                }
                outData.Write(buffer, 0, contentLeng);
                outData.Position = 0;
            }
            public object ReceivePost()
            {
                if (_netStream == null)
                {
                    return null;
                }

                string resive = "";
                var outlist = new List<object> { };
                string contType = "";
                httpHeaders = new Hashtable();
                MemoryStream data = new MemoryStream();
                byte[] bytes = new byte[Sender.ReceiveBufferSize];
                try
                {
                    if (IsSSL)
                    {
                        if (_netStream is SslStream)
                        {
                            if (!((SslStream)_netStream).IsAuthenticated)
                            {
                                outlist.Add("Ошибка авторизации");
                                return outlist;
                            }
                        }
                        else
                        {
                            outlist.Add("Ошибка авторизации");
                            return outlist;
                        }
                    }
                    ReadHeaders(_netStream);
                    contType = httpHeaders["CONTENT-TYPE"].ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)[0].ToLower();
                    ReadData(data, _netStream, Convert.ToInt32(httpHeaders["Content-Length".ToUpper()]));
                    switch (contType)
                    {
                        case "text/html":
                            resive += Encoding.UTF8.GetString(data.ToArray());
                            break;
                            //resive += GetStringStream(_netStream);//GetString(((MemoryStream)inputStream).ToArray(), HeaderLeng, oldsres - HeaderLeng);
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                }
                if (contType == "application/zip" || contType == "text/xml")
                {
                    data.Seek(0, SeekOrigin.Begin);
                    outlist.Add(data);
                    outlist.Add(httpHeaders["COOKIE"]);
                    return outlist;
                }
                else
                {
                    outlist.Add(resive);
                    outlist.Add(httpHeaders["COOKIE"]);
                    return outlist;
                }
            }

            public string GetTypeRequest(Stream stream)
            {
                var request = StreamReadLine(stream);
                string[] tokens = request.Split(' ');
                if (tokens.Length == 0)
                {
                    return null;
                }
                else
                {
                    return tokens[0];
                }
            }

            public void ParseHeaders(Stream stream)
            {
                string line;
                while ((line = StreamReadLine(stream)) != null)
                {
                    if (line.Equals("") || line.Equals("FAIL"))
                    {
                        return;
                    }
                    int separator = line.IndexOf(':');
                    if (separator == -1)
                    {
                        throw new Exception("invalid http header line: " + line);
                    }
                    string name = line.Substring(0, separator).ToUpper().Trim();
                    int pos = separator + 1;
                    while ((pos < line.Length) && (line[pos] == ' '))
                    {
                        pos++;
                    }
                    string value = line.Substring(pos, line.Length - pos);
                    httpHeaders[name] = value.Trim();
                }
            }

            private string StreamReadLine(Stream inputStream)
            {
                string data = "";
                int next_char;
                int oldChar;
                int fail = 0;
                while (inputStream.CanRead)
                {
                    if (fail > 100)
                    {
                        //try
                        //{
                        //    return "FAIL";
                        //    inputStream.Write(new byte[0], 0, 0);
                        //}
                        //catch
                        //{
                        //    return "FAIL";
                        //}
                        return "FAIL";
                    }
                    next_char = inputStream.ReadByte();
                    if (next_char == '\n') { break; }
                    if (next_char == '\r') { continue; }
                    if (next_char == -1) { Thread.Sleep(1); fail++; continue; };
                    oldChar = next_char;
                    data += Convert.ToChar(next_char);
                }
                return data;
            }

            private string GetStringStream(NetworkStream inputStream)
            {
                inputStream.Position = 0;
                string next_char;
                string data = "";
                byte[] buffer = new byte[inputStream.Length];
                inputStream.Read(buffer, 0, buffer.Length);
                string mass = Encoding.UTF8.GetString(buffer);
                int i = 0;
                while (i != mass.Length)
                {
                    next_char = Convert.ToChar(mass[i]).ToString();
                    i++;
                    if (next_char == "\0") { break; }
                    if (next_char == "\n") { continue; }
                    if (next_char == "\r") { continue; }
                    if (next_char == "") { Thread.Sleep(1); continue; };
                    data += Convert.ToChar(mass[i - 1]);
                }
                return data;
            }

            /// <summary>
            /// Опрос интернета
            /// </summary>
            /// <param name="url">Сайт для опроса</param>
            /// <returns>Наличие интернета</returns>
            public bool CheckInternetConnection(string url = "http://sncard.ru")
            {
                try
                {
                    HttpWebRequest iNetRequest = (HttpWebRequest)WebRequest.Create(url);
                    iNetRequest.Timeout = 5000;
                    WebResponse iNetResponse = iNetRequest.GetResponse();

                    iNetResponse.Close();
                }
                catch //(WebException ex)
                {
                    return false;
                }
                return true;
            }

            public class MobileSslStream : SslStream
            {
                public CertificateType Type { get; set; }
                public MobileSslStream(Stream innerStream, bool leaveInnerStreamOpen, RemoteCertificateValidationCallback userCertificateValidationCallback, LocalCertificateSelectionCallback userCertificateSelectionCallback)
                    : base(innerStream, leaveInnerStreamOpen, userCertificateValidationCallback, userCertificateSelectionCallback)
                {
                }
            }
        }
    }
}