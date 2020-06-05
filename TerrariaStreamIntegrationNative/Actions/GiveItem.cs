
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

        public override ActionResponse Handle()
        {
            var id = Item.NewItem(Player.position, Player.width, Player.height, _id, _amount, false, 0, _drop);
            if (_drop)
            {
                Main.item[id].noGrabDelay = 120;
            }
            return ActionResponse.Done;
        }
    }
}