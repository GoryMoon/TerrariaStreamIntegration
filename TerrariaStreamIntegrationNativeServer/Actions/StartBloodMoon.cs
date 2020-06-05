using System;
using Terraria;
using Terraria.ID;
using TerrariaStreamIntegration.Packets;

namespace TerrariaStreamIntegration.Actions
{
    public class StartBloodMoon: BaseAction
    {
        public override ActionResponse Handle()
        {
            if (!Main.bloodMoon)
            {
                Main.SkipToTime(0, false);
                MyModServer.PacketHandler.BroadcastToAllPlayer(new SoundPacket(SoundID.Roar, Player.position, 0));
                Main.bloodMoon = true;
                return ActionResponse.Done;
            }
            
            TryLater(TimeSpan.FromSeconds(20));
            return ActionResponse.Retry;
        }
    }
}