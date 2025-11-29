namespace GTA5Voice.Definitions
{
    internal static class Settings
    {
        internal static readonly Setting<string> VirtualServerUid = new Setting<string>("VirtualServerUID", true);
        internal static readonly Setting<int> IngameChannelId = new Setting<int>("IngameChannelId", true);
        internal static readonly Setting<int> FallbackChannelId = new Setting<int>("FallbackChannelId");
        internal static readonly Setting<string> IngameChannelPassword = new Setting<string>("IngameChannelPassword", true);
        internal static readonly Setting<bool> DebuggingEnabled = new Setting<bool>("DebuggingEnabled");
        internal static readonly Setting<string> Language = new Setting<string>("Language", true);
        internal static readonly Setting<int> CalculationInterval = new Setting<int>("CalculationInterval", defaultValue: 250);
        internal static readonly Setting<string> VoiceRanges = new Setting<string>("VoiceRanges", defaultValue: "[1, 3, 8, 15]");
        internal static readonly Setting<string> ExcludedChannels = new Setting<string>("ExcludedChannels");
    }
}
