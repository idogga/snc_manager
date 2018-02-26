using Newtonsoft.Json;
using snc_bonus_operator.Settings;
using System;

namespace snc_bonus_operator.Protocol
{
    /// <summary>
    /// Состояния устройства
    /// </summary>
    public enum DeviceStates
    {
        New,
        Registered,
        Blocked,
    }
    [Flags]
    public enum BitSetModes
    {
        /// <summary>
        /// Ничего не отмечено
        /// </summary>
        None = 0,
        /// <summary>
        /// Оповещения по почте
        /// </summary>
        EmailMessage = 1 << 0,
        /// <summary>
        /// Системные оповещения на устройство
        /// </summary>
        NotificationMessage = 1 << 1,
        /// <summary>
        /// Рассылка оповещений по смс
        /// </summary>
        PhoneMessage = 1 << 2,
    }
    public class DeviceInfo : RequestResult
    {
        /// <summary>
        /// Ключ пользователя
        /// </summary>
        [JsonProperty("UI")]
        public int UserID { get; set; } = 0;
        /// <summary>
        /// Ключ устройства в базе
        /// </summary>
        [JsonProperty("DK")]
        public int DeviceKey { get; set; } = 0;
        /// <summary>
        /// Название устройства
        /// </summary>
        [JsonProperty("DN")]
        public string DeviceName { get; set; } = "";
        /// <summary>
        /// Версия мобильного приложения на устройстве
        /// </summary>
        [JsonProperty("AV")]
        public string AppVersion { get; set; } = "";
        /// <summary>
        /// IMEI устройства
        /// </summary>
        [JsonProperty("IM")]
        public string Imei { get; set; } = "";
        /// <summary>
        /// Состояние устройства
        /// </summary>
        [JsonProperty("DS")]
        public DeviceStates DeviceState { get; set; } = DeviceStates.New;
        /// <summary>
        /// Уникальный идентификатор устройства для рассылки оповещений
        /// </summary>
        [JsonProperty("NF")]
        public string NotificationToken { get; set; }
        /// <summary>
        /// Битовые настройки пользователя в приложении (каналы рассылки уведомлений и т.п.)
        /// </summary>
        [JsonProperty("BS")]
        public long BitSetting { get; set; } = (int)BitSetModes.EmailMessage;

        public void Parse(string json)
        {
            try
            {
                var deserialized = JsonConvert.DeserializeObject<DeviceInfo>(json);
                UserID = deserialized.UserID;
                DeviceKey = deserialized.DeviceKey;
                DeviceName = deserialized.DeviceName;
                AppVersion = deserialized.AppVersion;
                Imei = deserialized.Imei;
                DeviceState = deserialized.DeviceState;
                BitSetting = deserialized.BitSetting;
                MobileStaticVariables.UserAppSettings.UseEmailUser = ((BitSetting & (long)BitSetModes.EmailMessage) == (long)BitSetModes.EmailMessage);
                MobileStaticVariables.UserAppSettings.UseSMSUser = ((BitSetting & (long)BitSetModes.PhoneMessage) == (long)BitSetModes.PhoneMessage);
                MobileStaticVariables.UserAppSettings.ShowNotifications = ((BitSetting & (long)BitSetModes.NotificationMessage) == (long)BitSetModes.NotificationMessage);

                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.UseSmsUser, MobileStaticVariables.UserAppSettings.UseSMSUser.ToString());
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.ShowNotifications, MobileStaticVariables.UserAppSettings.ShowNotifications.ToString());
                MobileStaticVariables.UserAppSettings.SaveSetting((int)SettingsEnum.UseEmailUser, MobileStaticVariables.UserAppSettings.UseEmailUser.ToString());
                
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }
    }
}