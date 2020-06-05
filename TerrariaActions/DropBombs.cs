using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TerrariaActions
{
    public class DropBombs: BaseAction<DropBombs>
    {
        [DefaultValue("1")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private string _amount;
        
        [DefaultValue("60")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "damage")]
        private string _damage;

        [DefaultValue(new[] {19, 24})]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "types")]
        private int[] _types;
        
        [DefaultValue(100)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "radius")]
        private int _radius;

        protected override DropBombs Process(DropBombs action, string username, string from, Dictionary<string, object> parameters)
        {
            action._amount = StringToInt(_amount, 1, parameters).ToString();
            action._damage = StringToInt(_damage, 1, parameters).ToString();
            return base.Process(action, username, from, parameters);
        }
    }
}