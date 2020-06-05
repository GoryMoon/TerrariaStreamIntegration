using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TerrariaActions
{
    public class FillMana: BaseAction<FillMana>
    {
        
        [DefaultValue("0")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private string _amount;
        
        protected override FillMana Process(FillMana action, string username, string from, Dictionary<string, object> parameters)
        {
            action._amount = StringToInt(_amount, 0, parameters).ToString();
            return base.Process(action, username, from, parameters);
        }
    }
}