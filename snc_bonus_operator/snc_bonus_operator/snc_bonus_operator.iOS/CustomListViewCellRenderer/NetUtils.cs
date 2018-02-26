using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;
using snc_bonus_operator;
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
using snc_bonus_operator.Settings;

[assembly: Dependency(typeof(snc_bonus_operator.Ios.DependencyServices.NetUtils))]
namespace snc_bonus_operator.Ios.DependencyServices
{
    public class NetUtils : Interfaces.INetUtils
    {
        static SenderConnector sc = new SenderConnector();

        public void SendData(string str, CertificateType type, string TimeOut = null)
        {
            sc.SendData(str, type, TimeOut);
        }

        public bool Open(int port, string Adress, CertificateType type, int ResiveTimeOut = -1)
        {
            return sc.Open(port, Adress, type, ResiveTimeOut);
        }

        public void Close()
        {
            sc.Close();
        }
        public string Receive()
        {
            return sc.Receive();
        }
        public string GetLastError()
        {
            return sc.LastError;
        }
        public InternetStatus IsServerPing(string Adress)
        {
            var result = InternetStatus.Online;
            if (!isPing(Adress))
            {
                result = isPing("vk.com") ? InternetStatus.ServerBroke : InternetStatus.UserBroke;
            }
            return result;
        }

        private bool isPing(string Adress)
        {
            try
            {
                var ping = new Ping();
                IPAddress ipAddr = null;
                if (!IPAddress.TryParse(Adress, out ipAddr))
                {
                    foreach (IPAddress ip in Dns.GetHostAddresses(Adress))
                    {
                        ipAddr = ip;
                    }
                }
                var reply = ping.Send(ipAddr, 2000);
                return reply.Status == IPStatus.Success;
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
                return false;
            }
        }

        /// <summary>
        /// Класс для работы со службой
        /// </summary>
        public class SenderConnector : IDisposable
        {
            private MainSender send = new MainSender();
            private string error = "";
            private string msg;
            /// <summary>
            /// Сообщение
            /// </summary>
            public string Message
            {
                get
                {
                    return msg;
                }
            }
            bool disposed = false;
            /// <summary>
            /// Состояние SSL
            /// </summary>
            public bool Ssl { get; set; }
            /// <summary>
            /// Возможные ошибки
            /// </summary>
            public string LastError
            {
                get
                {
                    return error;
                }
            }
            /// <summary>
            /// Конструктор
            /// </summary>
            public SenderConnector()
            {
                Ssl = true;
            }
            /// <summary>
            /// Использовать SSL
            /// </summary>
            /// <param name="State"></param>
            public void UseSsl(bool State)
            {
                Ssl = State;
            }
            /// <summary>
            /// Отправить запрос
            /// </summary>
            /// <param name="ds">Датасет RequestDS</param>
            /// <param name="TimeOut">Время выполнения запроса</param>
            public void SendData(string ds, CertificateType type, string TimeOut = null)
            {
                try
                {
                    error = "";
                    string name = Guid.NewGuid().ToString();
                    if (TimeOut != null)
                        name += ";TimeOut =" + TimeOut;
                    send.SendPost(new MemoryStream(Encoding.UTF8.GetBytes(ds)), name, type);
                    if (send.LastError != "")
                        error = send.LastError;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
            }
            /// <summary>
            /// Отправить запрос
            /// </summary>
            /// <param name="path">Путь к файлу</param>
            public void SendData(string path, CertificateType type)
            {
                send.SendPost(new MemoryStream(Encoding.UTF8.GetBytes(path)), Guid.NewGuid().ToString(), type);
                if (send.LastError != "")
                    error = send.LastError;
            }
            /// <summary>
            /// Открыть сокед
            /// </summary>
            /// <param name="port">Порт</param>
            /// <param name="Adress">Ip адрес</param>
            /// <param name="ResiveTimeOut">Время ожидания получаемых данных</param>
            /// <returns></returns>
            public bool Open(int port, string Adress, CertificateType type, int ResiveTimeOut = -1)
            {
                return send.Open(port, Adress, Ssl, type, ResiveTimeOut);
                //return send.Open(port, Adress, false, type, ResiveTimeOut);
            }
            /// <summary>
            /// Получить ответ на запрос
            /// </summary>
            /// <returns>При успешном получении RequestDS</returns>
            public string Receive()
            {
                msg = "";
                string str = "";
                try
                {
                    List<object> values = (List<object>)send.ReceivePost();
                    foreach (object val in values)
                    {
                        if (val != null)
                        {
                            if (val is MemoryStream)
                            {
                                MemoryStream file = val as MemoryStream;
                                file.Position = 0;
                                str = new StreamReader(file).ReadToEnd();
                                Logger.WriteLine("str2 : " + str);
                            }
                            else if (val is string)
                            {
                                if (!val.ToString().Contains("Guid"))
                                {
                                    msg += val.ToString();
                                    Logger.WriteLine("str1 : " + str);
                                }
                            }
                        }
                    }
                    if (send.LastError != "")
                        error = send.LastError;
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                }
                return str;
            }
            /// <summary>
            /// Закрыть соединение
            /// </summary>
            public void Close()
            {
                send.Close();
            }
            /// <summary>
            /// Очистить
            /// </summary>
            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            /// <summary>
            /// Диспосе
            /// </summary>
            /// <param name="disposing"></param>
            protected virtual void Dispose(bool disposing)
            {
                if (disposed)
                    return;

                if (disposing)
                {
                    send.Close();
                }

                disposed = true;
            }
            /// <summary>
            /// Диспосе
            /// </summary>
            ~SenderConnector()
            {
                Dispose(false);
            }

        }

