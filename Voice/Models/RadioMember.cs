using GTANetworkAPI;

namespace GTA5Voice.Voice.Models
{
    public class RadioMember
    {
        public RadioMember(Player player, bool isTalking = false)
        {
            Player = player;
            IsTalking = isTalking;
        }

        public Player Player { get; private set; }
        public bool IsTalking { get; set; }
    }
}
