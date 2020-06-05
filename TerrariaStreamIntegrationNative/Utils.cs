﻿using System;
 using System.Collections.Generic;
 using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
 using Terraria.Audio;
 using Terraria.Chat;
 using Terraria.ID;
 using Terraria.Localization;

namespace TerrariaStreamIntegration
{
    public static class Utils
    {
        public static int LatestSpawn { get; set; }

        public static bool IsServer()
        {
            return Main.netMode == 2 || IsSinglePlayer();
        }

        public static bool IsClient()
        {
            return Main.netMode == 1 || IsSinglePlayer();
        }

        public static bool IsSinglePlayer()
        {
            return Main.netMode == 0;
        }

        public static bool InGame()
        {
            return Main.mapReady;
        }

        public static Player GetPlayer(string name)
        {
            return Main.player.FirstOrDefault(player => player.name == name);
        }

        public static void SendChatMessage(string message, Color color)
        {
            if (IsSinglePlayer())
                NewText(message, color);
            else
                ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), color);
        }
        
        public static void NewText(string newText, Color color)
        {
            Main.NewText(newText, color.R, color.G, color.B);
        }

        public static void SpecialHandleEffects(int id, Player player)
        {
            if (id == BuffID.Gravitation)
            {
                if (Math.Abs(player.gravDir - 1.0) < double.Epsilon)
                {
                    player.gravDir = -1f;
                    player.fallStart = (int) (player.position.Y / 16.0);
                    player.jump = 0;
                    SoundEngine.PlaySound(SoundID.Item8, player.position);
                }
                else
                {
                    player.gravDir = 1f;
                    player.fallStart = (int) (player.position.Y / 16.0);
                    player.jump = 0;
                    SoundEngine.PlaySound(SoundID.Item8, player.position);
                }
            }
        }

        public static Player[] GetTargets(Player sender, string target)
        {
            var players = new List<Player>();
            switch (target)
            {
                case "self":
                    players.Add(sender);
                    break;
                case "random":
                    players.AddRange(Main.player);
                    break;
                default:
                {
                    var p = GetPlayer(target);
                    players.Add(p ?? sender);
                    break;
                }
            }

            return players.ToArray();
        }
    }
}