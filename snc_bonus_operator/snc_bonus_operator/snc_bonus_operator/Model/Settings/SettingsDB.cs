using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Realms;
using System.Text;
using System.Threading.Tasks;
using snc_bonus_operator.Protocol;
using Xamarin.Forms;
using snc_bonus_operator.Interfaces;

namespace snc_bonus_operator.Settings
{
    public class SettingsDB
    {
        public Realm SettingsDataBase;
        byte[] key = new byte[64];

        public SettingsDB()
        {
            var imei = Encoding.UTF8.GetBytes(DependencyService.Get<IDevice>().GetIdentifier());
            key = new byte[64]{
                0x75, 0x02, 0x18, 0x75, 0x34, 0x06, 0x07, 0x08,
                0x11, 0x32, 0x13, imei[0], 0x15, 0x25, 0x17, 0x18,
                imei[imei.Length/2], 0x02, 0x23, 0x24, 0x73, 0x26, 0x27, 0x28,
                0x25, 0x32, 0x32, 0x34, 0x12, imei[4], 0x08, 0x38,
                0x68, 0x54, 0x41, 0x18, 0x45, 0x46, 0x32, 0x48,
                0x51, imei[1], 0x53, 0x54, 0x55, 0x25, 0x57, 0x51,
                0x61, 0x62, 0x75, 0x64, 0x65, 0x66, 0x67, 0x63,
                0x68, 0x72, 0x73, 0x25, 0x25, 0x76, 0x11, imei[imei.Length-1]
            };
            var config = new RealmConfiguration("Settings.realm")
            {
                EncryptionKey = key
            };
            SettingsDataBase = Realm.GetInstance(config);
        }

