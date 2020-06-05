using System.Collections.Generic;
using static Terraria.ID.ProjectileID;

namespace TerrariaStreamIntegration
{
    public static class BombUtils
    {
        public static readonly Dictionary<int, short> Bombs = new Dictionary<int, short>();

        static BombUtils()
        {
            Add(Bomb, StickyBomb, HappyBomb, BombSkeletronPrime, SmokeBomb, BouncyBomb, BombFish, SantaBombs, 
                DD2GoblinBomb, ScarabBomb, WetBomb, LavaBomb, HoneyBomb, DryBomb, DirtBomb, DirtStickyBomb, 
                Dynamite, StickyDynamite, BouncyDynamite,
                Grenade, GrenadeI, GrenadeII, GrenadeIII, GrenadeIV, StickyGrenade, BouncyGrenade, PartyGirlGrenade,
                ClusterGrenadeI, ClusterGrenadeII, WetGrenade, LavaGrenade, HoneyGrenade, MiniNukeGrenadeI, 
                MiniNukeGrenadeII, DryGrenade, DynamiteKitten);
        }
        
        private static void Add(params short[] ids)
        {
            for (var i = 0; i < ids.Length; i++)
            {
                Bombs.Add(i, ids[i]);
            }
        }
    }
}