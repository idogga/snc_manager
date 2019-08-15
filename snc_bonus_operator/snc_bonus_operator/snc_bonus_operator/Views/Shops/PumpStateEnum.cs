namespace snc_bonus_operator
{
    public enum PumpStateEnum
    {
        PUMP_STATE_LOCKED = 0,                  // отключена
        PUMP_STATE_FREE,                        // свободна
        PUMP_STATE_BUSY,                        // занята
        PUMP_STATE_WORK,                        // работает
        PUMP_STATE_UNDEFINED,                       // не используется
        PUMP_STATE_PAUSED,                      // остановлена
        PUMP_STATE_ACTIVATED,                       // пистолет снят
        PUMP_STATE_NO_ANSWER,                       // нет ответа

        PUMP_STATE_LOCKED_BY_REQUEST = 100,                     // заблокирована
        PUMP_STATE_WAIT_AUTO_FILLING = 101,                     // авто заявка
        PUMP_STATE_WAIT_FULL_TANK = 102,						// до полного бака
    }
}
