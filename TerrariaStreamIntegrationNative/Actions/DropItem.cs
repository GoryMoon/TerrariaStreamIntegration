
using TerrariaStreamIntegration.Packets;

namespace TerrariaStreamIntegration.Actions
{
    public class DropItem: BaseAction
    {
        public override ActionResponse Handle()
        {
            MyMod.PacketHandler.SendToPlayer(Player.whoAmI, new DropItemPacket());
            return ActionResponse.Done;
        }
    }
}