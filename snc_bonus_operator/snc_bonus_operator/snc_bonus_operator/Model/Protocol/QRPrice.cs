using Newtonsoft.Json;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Text;

namespace snc_bonus_operator.Protocol
{
    public class QRPrice
    {
        /// <summary>
        /// Номер устройства пользователя
        /// </summary>
        [JsonProperty("D")]
        public int DeviceKey { get; set; } = 0;

        /// <summary>
        /// Информация о клиенте
        /// </summary>
        [JsonProperty("I")]
        public string Info { get; set; } = "";

        /// <summary>
        /// Информация для офлайна
        /// </summary>
        [JsonProperty("DT")]
        public string Data { get; set; } = "";


        public void SetInfo(QRInfo info, string key)
        {
            var str = JsonConvert.SerializeObject(info);
            Info = GetHash(str, key);
        }

        private string GetHash(string input, string keyString)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] iv = new byte[16];
            //Set up
            AesEngine engine = new AesEngine();
            CbcBlockCipher blockCipher = new CbcBlockCipher(engine); //CBC
            PaddedBufferedBlockCipher cipher = new PaddedBufferedBlockCipher(blockCipher); //Default scheme is PKCS5/PKCS7
            KeyParameter keyParam = new KeyParameter(Encoding.UTF8.GetBytes(keyString));
            ParametersWithIV keyParamWithIV = new ParametersWithIV(keyParam, iv, 0, 16);
            // Encrypt
            cipher.Init(true, keyParamWithIV);
            byte[] outputBytes = new byte[cipher.GetOutputSize(inputBytes.Length)];
            int length = cipher.ProcessBytes(inputBytes, outputBytes, 0);
            cipher.DoFinal(outputBytes, length); //Do the final block
            string encryptedInput = Convert.ToBase64String(outputBytes);

            Logger.WriteLine("Encrypted string: {0}" + encryptedInput);

            //Decrypt            
            cipher.Init(false, keyParamWithIV);
            byte[] comparisonBytes = new byte[cipher.GetOutputSize(outputBytes.Length)];
            length = cipher.ProcessBytes(outputBytes, comparisonBytes, 0);
            cipher.DoFinal(comparisonBytes, length); //Do the final block

            Logger.WriteLine("Decrypted string: {0}" + Encoding.UTF8.GetString(comparisonBytes, 0, comparisonBytes.Length));
            return encryptedInput;
        }

        public override string ToString()
        {
            string res = "";
            res += string.Format("Номер устройства : {0}\n", DeviceKey);
            res += string.Format("Шифрованая строка : {0}\n", Info);
            return res;
        }
    }

    public class QRInfo
    {
        /// <summary>
        /// Графический номер пользователя
        /// </summary>
        [JsonProperty("G")]
        public string GraphicalCardNumber { get; set; } = "";
        /// <summary>
        /// Количество бонусов для списания
        /// </summary>
        [JsonProperty("B")]
        public double BonusCount { get; set; } = 0;
        /// <summary>
        /// Количество бонусов для списания
        /// </summary>
        [JsonProperty("T")]
        public DateTime TimeGeneration { get; set; } = DateTime.Now;
    }

    public class QRData
    {
        [JsonProperty("CODA")]
        public int Cod_A { get; set; }

        [JsonProperty("CODO")]
        public int Cod_O { get; set; }

        [JsonProperty("CODOWN")]
        public int Cod_Own { get; set; }

        [JsonProperty("LOA")]
        public string CollectionSet { get; set; }

        [JsonProperty("MAK")]
        public int MifareApplicationKey { get; set; }
        /// <summary>
        /// тип дисконта
        /// </summary>
        [JsonProperty("DT")]
        public int DiscountType { get; set; }

        [JsonProperty("DB")]
        public double DiscontBonus { get; set; }

        /// <summary>
        /// Значение скидки
        /// </summary>
        [JsonProperty("DV")]
        public int DiscontVolume { get; set; }

        [JsonProperty("BSR")]
        public int BonusSumRezhim { get; set; }

        [JsonProperty("G")]
        public string GraphicalNumber { get; set; } = string.Empty;
    }
}
