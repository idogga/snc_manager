using System;

namespace snc_bonus_operator.Settings
{
    public enum CertificateType
    {
        BASE = 0,
        BASE_ISSUER,
        PRIVATE_USER,

        MAX
    }

    public class ConnectionSettings
    {
        /// <summary>
        /// Колличество попыток загрузить заявку
        /// </summary>
        public int OrderTries { get; set; } = 5;

        /// <summary>
        /// Колличество попыток загрузить ленту новостей
        /// </summary>
        public int RssTries { get; set; } = 3;

        /// <summary>
        /// Колличество попыток загрузить список эмитентов
        /// </summary>
        public int IssuerTries { get; set; } = 4;

        /// <summary>
        /// Колличество попыток загрузить список азс
        /// </summary>
        public int MapTries { get; set; } = 5;

        /// <summary>
        /// Колличество попыток загрузить информацию о карте пользователя
        /// </summary>
        public int CardTries { get; set; } = 5;

        /// <summary>
        /// Время ожидания перед отправкой нового запроса о заправки (в милисекундах)
        /// </summary>
        public int OrderWaitPeriod { get; set; } = 2000;

        /// <summary>
        /// Время ожидания ответа запроса ( в секундах )
        /// </summary>
        public double RequestPeriodLive { get; set; } = 61;

        /// <summary>
        /// Массив сертификатов
        /// </summary>
        public CertificateKey[] Certificates = new CertificateKey[(int)CertificateType.MAX];

#if DEBUG
        public int DebugPort { get; set; } = 2585;
#endif

