using System.ComponentModel;
using Newtonsoft.Json;

namespace TerrariaActions
{
    public class SwitchLocation: BaseAction<SwitchLocation>
    {
        
        [DefaultValue("random")]
        [JsonProperty(PropertyName = "other", DefaultValueHandling = DefaultValueHandling.Populate)]
        private string _other;
        
        [DefaultValue(0)]
        [JsonProperty(PropertyName = "style", DefaultValueHandling = DefaultValueHandling.Populate)]
        private int _style;
        
    }
}