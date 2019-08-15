using snc_bonus_operator.Protocol;
using System.Collections.Generic;
using System.Linq;

namespace snc_bonus_operator
{
    /// <summary>
    /// Работа со списком магазинов
    /// </summary>
    public class ShopCollection
    {
        private List<AzsInfo> _shopListBasic = new List<AzsInfo>();

        #region Публичные методы

        /// <summary>
        /// Вернуть все АЗС
        /// </summary>
        /// <returns></returns>
        public List<AzsInfo> GetAll()
        {
            return _shopListBasic;
        }

        /// <summary>
        /// Колличество магазинов в базе
        /// </summary>
        /// <returns></returns>
        public int GetListCount()
        {
            return _shopListBasic.Where(x => x.IsVisible).Count();
        }

        /// <summary>
        /// Загрузка списка азс
        /// </summary>
        /// <param name="infos"></param>
        public void SetList(List<AzsInfo> infos)
        {
            foreach (var azs in infos)
            {
                Add(azs);
            }
        }

        /// <summary>
        /// Поиск азс
        /// </summary>
        /// <param name="azsid"></param>
        /// <returns></returns>
        public AzsInfo FindAzs(int azsid)
        {
            return _shopListBasic.FirstOrDefault(x => x.AzsId == azsid);
        }

        /// <summary>
        /// Поиск азс
        /// </summary>
        /// <param name="AzsId"></param>
        /// <param name="issuerKey"></param>
        /// <returns></returns>
        public AzsInfo FindAzs(int AzsId, int issuerKey)
        {
            return _shopListBasic.FirstOrDefault(a => ((a.AzsId == AzsId)
                    && ((a.IssuerKey == issuerKey))));
        }

        public void LoadAzsFromServer()
        {
            var deserialized = MobileStaticVariables.WebUtils.SendMobileRequest<List<AzsInfo>>(RequestTagEnum.FilteredShops, GetHashTable());
            SetList(deserialized);
        }

        #endregion

        #region Приватные методы

        /// <summary>
        /// Возврат хэш-таблицы
        /// </summary>
        /// <returns>хэш-таблица</returns>
        private List<AzsTableHashItem> GetHashTable()
        {
            return _shopListBasic.Select(x => (AzsTableHashItem)x).ToList();
        }

        private List<AzsInfo> OrderByKoef(List<AzsInfo> list)
        {
            var temp = list.OrderByDescending(x => x.Koeff).ThenBy(y => y.Title);
            //#if DEBUG 
            //            foreach (var azs in temp)
            //            {
            //                Logger.WriteLine(azs.Title + " : " + azs.Koeff);
            //            }            
            //#endif
            return new List<AzsInfo>(temp);
        }

        /// <summary>
        /// Добавление одной азс в список
        /// </summary>
        /// <param name="azsInfo"></param>
        private void Add(AzsInfo azsInfo)
        {
            azsInfo.IsVisible = true;
            var old = _shopListBasic.FirstOrDefault(x => x.AzsId == azsInfo.AzsId);
            if (old == null)
            {
                _shopListBasic.Add(azsInfo);
            }
            else
            {
                azsInfo.IsFavourite = old.IsFavourite;
                _shopListBasic.Remove(old);
                _shopListBasic.Add(azsInfo);
            }
        }
        #endregion
    }
}