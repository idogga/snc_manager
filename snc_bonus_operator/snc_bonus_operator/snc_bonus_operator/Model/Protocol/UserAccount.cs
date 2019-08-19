using Plugin.FirebasePushNotification;
using snc_bonus_operator.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
//using Plugin.FirebasePushNotification;

namespace snc_bonus_operator.Protocol
{
    public delegate void RootPageDelegate();
    public class UserAccount : MobileSeller
    {
        public event RootPageDelegate UserRegisterHandler;
        public event RootPageDelegate UserUnloggedHandler;
        public event RootPageDelegate UserAuthorizeHandler;
        public event RootPageDelegate UserUnauthorizeHandler;

        public event RootPageDelegate UserReAuthorizeHandler;
        public event RootPageDelegate NeedToAuthorizeHandler;

        /// <summary>
        /// Уникальный номер заказа
        /// </summary>
        public int UniqueOrderNumber { get; set; } = 0;

        /// <summary>
        /// Номер эмитента
        /// </summary>
        public int Issuer { get; set; } = -1;

        /// <summary>
        /// Личный топик пользователя (для FireBase)
        /// </summary>
        public string NotificationUserTopic { get; set; } = "";

        /// <summary>
        /// Пароль
        /// </summary>
        public string PassHash { get; set; } = string.Empty;

        /// <summary>
        /// Номер карты, которая является базовой
        /// </summary>
        public int CardNumber { get; set; } = 0;

        /// <summary>
        /// Последнее действие пользователя
        /// </summary>
        public DateTime LastAuthorise { get; set; } = DateTime.MinValue;
        /// <summary>
        /// Список Администрируемых магазинов
        /// </summary>
        public List<ShopModel> ShopList { get; set; } = new List<ShopModel>();


        public List<Colegue> Stuff { get; set; } = new List<Colegue>();
        /// <summary>
        /// Переводом состояний
        /// </summary>
        public string GetUserStateTranslate(UserStates state)
        {
            string result = "";
            switch (state)
            {
                case (UserStates.Blocked):
                    result = "Заблокирован";
                    break;
                case (UserStates.Deleted):
                    result = "Удален";
                    break;
                case (UserStates.Registered):
                    result = "Зарегестрирован";
                    break;
                case (UserStates.Ungeristered):
                    result = "Не зарегестрирован";
                    break;
            }
            return result;
        }

        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        public void AuthorzedUser()
        {
            MobileStaticVariables.UserStatus = UserStatusEnum.Logged;
            UserAuthorizeHandler?.Invoke();
        }

        /// <summary>
        /// Переавторизация пользователя
        /// </summary>
        public void ReAuthorzedUser()
        {
            MobileStaticVariables.UserStatus = UserStatusEnum.Logged;
            UserReAuthorizeHandler?.Invoke();
        }

        public void NeedToAuthorize()
        {
            MobileStaticVariables.UserStatus = UserStatusEnum.Unlogged;
            NeedToAuthorizeHandler?.Invoke();
        }

        public void UnAuthorized()
        {
            MobileStaticVariables.UserStatus = UserStatusEnum.Unlogged;
            UserUnauthorizeHandler?.Invoke();
        }

        public void RegisterUser()
        {
            MobileStaticVariables.UserStatus = UserStatusEnum.Unlogged;
            UserRegisterHandler?.Invoke();
        }

        /// <summary>
        /// Удаление информации о пользователе
        /// </summary>
        public void DeleteInfo()
        {
            try
            {
                MobileStaticVariables.UserInfo.CardKeys = "";
                MobileStaticVariables.UserInfo.Email = "";
                MobileStaticVariables.UserInfo.GraphicalNumbers = "";
                MobileStaticVariables.UserInfo.Hash = "";
                MobileStaticVariables.UserInfo.LastActionDateTime = DateTime.Now;
                MobileStaticVariables.UserInfo.MobileUserKey = 0;
                MobileStaticVariables.UserInfo.PassHash = "";
                MobileStaticVariables.UserInfo.PhoneNumber = "";
                MobileStaticVariables.UserInfo.RegisterDateTime = DateTime.MinValue;
                MobileStaticVariables.UserInfo.UniqueOrderNumber = 0;
                MobileStaticVariables.UserInfo.UserNickName = "";
                MobileStaticVariables.UserInfo.MobileDeviceKey = 0;
                MobileStaticVariables.UserInfo.UserState = UserStates.Ungeristered;
                MobileStaticVariables.UserInfo.UserType = UserTypes.Client;
                MobileStaticVariables.UserInfo.CardNumber = 0;
                MobileStaticVariables.UserInfo.BirthDate = DateTime.MinValue;
                MobileStaticVariables.UserInfo.CarId = "";
                MobileStaticVariables.UserInfo.Gender = 1;
                MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] = null;
                MobileStaticVariables.UserInfo.ShopList = new List<ShopModel>();
                //DeviceSettings.UserStatus = UserStatusEnum.Unregister;
                CrossFirebasePushNotification.Current.Unsubscribe(MobileStaticVariables.UserInfo.NotificationUserTopic);
                MobileStaticVariables.UserInfo.NotificationUserTopic = "";
                var db = new SettingsDB();
                db.DeleteUserInfo();
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
            finally
            {
                MobileStaticVariables.UserStatus = UserStatusEnum.UnRegister;
            }
            UserUnloggedHandler?.Invoke();
        }

        /// <summary>
        /// Изменить уникальный номер пользователя.
        /// </summary>
        public void ChangeUniqueOrderNumber()
        {
            int numberOrder = (UniqueOrderNumber / 10) + 1;
            int result = DateTime.Now.Second % 10;
            UniqueOrderNumber = numberOrder * 10 + result;
            SaveSetting((int)SettingsEnum.UniqueOrderNumber, UniqueOrderNumber.ToString());
            //WriteToJson();
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
    }
}
