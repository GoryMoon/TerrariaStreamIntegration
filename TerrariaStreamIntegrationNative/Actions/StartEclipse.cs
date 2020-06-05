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
                switch (Main.netMode)
                {
                    case 0:
                        Main.NewText(Language.GetText("LegacyMisc." + 20).Value, 50, byte.MaxValue, 130);
                        break;
                    case 2:
                        ChatHelper.BroadcastChatMessage(Language.GetText("LegacyMisc." + 20).ToNetworkText(), new Color(50, byte.MaxValue, 130));
                        break;
                }
                if (Main.netMode == 2)
                {
                    NetMessage.SendData(7);
                }

                return ActionResponse.Done;
            }
            
            TryLater(TimeSpan.FromSeconds(20));
            return ActionResponse.Retry;
        }
    }
}