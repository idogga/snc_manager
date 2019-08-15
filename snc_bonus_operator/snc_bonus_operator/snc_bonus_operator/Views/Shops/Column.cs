using System.Collections.Generic;
using System.Linq;

namespace snc_bonus_operator
{
    /// <summary>
    /// Представление стороны(колонки)
    /// </summary>
    public class Column
    {
        /// <summary>
        /// Название колонки
        /// </summary>
        public string Name = string.Empty;

        /// <summary>
        /// Уникальный номер
        /// </summary>
        public int Id = 0;

        /// <summary>
        /// Состояние колонки
        /// </summary>
        public PumpStateEnum State = PumpStateEnum.PUMP_STATE_FREE;

        /// <summary>
        /// пистолеты
        /// </summary>
        private List<Nozzle> _hoses = new List<Nozzle>();

        public Column(int id)
        {
            Id = id;
            Name = id.ToString();
        }

        /// <summary>
        /// Добавление нового пистолета
        /// </summary>
        /// <param name="hose"></param>
        public void AddHose(Nozzle hose)
        {
            if (!_hoses.Any(x => x.Id == hose.Id))
            {
                _hoses.Add(hose);
            }
        }

        /// <summary>
        /// Обновление пистолета
        /// </summary>
        /// <param name="hose"></param>
        public bool UpdateHose(Nozzle hose)
        {
            var oldHose = _hoses.FirstOrDefault(x => x.Id == hose.Id);
            if (oldHose == null)
            {
                _hoses.Add(hose);
                return true;
            }
            else
            {
                if (oldHose.State != hose.State)
                {
                    oldHose.State = hose.State;
                    return true;
                }
                oldHose.Price = hose.Price;
                return false;
            }
        }

        /// <summary>
        /// Установка статуса
        /// </summary>
        /// <param name="status">Статус</param>
        internal void SetStatus(PumpStateEnum status)
        {
            if (status == PumpStateEnum.PUMP_STATE_FREE)
                return;
            if (status == PumpStateEnum.PUMP_STATE_ACTIVATED)
            {
                if (State == PumpStateEnum.PUMP_STATE_FREE)
                {
                    State = status;
                    return;
                }
            }
            State = status;
        }

        /// <summary>
        /// Список шлангов 
        /// </summary>
        /// <returns></returns>
        internal List<Nozzle> GetHoses()
        {
            return _hoses;
        }
    }
}