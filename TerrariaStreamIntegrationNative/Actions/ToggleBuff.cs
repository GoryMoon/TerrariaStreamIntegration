using Newtonsoft.Json;

namespace TerrariaStreamIntegration.Actions
{
    public class ToggleBuff: BaseAction
    {

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        public int Id;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "duration")]
        public int Duration;
        
        public override ActionResponse Handle()
        {
            var buffIndex = Player.FindBuffIndex(Id);
            if (buffIndex == -1)
            {
                Player.AddBuff(Id, Duration * 60);
                Utils.SpecialHandleEffects(Id, Player);
            }
            else
            {
                Player.DelBuff(buffIndex);
            }
            return ActionResponse.Done;
        }
    }
}