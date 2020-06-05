using System.ComponentModel;
using Newtonsoft.Json;

namespace TerrariaActions
{
    public class SetTime: BaseAction<SetTime>
    {
        
        [DefaultValue(0)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "time")]
        private int _time;
        
        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "is_day")]
        private bool _isDay;
    }
}