﻿using System;
 using Newtonsoft.Json;
 using TerrariaStreamIntegration.Packets;

 namespace TerrariaStreamIntegration.Actions
{
    public class FillMana: BaseAction
    {
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private int _amount;
        
        public override ActionResponse Handle()
        {
            if (!Player.dead)
            {
                MyMod.PacketHandler.SendToPlayer(Player.whoAmI, new ManaHealPacket(false, _amount));
                return ActionResponse.Done;
            }

            TryLater(TimeSpan.FromSeconds(5));
            return ActionResponse.Retry;
        }
    }
}