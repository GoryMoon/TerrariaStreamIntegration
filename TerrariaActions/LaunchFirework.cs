using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TerrariaActions
{
    public class LaunchFirework: BaseAction<LaunchFirework>
    {
        
        [DefaultValue("1")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private string _amount;

        protected override LaunchFirework Process(LaunchFirework action, string username, string @from, Dictionary<string, object> parameters)
        {
            action._amount = StringToInt(_amount, 1, parameters).ToString();
            return base.Process(action, username, @from, parameters);
        }
    }
}