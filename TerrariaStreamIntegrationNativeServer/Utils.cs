﻿using System;
 using System.Collections.Generic;
 using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Terraria;
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
            return Main.netMode == 2;
        }

        public static Player GetPlayer(string name)
        {
            return Main.player.FirstOrDefault(player => player.name == name);
        }

        public static void SendChatMessage(string message, Color color)
        {
            ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), color);
        }
        
        public static string ToUnderscoreCase(this string str) {
            return string.Concat(str.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString())).ToLower();
        }
        
        public static string SentenceCase(this string input)
        {
            return Regex.Replace(input, @"(?<=[A-Za-z])(?=[A-Z][a-z])|(?<=[a-z0-9])(?=[0-9]?[A-Z])", " ");
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
                    players.AddRange(Main.player.Where(player => player.active && !player.dead));
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