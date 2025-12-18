namespace GTA5Voice.Voice.Models
{
    public class PluginData
    {
        public PluginData()
        {
        }

        public PluginData(int? teamspeakId, bool websocketConnection, float currentVoiceRange, bool forceMuted = false, 
            bool phoneSpeakerEnabled = false, int[] currentCallMembers = null)
        {
            TeamspeakId = teamspeakId;
            WebsocketConnection = websocketConnection;
            CurrentVoiceRange = currentVoiceRange;
            ForceMuted = forceMuted;
            PhoneSpeakerEnabled = phoneSpeakerEnabled;
            CurrentCallMembers = currentCallMembers;
        }

        public int? TeamspeakId { get; set; }
        public bool WebsocketConnection { get; set; }
        public float CurrentVoiceRange { get; set; }
        public bool ForceMuted { get; set; }
        public bool PhoneSpeakerEnabled { get; set; }
        public int[] CurrentCallMembers { get; set; }
    }
}
