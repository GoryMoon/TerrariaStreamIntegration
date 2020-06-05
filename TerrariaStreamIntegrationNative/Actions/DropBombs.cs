using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria;

namespace TerrariaStreamIntegration.Actions
{
    public class DropBombs: BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private int _amount;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "damage")]
        private int _damage;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "types")]
        private int[] _types;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "radius")]
        private int _radius;
        
        public override ActionResponse Handle()
        {
            for (var i = 0; i < _amount; i++)
            {
                var next = Main.rand.Next(_types.Length);
                if (!BombUtils.Bombs.TryGetValue(_types[next], out var bomb))
                {
                    i--;
                    continue;
                }
                var x = Main.rand.Next(-_radius, _radius + 1);
                var y = Main.rand.Next(-_radius, _radius + 1);
                Projectile.NewProjectile(Player.position + new Vector2(x, y), Vector2.Zero, bomb, _damage, 8, Main.myPlayer);
            }
            return ActionResponse.Done;
        }
    }
}