        internal class MainSender
        {
            public string LastError { get; set; }
            private bool Ssl { get; set; }
            public String http_method;
            public String http_url;
            public String http_protocol_versionstring;
            public Hashtable httpHeaders = new Hashtable();
            public List<object> outlist = new List<object> { };
            public TcpClient tcp = null;
            public Socket sender = null;
            public MemoryStream headerstream;
            public Stream sendStream = null;

            public bool Open(int port, string Adress, bool ssl, CertificateType type, int ResiveTimeOut = -1)
            {
                Ssl = ssl;
                tcp = new TcpClient();
                sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                tcp.Client = sender;
                sender.ReceiveBufferSize = 128000;
                sender.SendBufferSize = 128000;
                System.Net.IPAddress ipAddr = null;
                IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
                if (Adress.ToLower() == "localhost")
                {
                    foreach (var ip in localIPs)
                    {
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                            ipAddr = ip;
                    }
                }
                else
                {
                    ipAddr = System.Net.IPAddress.Parse(Adress);
                }
                IPEndPoint ipEndPoint = new IPEndPoint(ipAddr, port);
                sender.ReceiveTimeout = 600000;
                if (ResiveTimeOut != -1)
                    sender.ReceiveTimeout = ResiveTimeOut;
                IAsyncResult result = sender.BeginConnect(ipEndPoint, null, null);

                bool success = result.AsyncWaitHandle.WaitOne(5000, true);

                if (!sender.Connected)
                {
                    // NOTE, MUST CLOSE THE SOCKET

                    sender.Close();
                    return false;
                }
                else
                {
                    return true;
                }

            }

            public void Close()
            {
                // Освобождаем сокет
                try
                {
                    sender.Shutdown(SocketShutdown.Both);
                    sender.Close();
                }
                catch
                {
                }
            }

            private static byte[] ReadFully(Stream input)
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

            public void SendPost(MemoryStream Data, string cookie, CertificateType type)
            {
                LastError = "";
                try
                {
                    // Буфер для входящих данных     
                    string Headers = "POST HTTP/1.0 \r\n" + "Content-type: text/xml \r\n" + "Cookie: Guid=" + cookie + "; \r\n" +
                    "Content-length: " + Data.Length + " \r\n" + "Content-transfer-encoding: text \r\n" + "Connection: open \r\n\r\n";
                    //message = Headers + message;
                    Data.Position = 0;
                    sendStream = GetStream(type);
                    //byte[] msg = new byte[Encoding.UTF8.GetBytes(Headers).Length + Data.ToArray().Length];
                    //Array.Copy(Encoding.UTF8.GetBytes(Headers), 0, msg, 0, msg.Length);
                    //Array.Copy(Data.ToArray(), 0, msg, Encoding.UTF8.GetBytes(Headers).Length, msg.Length);
                    sendStream.Write(Encoding.UTF8.GetBytes(Headers), 0, Encoding.UTF8.GetBytes(Headers).Length);
                    sendStream.Write(Data.ToArray(), 0, Convert.ToInt32(Data.Length));
                    // Отправляем данные через сокет
                    //int bytesSent = sender.Send(ReadFully(sendStream));
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                }
            }
            private X509CertificateCollection GetCertificate(CertificateType type)
            {
                X509CertificateCollection outCertificate = new X509CertificateCollection();
                X509Certificate2 x509 = null;
                AsymmetricCipherKeyPair privateKey = null;
                x509 = new X509Certificate2(Encoding.ASCII.GetBytes(MobileStaticVariables.ConectSettings.Certificates[(int)type].Certificate));
                var stringReader = new StringReader(MobileStaticVariables.ConectSettings.Certificates[(int)type].PrivateKey);
                PemReader reader = new PemReader(stringReader);
                var key = reader.ReadObject() as AsymmetricCipherKeyPair;
                if (key != null)
                    privateKey = key;

                if (x509 != null)
                {
                    if (privateKey != null)
                    {
                        PrivateKeyInfo info = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey.Private);
                        var seq = (Asn1Sequence)Asn1Object.FromByteArray(info.ParsePrivateKey().GetDerEncoded());
                        if (seq.Count != 9)
                            throw new PemException("malformed sequence in RSA private key");

                        var rsa = RsaPrivateKeyStructure.GetInstance(seq);
                        RsaPrivateCrtKeyParameters rsaparams = new RsaPrivateCrtKeyParameters(
                            rsa.Modulus, rsa.PublicExponent, rsa.PrivateExponent, rsa.Prime1, rsa.Prime2, rsa.Exponent1, rsa.Exponent2, rsa.Coefficient);

                        x509.PrivateKey = DotNetUtilities.ToRSA(rsaparams);
                        outCertificate.Add(x509);
                    }
                }
                return outCertificate;
            }
            private Stream GetStream(CertificateType type)
            {
                Stream outstream = null;
                if (Ssl)
                {
                    outstream = new SslStream(tcp.GetStream(), false, App_CertificateValidation, CertificateSelectionCallback);
                    ((SslStream)outstream).AuthenticateAsClient((tcp.Client.RemoteEndPoint as IPEndPoint).Address.ToString(), GetCertificate(type), SslProtocols.Tls, false);
                }
                else
                    outstream = tcp.GetStream();
                return outstream;
            }

