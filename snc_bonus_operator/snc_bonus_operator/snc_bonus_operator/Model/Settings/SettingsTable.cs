using Realms;

namespace snc_bonus_operator.Settings
{
    public class SettingsTable : RealmObject
    {
        public int Key { get; set; } = 0;

        public string Description { get; set; } = "";

        public string Value { get; set; } = null;
    }
}
