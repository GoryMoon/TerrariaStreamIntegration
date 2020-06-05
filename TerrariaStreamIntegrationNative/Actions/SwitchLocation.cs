using System;
using System.Linq;
using Newtonsoft.Json;
using Terraria;

namespace TerrariaStreamIntegration.Actions
{
    public class SwitchLocation: BaseAction
    {

        [JsonProperty(PropertyName = "other")]
        private string _other;
        
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
                
                    MyMod.Log.Debug($"Switching location of {player.name} and {other.name}");
                    var pos1 = player.position;
                    var pos2 = other.position;
                    player.Teleport(pos2);
                    other.Teleport(pos1);
                
                    return ActionResponse.Done;
                }
            }

            MyMod.Log.Debug("Retrying switch location");
            TryLater(TimeSpan.FromSeconds(20));
            return ActionResponse.Retry;
        }
    }
}