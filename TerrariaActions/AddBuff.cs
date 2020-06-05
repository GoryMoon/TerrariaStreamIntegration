
using System.ComponentModel;
using Newtonsoft.Json;

namespace TerrariaActions
{
    public class AddBuff: BaseAction<AddBuff>
    {
        [DefaultValue(18)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
        
        [DefaultValue(10)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "duration")]
        private int _duration;
        
    }
}