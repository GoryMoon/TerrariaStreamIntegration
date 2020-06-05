using Newtonsoft.Json;
using Terraria;

namespace TerrariaStreamIntegration.Actions
{
    public class SetTime: BaseAction
    {
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "time")]
        private int _time;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "is_day")]
        private bool _isDay;
        
        public override ActionResponse Handle()
        {
            Main.SkipToTime(_time, _isDay);
            return ActionResponse.Done;
        }
    }
}