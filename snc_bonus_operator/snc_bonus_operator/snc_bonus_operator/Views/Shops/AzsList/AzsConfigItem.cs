using System;
using System.Collections.Generic;

namespace snc_bonus_operator
{
    /// <summary>
    /// Запись с параметрами АЗС
    /// </summary>
    public class AzsConfigItem
    {
        /// <summary>
        /// Код АЗС
        /// </summary>
        public int Cod_Azs { get; private set; } = -1;

        /// <summary>
        /// Список шлангов на АЗС. Ну или просто список <see cref="NozzleConfigInfo"/> элементов
        /// </summary>
        public List<NozzleConfigInfo> NozzleInfo = new List<NozzleConfigInfo>();

        /// <summary>
        /// Дата последнего обновления
        /// </summary>
        public DateTime LastUpdate = DateTime.MinValue;

        public AzsConfigItem(int cOD_AZS)
        {
            Cod_Azs = cOD_AZS;
        }

        public override string ToString()
        {
            var result = string.Format("АЗС [{0}]", Cod_Azs);
            foreach (var item in NozzleInfo)
            {
                result += Environment.NewLine;
                result += "\t" + item.ToString();
            }
            return result;
        }
    }
}
