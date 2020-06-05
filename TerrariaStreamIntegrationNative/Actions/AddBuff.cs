
using System;
using Newtonsoft.Json;

namespace TerrariaStreamIntegration.Actions
{
    public class AddBuff: BaseAction
    {

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        public int Id;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "duration")]
        public int Duration;
        
        public override ActionResponse Handle()
        {
            if (!Player.dead)
            {
                var index = Player.FindBuffIndex(Id);
                Player.AddBuff(Id, Duration * 60);
                if (index == -1)
                {
                    Utils.SpecialHandleEffects(Id, Player);
                }
                return ActionResponse.Done;
            }

            TryLater(TimeSpan.FromSeconds(5));
            return ActionResponse.Retry;
        }
    }
}