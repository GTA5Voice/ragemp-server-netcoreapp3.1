using System;
using GTA5Voice.Services;
using GTANetworkAPI;

namespace GTA5Voice.Voice.Models
{
    public class VoiceClient
    {
        public VoiceClient(Player player, string teamspeakName)
        {
            Player = player;
            _teamspeakName = teamspeakName;
        }

        public Player Player { get; }
        private PluginData? PluginData { get; set; }
        private readonly string _teamspeakName;

        public void Initialize(SettingsService settingsService)
            => Player.TriggerEvent("Client:GTA5Voice:initialize", new VoiceData(settingsService), _teamspeakName);

        public void Start()
            => Player.TriggerEvent("Client:GTA5Voice:connect");

        public void SetPluginData(PluginData pluginData, Action<int, PluginData> onDataChanged)
        {
            PluginData = pluginData;
            onDataChanged(Player.Id, PluginData);
        }

        public PluginData? GetPluginData()
            => PluginData;
    }
}
