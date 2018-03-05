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

#if DEBUGARTYOM
        public int DebugPort { get; set; } = 2582;
#endif

        public ConnectionSettings()
        {
            Certificates[(int)CertificateType.BASE] = new CertificateKey()
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
