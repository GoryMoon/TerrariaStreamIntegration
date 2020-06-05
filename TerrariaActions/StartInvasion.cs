using System.ComponentModel;
using Newtonsoft.Json;

namespace TerrariaActions
{
    public class StartInvasion: BaseAction<StartInvasion>
    {
        
        [DefaultValue(1)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
    }
}