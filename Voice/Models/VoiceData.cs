using GTA5Voice.Definitions;
using GTA5Voice.Services;

namespace GTA5Voice.Voice.Models
{
    public class VoiceData
    {
        public VoiceData(SettingsService settingsService)
        {
            VirtualServerUid = settingsService.Get<string>(Settings.VirtualServerUid.Key);
            IngameChannelId = settingsService.Get<int>(Settings.IngameChannelId.Key);
            IngameChannelPassword = settingsService.Get<string>(Settings.IngameChannelPassword.Key);
            FallbackChannelId = settingsService.Get<int>(Settings.FallbackChannelId.Key);
            Language = settingsService.Get<string>(Settings.Language.Key);
            CalculationInterval = settingsService.Get<int>(Settings.CalculationInterval.Key);
            VoiceRanges = settingsService.Get<string>(Settings.VoiceRanges.Key);
            ExcludedChannels = settingsService.Get<string>(Settings.ExcludedChannels.Key);
        }

        public string VirtualServerUid { get; set; }
        public int IngameChannelId { get; set; }
        public string IngameChannelPassword { get; set; }
        public int FallbackChannelId { get; set; }
        public string Language { get; set; }
        public int CalculationInterval { get; set; }
        public string VoiceRanges { get; set; }
        public string ExcludedChannels { get; set; }
    }
}
