using System;
using System.Globalization;
using Xamarin.Forms;

namespace snc_bonus_operator.Shops
{
    class ShopSettingsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var settings = value as ShopSettingsEnum?;
            if (!settings.HasValue)
                return "Нет сервисов";
            var result = "";
            if(settings.Value == ShopSettingsEnum.None)
                return "Нет сервисов";
            if (IsVariant(settings.Value, ShopSettingsEnum.BankCard))
                result += "Расчет банковскими картами" + Environment.NewLine;
            if (IsVariant(settings.Value, ShopSettingsEnum.CarParkService))
                result += "Автостоянка" + Environment.NewLine;
            if (IsVariant(settings.Value, ShopSettingsEnum.CarWashService))
                result += "Автомойка" + Environment.NewLine;
            if (IsVariant(settings.Value, ShopSettingsEnum.Cash))
                result += "Расчет наличными" + Environment.NewLine;
            if (IsVariant(settings.Value, ShopSettingsEnum.FoodService))
                result += "Покупка еды" + Environment.NewLine;
            if (IsVariant(settings.Value, ShopSettingsEnum.FuelSellingService))
                result += "Заправка бензином" + Environment.NewLine;
            if (IsVariant(settings.Value, ShopSettingsEnum.GasSellingService))
                result += "Заправка газом" + Environment.NewLine;
            if (IsVariant(settings.Value, ShopSettingsEnum.GoodSellingService))
                result += "Продажа товаров" + Environment.NewLine;
            if (IsVariant(settings.Value, ShopSettingsEnum.HotelService))
                result += "Гостиница" + Environment.NewLine;
            if (IsVariant(settings.Value, ShopSettingsEnum.MobileApp))
                result += "Расчет через мобильное приложение" + Environment.NewLine;
            result.Remove(result.Length-2);
            return result;
        }

        private bool IsVariant(ShopSettingsEnum value, ShopSettingsEnum bankCard)
        {
            return (value & bankCard) == (int)ShopSettingsEnum.None;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double divider;

            if (!Double.TryParse(parameter as string, out divider))
                divider = 1;

            return ((double)(int)value) / divider;
        }
    }
}
