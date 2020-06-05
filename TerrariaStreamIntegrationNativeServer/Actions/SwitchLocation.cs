using System;
using System.Linq;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;

namespace TerrariaStreamIntegration.Actions
{
    public class SwitchLocation: BaseAction
    {

        [JsonProperty(PropertyName = "other")]
        private string _other;

        [JsonProperty(PropertyName = "style")]
        private int _style;
        
        public override ActionResponse Handle()
        {
            var player = Player;
            var others = Utils.GetTargets(player, _other);
            if (others.Length > 0)
            {
                others = others.Where(player1 => player1.whoAmI != player.whoAmI).ToArray();
                if (others.Length > 0)
                {
                    var other = others[Main.rand.Next(others.Length)];
                
                    MyModServer.Log.Debug($"Switching location of {player.name} and {other.name}");
                    var pos1 = player.position;
                    var pos2 = other.position;
                    
                    player.Teleport(pos2, _style);
                    RemoteClient.CheckSection(player.whoAmI, pos2);
                    NetMessage.TrySendData(65, -1, -1, null, 0, player.whoAmI, pos2.X, pos2.Y, _style);
                    
                    other.Teleport(pos1);
                    RemoteClient.CheckSection(other.whoAmI, pos1);
                    NetMessage.TrySendData(65, -1, -1, null, 0, other.whoAmI, pos1.X, pos1.Y, _style);
                    
                    return ActionResponse.Done;
                }
            }

            MyModServer.Log.Debug("Retrying switch location");
            TryLater(TimeSpan.FromSeconds(20));
            return ActionResponse.Retry;
        }
    }
}