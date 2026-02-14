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
        internal static readonly Setting<bool> EnableDistanceBasedVolume = new Setting<bool>("EnableDistanceBasedVolume", defaultValue: false);
        internal static readonly Setting<double> VolumeDecreaseMultiplier = new Setting<double>("VolumeDecreaseMultiplier", defaultValue: 1.0);
        internal static readonly Setting<double> MinimumVoiceVolume = new Setting<double>("MinimumVoiceVolume", defaultValue: 0.25);

    }
}
