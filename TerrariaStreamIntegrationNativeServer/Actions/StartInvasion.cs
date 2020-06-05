﻿using System;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;
using TerrariaStreamIntegration.Packets;

namespace TerrariaStreamIntegration.Actions
{
    public class StartInvasion: BaseAction
    {
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "id")]
        private int _id;
        
        public override ActionResponse Handle()
        {
            if (Main.invasionType == 0)
            {
                MyModServer.PacketHandler.BroadcastToAllPlayer(new SoundPacket(SoundID.Roar, Player.position, 0));
                Main.invasionDelay = 0;
                Main.StartInvasion(_id);
                return ActionResponse.Done;
            }
            
            TryLater(TimeSpan.FromSeconds(30));
            return ActionResponse.Retry;
        }
    }
}