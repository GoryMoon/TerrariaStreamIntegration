using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;

namespace TerrariaStreamIntegration.Actions
{
    public class StartEclipse: BaseAction
    {
        public override ActionResponse Handle()
        {
            if (!Main.eclipse)
            {
                Main.SkipToTime(0, true);
                Main.eclipse = true;
                ChatHelper.BroadcastChatMessage(Language.GetText("LegacyMisc." + 20).ToNetworkText(), new Color(50, (int) byte.MaxValue, 130), -1);
                NetMessage.SendData(7);
                return ActionResponse.Done;
            }
            
            TryLater(TimeSpan.FromSeconds(20));
            return ActionResponse.Retry;
        }
    }
}