        public void DeleteUserInfo()
        {
            try
            {
                var config = new RealmConfiguration("Settings.realm")
                {
                    EncryptionKey = key
                };
                SettingsDataBase = Realm.GetInstance(config);
                List<SettingsTable> list = new List<SettingsTable>();
                list = SettingsDataBase.All<SettingsTable>().ToList() ?? new List<SettingsTable>();
                // Удаление всех личных пользовательских данных из таблицы
                foreach (var item in list)
                {
                    using (var trans = SettingsDataBase.BeginWrite())
                    {
                        switch (item.Key)
                        {
                            case (int)SettingsEnum.MobileUserKey:
                            case (int)SettingsEnum.MobileDeviceKey:
                            case (int)SettingsEnum.UserName:
                            case (int)SettingsEnum.PhoneNumber:
                            case (int)SettingsEnum.Email:
                            case (int)SettingsEnum.UserState:
                            case (int)SettingsEnum.RegisterDateTime:
                            case (int)SettingsEnum.LastActionDateTime:
                            case (int)SettingsEnum.UserType:
                            case (int)SettingsEnum.Hash:
                            case (int)SettingsEnum.CardKeys:
                            case (int)SettingsEnum.GraphicalNumbers:
                            case (int)SettingsEnum.PassHash:
                            case (int)SettingsEnum.UniqueOrderNumber:
                            case (int)SettingsEnum.UserTopicName:
                            case (int)SettingsEnum.PRIVATE_USER_PRIVATE_IP:
                            case (int)SettingsEnum.PRIVATE_USER_PRIVATE_PORT:
                            case (int)SettingsEnum.PRIVATE_USER_CERTIFICATE:
                            case (int)SettingsEnum.PRIVATE_USER_PRIVATE_CERTIFICATE:
                            case (int)SettingsEnum.UserCardNumber:
                            case (int)SettingsEnum.Gender:
                            case (int)SettingsEnum.CarId:
                            case (int)SettingsEnum.BirthDate:
                            case (int)SettingsEnum.UserNickName:
                            case (int)SettingsEnum.ShopList:
                            case (int)SettingsEnum.KeyString:
                                SettingsDataBase.Remove(item);
                                trans.Commit();
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        public void LoadSettings()
        {
            try
            {
                var config = new RealmConfiguration("Settings.realm")
                {
                    EncryptionKey = key
                };
                SettingsDataBase = Realm.GetInstance(config);
                List<SettingsTable> list = new List<SettingsTable>();
                list = SettingsDataBase.All<SettingsTable>().ToList() ?? new List<SettingsTable>();
                foreach (var item in list)
                {
                    try
                    {
                        switch (item.Key)
                        {
                            case (int)SettingsEnum.CurrentLanguage:
                                MobileStaticVariables.UserAppSettings.CurrentLanguage = item.Value;
                                break;
                            case (int)SettingsEnum.IsDataBaseLoad:
                                MobileStaticVariables.UserAppSettings.IsDataBaseLoad = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.Stuff:
                                MobileStaticVariables.UserInfo.Stuff = JsonConvert.DeserializeObject<List<Colegue>>(item.Value);
                                break;
                            case (int)SettingsEnum.IsIntroShown:
                                MobileStaticVariables.UserAppSettings.IsIntroShown = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.IsUseFingerprint:
                                MobileStaticVariables.UserAppSettings.IsUseFingerprint = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.LastUpdate:
                                MobileStaticVariables.UserAppSettings.LastUpdate = DateTime.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.LastUpdateSupportPack:
                                MobileStaticVariables.UserAppSettings.LastUpdateSupportPack = DateTime.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.NumberOfLoadApp:
                                MobileStaticVariables.UserAppSettings.NumberOfLoadApp = int.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.SaveOrderMemory:
                                MobileStaticVariables.UserAppSettings.SaveOrderMemory = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.ShowNotifications:
                                MobileStaticVariables.UserAppSettings.ShowNotifications = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.UseVibration:
                                MobileStaticVariables.UserAppSettings.UseVibration = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.MobileUserKey:
                                MobileStaticVariables.UserInfo.MobileUserKey = int.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.MobileDeviceKey:
                                MobileStaticVariables.UserInfo.MobileDeviceKey = int.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.UserName:
                                MobileStaticVariables.UserInfo.Name = item.Value;
                                break;
                            case (int)SettingsEnum.PhoneNumber:
                                MobileStaticVariables.UserInfo.PhoneNumber = item.Value;
                                break;
                            case (int)SettingsEnum.Email:
                                MobileStaticVariables.UserInfo.Email = item.Value;
                                break;
                            case (int)SettingsEnum.UserState:
                                MobileStaticVariables.UserInfo.UserState = (UserStates)int.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.RegisterDateTime:
                                MobileStaticVariables.UserInfo.RegisterDateTime = DateTime.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.LastActionDateTime:
                                MobileStaticVariables.UserInfo.LastActionDateTime = DateTime.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.UserType:
                                MobileStaticVariables.UserInfo.UserType = (UserTypes)int.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.Hash:
                                MobileStaticVariables.UserInfo.Hash = item.Value;
                                break;
                            case (int)SettingsEnum.CardKeys:
                                MobileStaticVariables.UserInfo.CardKeys = item.Value;
                                break;
                            case (int)SettingsEnum.GraphicalNumbers:
                                MobileStaticVariables.UserInfo.GraphicalNumbers = item.Value;
                                break;
                            case (int)SettingsEnum.PassHash:
                                MobileStaticVariables.UserInfo.PassHash = item.Value;
                                break;
                            case (int)SettingsEnum.UniqueOrderNumber:
                                MobileStaticVariables.UserInfo.UniqueOrderNumber = int.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.ScreenHeight:
                                MobileStaticVariables.UserAppSettings.ScreenHeight = double.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.ScreenWidth:
                                MobileStaticVariables.UserAppSettings.ScreenWidth = double.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.PRIVATE_USER_PRIVATE_CERTIFICATE:
                                {
                                    if (MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] == null)
                                        MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] = new CertificateKey();
                                    MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER].PrivateKey = item.Value;
                                }
                                break;
                            case (int)SettingsEnum.PRIVATE_USER_CERTIFICATE:
                                {
                                    if (MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] == null)
                                        MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] = new CertificateKey();
                                    MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER].Certificate = item.Value;
                                }
                                break;
                            case (int)SettingsEnum.PRIVATE_USER_PRIVATE_IP:
                                {
                                    if (MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] == null)
                                        MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] = new CertificateKey();
                                    MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER].IP = item.Value;
                                }
                                break;
                            case (int)SettingsEnum.PRIVATE_USER_PRIVATE_PORT:
                                {
                                    if (MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] == null)
                                        MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER] = new CertificateKey();
                                    MobileStaticVariables.ConectSettings.Certificates[(int)CertificateType.PRIVATE_USER].Port = int.Parse(item.Value);
                                }
                                break;
                            case (int)SettingsEnum.UserTopicName:
                                MobileStaticVariables.UserInfo.NotificationUserTopic = item.Value;
                                //if (MobileStaticVariables.UserAppSettings.ShowNotifications)
                                //    CrossFirebasePushNotification.Current.Subscribe(MobileStaticVariable.UserInfo.NotificationUserTopic);
                                break;
                            case (int)SettingsEnum.UserCardNumber:
                                MobileStaticVariables.UserInfo.CardNumber = int.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.Gender:
                                MobileStaticVariables.UserInfo.Gender = int.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.CarId:
                                MobileStaticVariables.UserInfo.CarId = item.Value;
                                break;
                            case (int)SettingsEnum.BirthDate:
                                MobileStaticVariables.UserInfo.BirthDate = DateTime.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.UserNickName:
                                MobileStaticVariables.UserInfo.UserNickName = item.Value;
                                break;
                            case (int)SettingsEnum.OrderButtonWork:
                                MobileStaticVariables.UserAppSettings.OrderButtonWork = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.NewsButtonWork:
                                MobileStaticVariables.UserAppSettings.NewsButtonWork = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.CardButtonWork:
                                MobileStaticVariables.UserAppSettings.CardButtonWork = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.UseSmsUser:
                                MobileStaticVariables.UserAppSettings.UseSMSUser = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.UseSmsIssuer:
                                MobileStaticVariables.UserAppSettings.UseSMSIssuer = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.UseEmailUser:
                                MobileStaticVariables.UserAppSettings.UseEmailUser = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.IsDeviceBlock:
                                MobileStaticVariables.UserAppSettings.IsDeviceBlock = bool.Parse(item.Value);
                                break;
                            case (int)SettingsEnum.ShopList:
                                MobileStaticVariables.UserInfo.ShopList = JsonConvert.DeserializeObject<List<ShopModel>>(item.Value);
                                break;