        public ConnectionSettings()
        {
            Certificates[(int)CertificateType.BASE] = new CertificateKey()
            {
                PrivateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAtgH4e/hDp/OLh5q4ruNh0DJCMlxAzJqZinsBoq2Zjrp5XHoq
dGmUq9li+9ZpL9XZ9w9Ao0BwxxwzRL/7YDj9B/RcW3oEnDS97xVtoeC/mQ4MQLHJ
QB4o+P996ZiAA+GL6D96ZhV1JHgjm3jdjs0Xr3XYCsxM3GbyknfwL5D0G5CpGwmR
SARzHj1BO5vPiNNvuLq4TerDIZ4yipJ41BG/wTO2NIZmwkkRfTBNraU4jgOxGD9/
hY/Ia5chscCDRe0/oQRHmbIomOqYVMDQfX1ow82FaXMbKQsMw9LDJihp3gCFeWu4
HTgr42Ir6zhUMvbWqOKam7yMSqcFIPy0kNQOGwIDAQABAoIBAEMGOsE8TX3d2/YV
7gjJR03qFCKDgoFWNVCft5x2nWPIG8UIX/X6o3sdVKw06wtojxnCDiWQJ3fLVL3u
jN2EXvm87P3q0yPK8F7I31SLdUMhvzVbwybPdHstaurI8+t59ZGTPrm1ESxa6ZHp
Un2x3RUKKoLfdTZ82rtBW7vIf3xeisqXV0y7QEyAxURH0qv19bHwqI0qSagmxK+f
9cFkQ5XCQ/Dhm9sN/9L1RfMYEB5DBsfUB3sVcFMZey+m687noEsAst0uCMpxRD2l
pclsv08G7PeqxJ/KMKPI02+suc7uiQTDVO4/n9nAIPzUAflE+gtAhKz8dvtFrbp5
0pXcRFECgYEA97PjfVe/FB4566rwENGRj0uLSWtqCNVOwKhmdWglIqwcYUXq1sDM
XpmAgSPmf8QC27wT72jw3mlQKK/oRej1s/me5bQbp3QK2mBbjwLcr4e5nQ0sQ2Xg
9vo+QUgMHeCeFMR7cBdJRG8nXUfQn+7qRFb+EI9jSmk2ME7rvYjkTr8CgYEAvBq7
ODyIW1UdpKSfXdnaI7ifJT/yDIW2mpyk1rUdi8VTI8PX3l3WR6jC52nWcsw3+yfo
ySsEEqwP3+Xh4IHoeGJUwFjGc06PhXMiaec4nUaee7vlH70RLJ1qmE7ChVvEElm1
/HBiNLmqjzIV+dZLX/l7/jnn+F+0aU1JfcqS86UCgYEApWfvajzU8Oc6QD/2bRS0
gZ6tlUpwFRPzsb0CJjZ0TdHmZaJdrigykJ0qbZH+kqTeT8MpAL7v0WU7zN+iWES3
BgaTb8o0iT55HNgpa7c6jNAyR0iLnToD3oi+V7N3u9/JUA7garpyB4u+GpEhJBtT
Pm1k3MQTRpY5REr/KqKl6lsCgYAi547wG4Nt9zhd8dJEtwUcdcjKP7hpjHJa9FA+
KzOFWSNZUqD++Uim2XD5QhFyEeUdbMVsdtf1own7EQw9/b0mgZCadJ62jNBjAf9T
yX0e6hjEexRENHA4aCl8g7jiyCl1AkKbyjre78jvc2rShmpML95LSXF7DD9M/vNx
LSLhpQKBgQDwHjI8zFSzstroUnPpTOTWzfNYp7xXGgyOZVMyk2Hw+nQKkDNHN6cq
+MRSF4KjeHCmzW9O11QULWr3adV454QYCJ4CCe6Z8jzsI85IMv0ktC8FlYfJMrFw
3pyzMPZ/oKuWfMx1481ZvvNMqOGbdiSTVvfhQUxvGMMRVMdDFKoqdg==
-----END RSA PRIVATE KEY-----",
                Certificate = @"-----BEGIN CERTIFICATE-----
MIICtTCCAZ2gAwIBAgIIXxzUDt0iUzkwDQYJKoZIhvcNAQELBQAwHzEdMBsGA1UEAwwUcm9vdCBX
SU4tQkZRSUNOR1AzSzkwIBcNMTkwMTA0MDAwMDAwWhgPMjExOTAxMDQwMDAwMDBaMBQxEjAQBgNV
BAMMCWxvY2FsaG9zdDCCASIwDQYJKoZIhvcNAQEBBQADggEPADCCAQoCggEBALYB+Hv4Q6fzi4ea
uK7jYdAyQjJcQMyamYp7AaKtmY66eVx6KnRplKvZYvvWaS/V2fcPQKNAcMccM0S/+2A4/Qf0XFt6
BJw0ve8VbaHgv5kODECxyUAeKPj/femYgAPhi+g/emYVdSR4I5t43Y7NF6912ArMTNxm8pJ38C+Q
9BuQqRsJkUgEcx49QTubz4jTb7i6uE3qwyGeMoqSeNQRv8EztjSGZsJJEX0wTa2lOI4DsRg/f4WP
yGuXIbHAg0XtP6EER5myKJjqmFTA0H19aMPNhWlzGykLDMPSwyYoad4AhXlruB04K+NiK+s4VDL2
1qjimpu8jEqnBSD8tJDUDhsCAwEAATANBgkqhkiG9w0BAQsFAAOCAQEANgPAyfCedDiSEaGUzY4T
V5YcUpJQfcUOBR6NbirjysrGvETOLqabaRpIyxNbD7vJadArPISOjXGzEZ5ZPZQvZTz/pDF0d5ZU
UaU/YgabCEzM/t2Bwk+ZdyygHKX3Qi1k6Jd0a6DaAdp6ZAMJ8Rh2Zezq2F/kytxNJNsxWYEeEYE4
BQ2WIyr9x22FA3DnLdAZFTT7xtS8zlxkMNBvNleeembK+kKV3DHxXg+1DZgRJVwscHhIf0pwWphC
8v/0s/6SS6T6WSkZqgOTkL/BIVgf5uF8/fUBE4UNrWaQBGUdATgy05XO7EnNpeyDgH3zeqotGWYm
qdR/KmUiFEzGmvNftg==
-----END CERTIFICATE-----",
                IP = "mobile.sncard.ru",
                Port = 2585
            };

            Certificates[(int)CertificateType.BASE_ISSUER] = new CertificateKey()
            {
                PrivateKey = @"-----BEGIN RSA PRIVATE KEY-----
MIIEpAIBAAKCAQEAgA8M9RwSOfFeQtqFnXLLC1xyn+IqjB95yan6+6BYuyPAruAa
8LKnbj4H92AbLJXuEUWLXhLrxckFwbzsW7iaiIHstE1sRBQHzG8kK8HBZqaKeFxf
ad1LmnWUKJRxP0LSbM1p/yFBqmB36hdEBeQhcxqrCKZVQ1wn7ncUWlqEDFQZIb4w
cT4GU9szbY8nMlO4BIOzphlX2QOzQFWWjACSxVAvoo+7AiqAxO4pgBFbQk1iNAlO
yhVgJzXEUeMItGZTJZmSd77KZ+qS0Ksi6N3ar72LtZ8YBb3vDIWU1CmfvievUBhS
FCTz8QTIT7/uUyQ40tNQR1kOof7ZaPMueHsfTwIDAQABAoIBAAEH0lcZuTtH3nvq
+XYGgPhxY+F8U/XIQ/kMuI03Nebddb8vnvfhjUV6cJcndZ2djrk5rXVmlDduNqs8
yWPLBx3iwFRQZqWC4WRG6w8Z0NhmaEBpzkF95AKMbt42AcplCgtLeSuU8kBKtlxV
l97GREOOGZ0UnZjzrVD0A1+Zf+Fc9j+/mo5VF9s3HrkpYADT+wcabECtzds9OatA
BPipVjU9Vbugtqln07GGKX8Uy726eSec+KFYxqXVoPh+Hgs5Y1bWyRCEDaGb9ySo
l9YjFPj3DcC4hg/ijAC+d1Zrpi9ZIkrP9l+H4I3AjOizlmMRdusjZJzubNrlNe4U
SRtL8b0CgYEAv6jNota0zcTkHKS3Xxpem3xxwHnmgXElB/DIByqvWGAzCreQSbFw
DDpZPDdj6I01OI88CXKFNtLvAYc5M8B+8N92Rr1mGdwTaw1JTKx7lvedyDoUD3aI
DzZzb1BHMVCx7xTxVBxoWp/0DXBOzhS5K2sZyZhybWV6MY4bv/9plOsCgYEAqwxq
b/AAu/jQa3snkhKJqsyDI2Am7aOwEnJHAS7RxA4CZRLvrmq5b7cylLx6ZEQoqBGD
N/5tz8PTjYnG2XE78ZMwP+kE7qno7RS5m16kwQ9T0L3ir5QZiIuOY6f8hm8GrreC
duv85PPztGsao58EWrWF4u07rVNunGrxPltvVi0CgYEAjHAOrmGRfq3lUK/JxiA2
bsyTNaydIQBdWCIxED9Q2Ps0q4eybK1eIzemJ5+Wz9KYyub54RpPTsrlY0NIwQku
eyXjLxadeBlxCSJlMY+5x/eNYChehq4eKLeHgmtan2I3366C31Upii5m0GoY9Jzu
ykfiT3wrbMnM7f7pipiHLx0CgYBrY54nrS0o1uwzrtyHLzBTlZb3zNRj+pL+4dSG
f5ifWJRUVPE6NjM6WnBdRYAqF0jXTDdwHkNfX/kgMdIwjpEt+FUgqvG3zeE5h/uH
+oHB3BepRK5fwcHOIqBBHSTEWkx9wSUd+MeMD0WAjoXpGFspgDIQ6RTAm1MkzVes
6Mcb+QKBgQC5b6XLjSvRid2ZSXuZs4Ar3AXJBZXoFKJIOeqdUpFBBzzXFQP/gW4g
JnDF3v+9ky2PC324DKvit8TVrjExgjjallhsc6fiMMfWSI73FCRpB7tSgqxo/PXj
A8WRwpzErU6vE9IuEpmcOa2xOh5IKb7dLutRGCVQrRGe0ZzPr+/OBQ==
-----END RSA PRIVATE KEY-----",
                Certificate = @"-----BEGIN CERTIFICATE-----
MIICqzCCAZOgAwIBAgIILdUBCnV2tOQwDQYJKoZIhvcNAQELBQAwFzEVMBMGA1UEAwwMcm9vdCBT
TkMtVkFWMB4XDTE3MDIwMjAwMDAwMFoXDTE5MDIwMjAwMDAwMFowFDESMBAGA1UEAwwJbG9jYWxo
b3N0MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEAgA8M9RwSOfFeQtqFnXLLC1xyn+Iq
jB95yan6+6BYuyPAruAa8LKnbj4H92AbLJXuEUWLXhLrxckFwbzsW7iaiIHstE1sRBQHzG8kK8HB
ZqaKeFxfad1LmnWUKJRxP0LSbM1p/yFBqmB36hdEBeQhcxqrCKZVQ1wn7ncUWlqEDFQZIb4wcT4G
U9szbY8nMlO4BIOzphlX2QOzQFWWjACSxVAvoo+7AiqAxO4pgBFbQk1iNAlOyhVgJzXEUeMItGZT
JZmSd77KZ+qS0Ksi6N3ar72LtZ8YBb3vDIWU1CmfvievUBhSFCTz8QTIT7/uUyQ40tNQR1kOof7Z
aPMueHsfTwIDAQABMA0GCSqGSIb3DQEBCwUAA4IBAQBm57POTZeXzjVoMif+m/caq53VTXQcFCSL
1qmV4DznCI++9hWdjbXx/e3lrDhW4H4X752r27oixD6Z/SgOl5GgGeGTHyHrXpprJ3C4ZJRpAXYY
T6p1tKTzOCx59zFaIW1PqgnV0cP8lT6oBMFcnzx/dvF07Gk3hfgRreqekypxj1sBdYAElfZ2oKe0
VN6qNwFXaDJ61O0e5JZQ0ygl7EvRmDkefjCleW1g73LBgLDTNWLsHvFALjZSEABOGoJwnYSRO5Eq
J7b8AyuGUGZivZ4zFOpg5oLj83brdmJt2CN+kFAcfALBmCXNKUenqI0KbWHgyq3FZQXcA0nmxX7J
XTas
-----END CERTIFICATE-----",
                IP = "mobile.sncard.ru",
                Port = 2585
            };
        }

        /// <summary>
        /// Сохранение настройки
        /// </summary>
        /// <param name="index">Индекс настройки</param>
        /// <param name="v">Значение настройки</param>
        public void SaveSetting(int index, string v)
        {
            var db = new SettingsDB();
            db.SaveSetting(index, v);
        }

        public override string ToString()
        {
            var res = "Попыток для загрузки : ";
            res += "карты: " + OrderTries + Environment.NewLine;
            res += " новостей : ";
            res += " списка эмитентов : " + IssuerTries + Environment.NewLine;
            res += " карты : " + MapTries;
            res += " информации о карте : " + CardTries + Environment.NewLine;
            res += "Время жизни запроса : " + RequestPeriodLive + Environment.NewLine;
            //res += "Настройки заявки : " + OrderSetting.ToString() + Environment.NewLine;
            return res;
        }
    }
}
