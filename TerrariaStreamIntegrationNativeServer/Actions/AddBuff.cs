
using System;
using Newtonsoft.Json;
using TerrariaStreamIntegration.Packets;

namespace TerrariaStreamIntegration.Actions
{
    public class AddBuff: BaseAction
    {

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "duration")]
        private int _duration;
        
        public override ActionResponse Handle()
        {
            if (!Player.dead)
            {
                MyModServer.PacketHandler.SendToPlayer(Player.whoAmI, new BuffPacket(_id, _duration, true));
                return ActionResponse.Done;
            }

            TryLater(TimeSpan.FromSeconds(5));
            return ActionResponse.Retry;
        }
    }
}