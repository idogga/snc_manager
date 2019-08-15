using snc_bonus_operator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ssnc_bonus_operator
{
    public delegate void UpdateColumnsDelegate(Column column);
    /// <summary>
    /// Контроллер азс
    /// </summary>
    public class AzsController
    {
        public event UpdateColumnsDelegate UpdateUpdateColumns;
        public event UpdateColumnsDelegate ErrorLoading;

        private List<Column> _columns = new List<Column>();
        private bool _isVisibleColumn = true;
        private AzsPriceBook _azsPriceBook;
        private ShopView _azs;


        public AzsController(VisualElement element, ShopView azs)
        {
            _azs = azs;
        }

        public void StartMonitor()
        {
            _isVisibleColumn = true;
            Device.StartTimer(TimeSpan.FromSeconds(1), LoadInfo);
            Device.StartTimer(TimeSpan.FromSeconds(15), CheckAzsData);
        }

        public void StopMonitor()
        {
            _isVisibleColumn = false;
        }

        /// <summary>
        /// Проверка есть ли данные об азс
        /// </summary>
        /// <returns></returns>
        private bool CheckAzsData()
        {
            if (_columns.Count == 0)
                ErrorLoading?.Invoke(null);
            return false;
        }

        /// <summary>
        /// Загрузка данных со службы
        /// </summary>
        /// <returns></returns>
        private bool LoadInfo()
        {
            Task.Factory.StartNew(() =>
            {
                var check = new AzsCheck() { CodAzs = _azs.ShopModel.ShopKey, MobileDeviceKey = MobileStaticVariables.UserInfo.MobileDeviceKey };
                _azsPriceBook = MobileStaticVariables.WebUtils.SendMobileRequest<AzsPriceBook>(RequestTagEnum.CheckAzs, check);
                Update();
            });
            return _isVisibleColumn;
        }

        /// <summary>
        /// Обновление колонок
        /// </summary>
        private void Update()
        {
            foreach (var column in _azsPriceBook.Book)
            {
                var col = _columns.FirstOrDefault(x => x.Id == column.DispenserNumber);
                var updateCol = false;
                if (col == null)
                {
                    updateCol = true;
                    col = new Column(column.DispenserNumber);
                    _columns.Add(col);
                    foreach (var hose in column.NozzleFuelTypes)
                    {
                        var price = _azsPriceBook.Prices.FirstOrDefault(x => x.F_Code == hose.F_Code & x.FF_Code == hose.FF_Code);
                        col.AddHose(new Nozzle(hose.Nozzle, hose.F_Code, hose.FF_Code, price.FuelName, hose.Status, price.Price));
                        col.SetStatus(hose.Status);
                    }
                }
                else
                {
                    col.State = PumpStateEnum.PUMP_STATE_FREE;
                    foreach (var hose in column.NozzleFuelTypes)
                    {
                        var price = _azsPriceBook.Prices.FirstOrDefault(x => x.F_Code == hose.F_Code & x.FF_Code == hose.FF_Code);
                        updateCol |= col.UpdateHose(new Nozzle(hose.Nozzle, hose.F_Code, hose.FF_Code, price.FuelName, hose.Status, price.Price));
                        col.SetStatus(hose.Status);
                    }
                }
                if (updateCol)
                {
                    Device.BeginInvokeOnMainThread(() =>
                        UpdateUpdateColumns?.Invoke(col));
                }
            }
        }

        /// <summary>
        /// Возвращает список колонок
        /// </summary>
        /// <param name="columnId">номер колонки</param>
        /// <returns></returns>
        public Column GetColumn(int columnId)
        {
            return _columns.FirstOrDefault(x => x.Id == columnId);
        }
    }
}