using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TerrariaActions
{
    public class DropStar: BaseAction<DropStar>
    {

        [DefaultValue("5")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private string _amount;

        [DefaultValue("100")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "spread")]
        private string _spread;
        
        [DefaultValue("10")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "width")]
        private string _width;

        [DefaultValue(600.0f)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "height")]
        private readonly float _height;

        [DefaultValue("100")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "damage")]
        private string _damage;
        
        [DefaultValue(false)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "hostile")]
        private readonly bool _hostile;

        protected override DropStar Process(DropStar action, string username, string from, Dictionary<string, object> parameters)
        {
            action._amount = StringToInt(_amount, 0, parameters).ToString();
            action._spread = StringToInt(_spread, 0, parameters).ToString();
            action._width = StringToInt(_width, 0, parameters).ToString();
            action._damage = StringToInt(_damage, 0, parameters).ToString();
            return base.Process(action, username, from, parameters);
        }
    }
}