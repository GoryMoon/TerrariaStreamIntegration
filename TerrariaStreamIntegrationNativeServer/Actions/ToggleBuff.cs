using Newtonsoft.Json;
using TerrariaStreamIntegration.Packets;

namespace TerrariaStreamIntegration.Actions
{
    public class ToggleBuff: BaseAction
    {

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "duration")]
        private int _duration;
        
        public override ActionResponse Handle()
        {
            MyModServer.PacketHandler.SendToPlayer(Player.whoAmI, new BuffPacket(_id, _duration, false));
            return ActionResponse.Done;
        }
    }
}