using Newtonsoft.Json;
using Terraria;

namespace TerrariaStreamIntegration.Actions
{
    public class SpawnNpc: BaseAction
    {
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private int _amount;

        public override ActionResponse Handle()
        {
            for (var i = 0; i < _amount; i++)
            {
                NPC.SpawnOnPlayer(Player.whoAmI, _id);
                var latestSpawn = Utils.LatestSpawn;
                Main.npc[latestSpawn].GivenName = From;
                NetMessage.SendData(56, -1, -1, null, latestSpawn);
            }

            return ActionResponse.Done;
        }
    }
}