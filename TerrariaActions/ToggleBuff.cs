
using System.ComponentModel;
using Newtonsoft.Json;

namespace TerrariaActions
{
    public class ToggleBuff: BaseAction<ToggleBuff>
    {
        
        [DefaultValue(18)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
        
        [DefaultValue(10)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "duration")]
        private int _duration;
        
    }
}