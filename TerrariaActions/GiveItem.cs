﻿
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace TerrariaActions
{
    public class GiveItem: BaseAction<GiveItem>
    {
        
        [DefaultValue(2)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
        
        [DefaultValue("1")]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private string _amount;
        
        [DefaultValue(true)]
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "drop")]
        private bool _drop;
        
        protected override GiveItem Process(GiveItem action, string username, string from, Dictionary<string, object> parameters)
        {
            action._amount = StringToInt(_amount, 1, parameters).ToString();
            return base.Process(action, username, from, parameters);
        }
    }
}