            private static bool App_CertificateValidation(Object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
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
                            parseRequest(netStream);
                            readHeaders(netStream);
                            //HeaderLeng = Convert.ToInt32(headerstream.Position);
                            //OneHeader = HeaderLeng;
                        }
                        catch
                        {
                            wh++;
                            if (wh > 100)
                                throw new System.InvalidOperationException("Не найден Content-Length в заголовках");
                        }
                    }
                    else
                    {
                        wh++;
                        if (wh > 100)
                            throw new System.InvalidOperationException("Не найден Content-Length в заголовках");
                    }
                    if (httpHeaders.Contains("Content-Length".ToUpper()))
                        break;
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
                string resive = "";
                outlist = new List<object> { };
                MemoryStream stream = new MemoryStream();
                string contType = "";
                httpHeaders = new Hashtable();
                byte[] bytes = new byte[sender.ReceiveBufferSize];
                try
                {
                    Stream nstr;
                    if (Ssl == true)
                        nstr = sendStream;
                    else
                        nstr = tcp.GetStream();
                    ReadHeaders(nstr);
                    contType = httpHeaders["CONTENT-TYPE"].ToString().Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries)[0].ToLower();
                    ReadData(stream, nstr, Convert.ToInt32(httpHeaders["Content-Length".ToUpper()]));
                    switch (contType)
                    {
                        case "text/xml":
                            resive += GetStringStream(stream);
                            break;
                        case "application/zip":
                            break;
                        case "text/html":
                            resive += GetStringStream(stream);//GetString(((MemoryStream)inputStream).ToArray(), HeaderLeng, oldsres - HeaderLeng);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    LastError = ex.Message;
                }
                if (contType == "application/zip" || contType == "text/xml")
                {
                    stream.Seek(0, SeekOrigin.Begin);
                    outlist.Add(stream);
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

            public void parseRequest(Stream stream)
            {
                String request = streamReadLine(stream);
                string[] tokens = request.Split(' ');
                http_method = tokens[0].ToUpper().Trim();
                http_url = tokens[1].Trim();
                http_protocol_versionstring = tokens[2].Trim();

                //Console.WriteLine("starting: " + request);
            }

            public void readHeaders(Stream stream)
            {
                //Console.WriteLine("readHeaders()");
                String line;
                while ((line = streamReadLine(stream)) != null)
                {
                    if (line.Equals(""))
                    {
                        //Console.WriteLine("got headers");
                        return;
                    }

                    int separator = line.IndexOf(':');
                    if (separator == -1)
                    {
                        throw new Exception("invalid http header line: " + line);
                    }
                    String name = line.Substring(0, separator).ToUpper().Trim();
                    int pos = separator + 1;
                    while ((pos < line.Length) && (line[pos] == ' '))
                    {
                        pos++; // strip any spaces
                    }

                    string value = line.Substring(pos, line.Length - pos);
                    //Console.WriteLine("header: {0}:{1}", name, value);
                    httpHeaders[name] = value.Trim();
                }
            }

            private string streamReadLine(Stream inputStream)
            {
                string data = "";
                int next_char;
                int oldChar;
                int fail = 0;
                while (inputStream.CanRead)
                {
                    if (fail > 100)
                    {
                        try
                        {
                            inputStream.Write(new byte[0], 0, 0);
                        }
                        catch
                        {
                            return "FAIL";
                        }
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

            //static string GetString(byte[] bytes,int leng)
            //{
            //    return System.Text.Encoding.UTF8.GetString(bytes,0,leng);
            //}
            private string GetStringStream(Stream inputStream)
            {
                inputStream.Position = 0;
                string next_char;
                string data = "";
                byte[] buffer = new byte[inputStream.Length];
                ((MemoryStream)inputStream).Read(buffer, 0, buffer.Length);
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
            static string GetString(byte[] bytes, int position, int leng)
            {
                char[] chars = new char[leng];//[bytes.Length / sizeof(char)];
                System.Buffer.BlockCopy(bytes, position, chars, 0, leng);
                return new string(chars);
            }

            public MemoryStream GenerateStreamFromString(MemoryStream stream, string s)
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(s);
                writer.Flush();
                stream.Position = 0;
                return stream;
            }
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
    }    
}
