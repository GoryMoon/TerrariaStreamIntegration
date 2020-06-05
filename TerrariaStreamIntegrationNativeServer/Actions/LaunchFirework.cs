using Newtonsoft.Json;
using Terraria;

namespace TerrariaStreamIntegration.Actions
{
    public class LaunchFirework: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private int _amount;

        public override ActionResponse Handle()
        {
            for (var i = 0; i < _amount; i++)
            {
                var pos = Player.position.ToTileCoordinates();
                WorldGen.LaunchRocketSmall(pos.X, pos.Y);
            }

            return ActionResponse.Done;
        }
    }
}