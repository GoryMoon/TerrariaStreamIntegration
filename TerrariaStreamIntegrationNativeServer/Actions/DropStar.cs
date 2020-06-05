using System;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria;
using Terraria.ID;

namespace TerrariaStreamIntegration.Actions
{
    public class DropStar: BaseAction
    {

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "amount")]
        private readonly int _amount;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "spread")]
        private readonly int _spread;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "width")]
        private readonly int _width;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "height")]
        private readonly float _height;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "damage")]
        private readonly int _damage;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "hostile")]
        private readonly bool _hostile;
        
        public override ActionResponse Handle()
        {
            if (!Main.dayTime || _hostile)
            {
                for (var i = 0; i < _amount; i++)
                {
                    var width = Main.rand.Next(-_width, _width + 1);
                    var player = Player;
                    var vector = new Vector2(player.position.X + player.width * 0.5f + width, player.MountedCenter.Y - _height);
                    var num6 = (float) Main.rand.Next(-_spread, _spread + 1);
                    var num7 = (float) (Main.rand.Next(200) + 100);
                    var num8 = 12f / (float) Math.Sqrt(num6 * (double) num6 + num7 * (double) num7);
                    var speedX = num6 * num8;
                    var speedY = num7 * num8;
                    var num = Projectile.NewProjectile(vector.X, vector.Y, speedX, speedY, _hostile ? ProjectileID.Starfury: ProjectileID.FallingStarSpawner, _damage, 10f);
                    if (_hostile)
                    {
                        var projectile = Main.projectile[num];
                        projectile.friendly = false;
                        projectile.hostile = true;
                        projectile.melee = false;
                        projectile.magic = true;
                        projectile.netUpdate = true;
                    }
                }
                return ActionResponse.Done;
            }

            TryLater(TimeSpan.FromSeconds(15));
            return ActionResponse.Retry;
        }
    }
}