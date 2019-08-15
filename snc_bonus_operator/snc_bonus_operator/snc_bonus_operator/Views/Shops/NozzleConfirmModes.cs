namespace snc_bonus_operator
{
    /// <summary>
    /// Режимы подтверждения начала заявки снятием пистолета
    /// </summary>
    public enum NozzleConfirmModes
    {
        /// <summary>
        /// Подтверждение снятием пистолета
        /// </summary>
        OrderConfirmByNozzle = 0,
        /// <summary>
        /// Без подтверждения
        /// </summary>
        NoOrderConfirm = 1,
    }
}
