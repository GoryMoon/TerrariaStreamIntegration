
using Newtonsoft.Json;
using Terraria;

namespace TerrariaStreamIntegration.Actions
{
    public class GiveItem: BaseAction
    {
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private int _amount;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "drop")]
        private bool _drop;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "name")]
        private string _name;
        
        public override ActionResponse Handle()
        {
            var player = Player;
            var id = Item.NewItem(player.position, player.width, player.height, _id, _amount, false, 0, _drop);
            if (_drop)
            {
                Main.item[id].noGrabDelay = 120;
            }

            if (!string.IsNullOrEmpty(_name))
            {
                Main.item[id].SetNameOverride(_name);
            }
            return ActionResponse.Done;
        }
    }
}