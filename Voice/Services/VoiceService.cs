using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using GTA5Voice.Extensions;
using GTA5Voice.Logging;
using GTA5Voice.Voice.Models;
using GTANetworkAPI;

namespace GTA5Voice.Voice.Services
{
    public class VoiceService
    {
        private static readonly List<VoiceClient> Clients = new List<VoiceClient>();

        internal static VoiceClient? FindClient(Player player)
            => Clients.FirstOrDefault(x => x.Player == player);

        private static VoiceClient? FindClient(int playerId)
            => Clients.FirstOrDefault(x => x.Player.Id == playerId);

        private static Player[] GetVoiceClientPlayers()
            => Clients.Select(x => x.Player).ToArray();

        private static Player[] GetOtherVoiceClientPlayers(int selfId)
            => Clients.Where(x => x.Player.Id != selfId).Select(x => x.Player).ToArray();

        public VoiceClient? AddClient(Player player)
        {
            if (FindClient(player) != null)
            {
                ConsoleLogger.Debug($"Voice client (id: {player.Id}) already exists");
                return null;
            }

            var client = new VoiceClient(player, Guid.NewGuid().ToString("N")[..24]);
            Clients.Add(client);
            ConsoleLogger.Debug($"Added voice client (id: {client.Player.Id})");
            return client;
        }

        public void RemoveClient(Player player)
        {
            var client = FindClient(player);
            if (client != null)
            {
                Clients.Remove(client);
                RemoveLocalClientData(client.Player.Id);
                ConsoleLogger.Debug($"Removed voice client (id: {client.Player.Id})");
                return;
            }

            ConsoleLogger.Debug($"Couldn't find voice client (id: {player.Id})");
        }

        private class VoiceClientData
        {
            public VoiceClientData(ushort remoteId, int? teamspeakId, bool websocketConnection, float currentVoiceRange, 
                bool forceMuted, bool phoneSpeakerEnabled, int[] currentCallMembers)
            {
                RemoteId = remoteId;
                TeamspeakId = teamspeakId;
                WebsocketConnection = websocketConnection;
                CurrentVoiceRange = currentVoiceRange;
                ForceMuted = forceMuted;
                PhoneSpeakerEnabled = phoneSpeakerEnabled;
                CurrentCallMembers = currentCallMembers;
            }

            public ushort RemoteId { get; }
            public int? TeamspeakId { get; }
            public bool WebsocketConnection { get; }
            public float CurrentVoiceRange { get; }
            public bool ForceMuted { get; }
            public bool PhoneSpeakerEnabled { get; }
            public int[] CurrentCallMembers { get; set; }
        }

        public void LoadLocalClientData(int remoteId)
        {
            var requestingClient = FindClient(remoteId);
            if (requestingClient == null)
                return;

            var otherClients = Clients.Where(x => x.Player.Id != remoteId).ToArray();
            if (otherClients.Length == 0)
                return;

            const int chunkSize = 1 << 15; // Max length is 2^16, recommended is 2^15 tho

            foreach (var chunk in SplitIntoChunks(otherClients, chunkSize))
            {
                var clientData = chunk.Select(client =>
                {
                    var pluginData = client.GetPluginData();
                    return new VoiceClientData(
                        client.Player.Id,
                        pluginData?.TeamspeakId,
                        pluginData?.WebsocketConnection ?? false,
                        pluginData?.CurrentVoiceRange ?? 0,
                        pluginData?.ForceMuted ?? false,
                        pluginData?.PhoneSpeakerEnabled ?? false,
                        pluginData?.CurrentCallMembers ?? Array.Empty<int>()
                    );
                }).ToArray();

                NAPI.ClientEvent.TriggerClientEvent(requestingClient.Player, "Client:GTA5Voice:LoadClientData",
                    JsonSerializer.Serialize(clientData));
            }
        }

        private static IEnumerable<VoiceClient[]> SplitIntoChunks(VoiceClient[] clients, int chunkSize)
        {
            for (var i = 0; i < clients.Length; i += chunkSize)
            {
                var length = Math.Min(chunkSize, clients.Length - i);
                var chunk = new VoiceClient[length];
                Array.Copy(clients, i, chunk, 0, length);
                yield return chunk;
            }
        }

        /**
         * Updates the client data for all other voice clients
         */
        public void UpdateLocalClientData(int remoteId, PluginData pluginData)
        {
            var vClients = GetOtherVoiceClientPlayers(remoteId);
            NAPI.ClientEvent.TriggerClientEventToPlayers(vClients, "Client:GTA5Voice:UpdateClientData", remoteId,
                pluginData);
        }

        public void SetForceMuted(Player player, bool forceMuted)
        {
            var client = FindClient(player);
            if (client == null)
            {
                ConsoleLogger.Debug($"Couldn't find voice client (id: {player.Id})");
                return;
            }

            var pluginData = client.GetPluginData();
            if (pluginData == null || pluginData.ForceMuted == forceMuted)
                return;

            var updatedPluginData = new PluginData(pluginData.TeamspeakId, pluginData.WebsocketConnection,
                pluginData.CurrentVoiceRange, forceMuted, pluginData.PhoneSpeakerEnabled, pluginData.CurrentCallMembers);
            client.SetPluginData(updatedPluginData, UpdateLocalClientData);
        }
        
        public void SetPhoneSpeakerEnabled(Player player, bool phoneSpeakerEnabled)
        {
            var client = FindClient(player);
            if (client == null)
            {
                ConsoleLogger.Debug($"Couldn't find voice client (id: {player.Id})");
                return;
            }
            
            // Prevent to enable phone speaker while not on phone call
            if (player.GetCurrentCall() == null)
                phoneSpeakerEnabled = false;

            var pluginData = client.GetPluginData();
            if (pluginData == null || pluginData.PhoneSpeakerEnabled == phoneSpeakerEnabled)
                return;

            var updatedPluginData = new PluginData(pluginData.TeamspeakId, pluginData.WebsocketConnection,
                pluginData.CurrentVoiceRange, pluginData.ForceMuted, phoneSpeakerEnabled, pluginData.CurrentCallMembers);
            client.SetPluginData(updatedPluginData, UpdateLocalClientData);
        }
        
        public void SetCurrentCallMembers(Player player, int[] callMembers)
        {
            var client = FindClient(player);
            if (client == null)
            {
                ConsoleLogger.Debug($"Couldn't find voice client (id: {player.Id})");
                return;
            }


            var pluginData = client.GetPluginData();
            if (pluginData == null || pluginData.CurrentCallMembers == callMembers)
                return;

            var updatedPluginData = new PluginData(pluginData.TeamspeakId, pluginData.WebsocketConnection,
                pluginData.CurrentVoiceRange, pluginData.ForceMuted, pluginData.PhoneSpeakerEnabled, callMembers);
            client.SetPluginData(updatedPluginData, UpdateLocalClientData);
        }
        
        public void ClearCurrentCallMembers(Player player)
            => SetCurrentCallMembers(player, Array.Empty<int>());

        /**
         * Removes a specific player from the data for all other voice clients
         */
        private void RemoveLocalClientData(int remoteId)
        {
            var vClients = GetOtherVoiceClientPlayers(remoteId);
            NAPI.ClientEvent.TriggerClientEventToPlayers(vClients, "Client:GTA5Voice:RemoveClient", remoteId);
        }
    }
}
