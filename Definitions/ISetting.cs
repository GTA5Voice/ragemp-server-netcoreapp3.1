namespace GTA5Voice.Definitions
{
    internal interface ISetting
    {
        string Key { get; }
        bool Required { get; }
        object DefaultValue { get; }
        bool HasDefaultValue { get; }
    }

    internal class Setting<T> : ISetting
    {
        public string Key { get; }
        public bool Required { get; }
        private T DefaultValue { get; }
        public bool HasDefaultValue { get; }

        object ISetting.DefaultValue => HasDefaultValue ? (object)DefaultValue : null;

        public Setting(string key, bool required = false, T defaultValue = default(T))
        {
            Key = key;
            Required = required;

            if (defaultValue == null || (typeof(T).IsValueType && defaultValue.Equals(default(T))))
            {
                HasDefaultValue = false;
                DefaultValue = default(T);
            }
            else
            {
                HasDefaultValue = true;
                DefaultValue = defaultValue;
            }
        }
    }
}
