using Newtonsoft.Json;
using snc_bonus_operator.Settings;
using System.Collections.Generic;

namespace snc_bonus_operator.Protocol
{
    public enum LimitationActionType
    {
        /// <summary>
        /// Нет ограничения
        /// </summary>
        None = 0,
        /// <summary>
        /// Не доступен (не нажимается кнопка, отображается "функция временно не доступна" и т.п.)
        /// </summary>
        Enabled = 1,
        /// <summary>
        /// Скрыт от пользователя
        /// </summary>
        Visible = 2,
        /// <summary>
        /// Минимальное значение
        /// </summary>
        MinValue = 3,
        /// <summary>
        /// Максимальное значение
        /// </summary>
        MaxValue = 4,
    }
    public enum LimitationObject
    {
        CommonObject = 0,
        OrderButton = 1,
        NewsButton = 2,
        CardsButton = 3,
        IssuerSms = 4,
    }
    /// <summary>
    /// Ограничения мобильного приложения
    /// </summary>
    public class Limitation
    {
        /// <summary>
        /// Список ограничений
        /// </summary>
        [JsonProperty("LT")]
        public List<LimitationItem> Limitations { get; set; } = new List<LimitationItem>();

        public void LoadLimitation()
        {
            var rules = new MobileRules();

            foreach (var limit in Limitations)
            {
                switch (limit.Id)
                {
                    case (LimitationObject.OrderButton):
                        if (limit.Action == LimitationActionType.Enabled)
                        {
                            rules.OrderButtonRule = bool.Parse(limit.Value);
                        }
                        break;
                    case (LimitationObject.NewsButton):
                        if (limit.Action == LimitationActionType.Enabled)
                        {
                            rules.NewsButtonRule = bool.Parse(limit.Value);
                        }
                        break;
                    case (LimitationObject.CardsButton):
                        if (limit.Action == LimitationActionType.Enabled)
                        {
                            rules.CardsButtonRule = bool.Parse(limit.Value);
                        }
                        break;
                    case (LimitationObject.IssuerSms):
                        if (limit.Action == LimitationActionType.Enabled)
                        {
                            rules.UseSms = bool.Parse(limit.Value);
                        }
                        break;
                    default:
                        break;
                }
            }
            rules.SaveSettings();
        }

        private class MobileRules
        {
            public bool OrderButtonRule { get; set; } = true;
            public bool NewsButtonRule { get; set; } = true;
            public bool CardsButtonRule { get; set; } = true;
            public bool UseSms { get; set; } = false;

            public void SaveSettings()
            {
                MobileStaticVariables.UserAppSettings.CardButtonWork = CardsButtonRule;
                MobileStaticVariables.UserAppSettings.NewsButtonWork = NewsButtonRule;
                MobileStaticVariables.UserAppSettings.OrderButtonWork = OrderButtonRule;
                MobileStaticVariables.UserAppSettings.UseSMSIssuer = UseSms;

                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.OrderButtonWork, MobileStaticVariables.UserAppSettings.OrderButtonWork.ToString());
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.NewsButtonWork, MobileStaticVariables.UserAppSettings.NewsButtonWork.ToString());
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.CardButtonWork, MobileStaticVariables.UserAppSettings.CardButtonWork.ToString());

                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.UseSmsIssuer, MobileStaticVariables.UserAppSettings.UseSMSIssuer.ToString());

            }
        }
    }
    /// <summary>
    /// Запись из таблицы ограничений
    /// </summary>
    public class LimitationItem
    {
        /// <summary>
        /// Номер ограничения
        /// </summary>
        [JsonProperty("ID")]
        public LimitationObject Id { get; set; } = LimitationObject.CommonObject;

        /// <summary>
        /// Действие ограничения
        /// </summary>
        [JsonProperty("LA")]
        public LimitationActionType Action { get; set; } = LimitationActionType.None;
        /// <summary>
        /// Значение ограничения
        /// </summary>
        [JsonProperty("VA")]
        public string Value { get; set; } = "";
    }
}