                            case (int)SettingsEnum.IssuerTitle:
                                MobileStaticVariables.MainIssuer.IssuerTitle = item.Value;
                                break;
                            case (int)SettingsEnum.Address:
                                MobileStaticVariables.MainIssuer.Address = item.Value;
                                break;
                            case (int)SettingsEnum.Description:
                                MobileStaticVariables.MainIssuer.Description = item.Value;
                                break;
                            case (int)SettingsEnum.Link:
                                MobileStaticVariables.MainIssuer.Link = item.Value;
                                break;
                            case (int)SettingsEnum.MyProfileLink:
                                MobileStaticVariables.MainIssuer.MyProfileLink = item.Value;
                                break;
                            case (int)SettingsEnum.Phone:
                                MobileStaticVariables.MainIssuer.Phone = item.Value;
                                break;
                            case (int)SettingsEnum.EmailIssuer:
                                MobileStaticVariables.MainIssuer.Email = item.Value;
                                break;
                            case (int)SettingsEnum.MainHexColor:
                                MobileStaticVariables.MainIssuer.MainHexString = item.Value;
                                break;
                            case (int)SettingsEnum.Currency:
                                MobileStaticVariables.MainIssuer.Currency = item.Value;
                                break;
                            case (int)SettingsEnum.BackgroundHexString:
                                MobileStaticVariables.MainIssuer.BackgroundHexString = item.Value;
                                break;
                            case (int)SettingsEnum.LettersHexString:
                                MobileStaticVariables.MainIssuer.LettersHexString = item.Value;
                                break;
                            case (int)SettingsEnum.LinkHexString:
                                MobileStaticVariables.MainIssuer.LinkHexString = item.Value;
                                break;
                            case (int)SettingsEnum.HeaderHexString:
                                MobileStaticVariables.MainIssuer.HeaderHexString = item.Value;
                                break;
                            case (int)SettingsEnum.SelectHexString:
                                MobileStaticVariables.MainIssuer.SelectHexString = item.Value;
                                break;
                            case (int)SettingsEnum.SublettersHexString:
                                MobileStaticVariables.MainIssuer.SublettersHexString = item.Value;
                                break;
                            default:
                                break;
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        public void SaveSetting(int index, string value)
        {
            try
            {
                var config = new RealmConfiguration("Settings.realm")
                {
                    EncryptionKey = key
                };
                SettingsDataBase = Realm.GetInstance(config);
                List<SettingsTable> list = new List<SettingsTable>();
                var setting = SettingsDataBase.All<SettingsTable>().FirstOrDefault(x => x.Key == index) ?? new SettingsTable() { Key = index, Value = value };
                SettingsDataBase.Write(() =>
                {
                    setting.Value = value;
                    SettingsDataBase.Add(setting);
                });
            }
            catch (Exception ex)
            {
                Logger.WriteError(ex);
            }
        }

        public List<string> GetLoggedPerson()
        {
            var result = new List<string>();
            var config = new RealmConfiguration("Settings.realm")
            {
                EncryptionKey = key
            };
            SettingsDataBase = Realm.GetInstance(config);
            List<SettingsTable> list = new List<SettingsTable>();
            list = SettingsDataBase.All<SettingsTable>().ToList() ?? new List<SettingsTable>();
            var personlistJson = list.FirstOrDefault(x => x.Key == (int)SettingsEnum.PersonList);
            if (personlistJson != null)
            {
                if (personlistJson.Value != null && personlistJson.Value != "")
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<List<string>>(personlistJson.Value);
                    }
                    catch
                    {
                        result = new List<string>();
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Добавление зарегестрированного пользователя
        /// </summary>
        /// <param name="Name"></param>
        public void AddLoggedPerson(string Name)
        {
            var result = new List<string>();
            var config = new RealmConfiguration("Settings.realm")
            {
                EncryptionKey = key
            };
            SettingsDataBase = Realm.GetInstance(config);
            List<SettingsTable> list = new List<SettingsTable>();
            list = SettingsDataBase.All<SettingsTable>().ToList() ?? new List<SettingsTable>();
            var personlistJson = list.FirstOrDefault(x => x.Key == (int)SettingsEnum.PersonList);
            if (personlistJson != null)
            {
                if (personlistJson.Value != null && personlistJson.Value != "")
                {
                    try
                    {
                        result = JsonConvert.DeserializeObject<List<string>>(personlistJson.Value);
                    }
                    catch
                    {
                        result = new List<string>();
                    }
                }
            }
            if (!result.Contains(Name))
            {
                result.Add(Name);
                var value = JsonConvert.SerializeObject(result);
                SaveSetting((int)SettingsEnum.PersonList, value);
            }
        }
    }
}
