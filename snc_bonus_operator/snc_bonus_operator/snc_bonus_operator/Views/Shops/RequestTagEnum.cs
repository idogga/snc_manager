namespace snc_bonus_operator
{
    public enum RequestTagEnum
    {
        /// <summary>
        /// Загрузка данных обо всей азс
        /// </summary>
        CheckAzs,

        /// <summary>
        /// загрузка данных о конкретном шланге
        /// </summary>
        CheckNozzle,

        /// <summary>
        /// Расчет продажи
        /// </summary>
        CalculateFuelSale,

        /// <summary>
        /// Колличество бонусов
        /// </summary>
        BonusCount,

        /// <summary>
        /// Ограничения со стороны ПЦ и службы
        /// </summary>
        Limitations,

        /// <summary>
        /// Новости компании
        /// </summary>
        News,

        /// <summary>
        /// Список акций
        /// </summary>
        Sales,

        /// <summary>
        /// Добавление телефона / почтового адреса
        /// </summary>
        ReqPhoneEmail,

        /// <summary>
        /// Информация о профиле пользователя
        /// </summary>
        Profile,

        /// <summary>
        /// Установка состояния пользовательского устройства
        /// </summary>
        SetDevice,

        /// <summary>
        /// Передача ключевого слова
        /// </summary>
        KeyWord,

        /// <summary>
        /// Передача чека о совершенной продаже
        /// </summary>
        GetBonusTransaction,

        /// <summary>
        /// Запрос чека по совершенной продаже
        /// </summary>
        GetBonusBill,

        /// <summary>
        /// Все доступные пользовательские программы
        /// </summary>
        GetPrograms,

        /// <summary>
        /// Установка программ
        /// </summary>
        SetPrograms,

        /// <summary>
        /// Установка программ, которые уже были у пользователя
        /// </summary>
        SetOldPrograms,

        /// <summary>
        /// Все пользовательские программы
        /// </summary>
        GetMyBonusPrograms,

        /// <summary>
        /// Передать логи
        /// </summary>
        SetLoggs,

        /// <summary>
        /// Пользовательские транзакции
        /// </summary>
        BonusFilteredTransactions,

        /// <summary>
        /// Пользовательское приложение
        /// </summary>
        BonusApp,

        /// <summary>
        /// Блокировка карты
        /// </summary>
        Block,

        /// <summary>
        /// Изменение реквизитов карты
        /// </summary>
        ChangeRequisites,

        /// <summary>
        /// Регистрация нового пользователя
        /// </summary>
        RegNew,

        /// <summary>
        /// Подтверждение регистрации
        /// </summary>
        Register,

        /// <summary>
        /// Запрос дополнительных товаров
        /// </summary>
        GetAvailableGoods,

        /// <summary>
        /// запрос отфильтрованых азс
        /// </summary>
        FilteredShops,

        /// <summary>
        /// Блокировка менеджера
        /// </summary>
        BlockManager,
        RegManager,
        GetMyCrew,
        /// <summary>
        /// Запрос статусов АЗС
        /// </summary>
        CheckAzsSeller,
        /// <summary>
        /// Обновление пароля
        /// </summary>
        SellerReloadPassword,
        /// <summary>
        /// Запрос о конкретной ТО
        /// </summary>
        GetAzsInfo,
        /// <summary>
        /// Запрос всех транзакций
        /// </summary>
        AllTransactionsSeller,

        /// <summary>
        /// Запрос всех мобильных покупок на выбранных ТО
        /// </summary>
        AllMobileTransactions
    }
}
