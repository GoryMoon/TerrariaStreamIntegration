using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria;
using Terraria.Chat;
using Terraria.Localization;

namespace TerrariaStreamIntegration
{
    public class DeathCounter
    {

        public static string FileName = "/death.json";
        private Dictionary<string, int> _deathCount = new Dictionary<string, int>();
        private static readonly JsonSerializer Serializer = new JsonSerializer { Formatting = Formatting.Indented};
        
        public void OnPlayerDeath(Player player)
        {
            var name = player.name;
            if (_deathCount.TryGetValue(name, out var deaths))
            {
                _deathCount[name] = ++deaths;
            }
            else
            {
                _deathCount[name] = deaths = 1;
            }

            if (deaths % 25 == 0 || deaths == 10)
            {
                var message = $"{name} have died {deaths} times!";
                switch (Main.netMode)
                {
                    case 0:
                        Main.NewText(message, 250, 250, 0);
                        break;
                    case 2:
                        ChatHelper.BroadcastChatMessage(NetworkText.FromLiteral(message), new Color(250, 250, 0));
                        break;
                }
            }
        }
        
        public void Load()
        {
            MyMod.Log.Debug("Reading death counter");
            if (File.Exists(FileName))
            {
                try
                {
                    using (var file = File.OpenText(FileName))
                    {
                        _deathCount = Serializer.Deserialize(file, typeof(Dictionary<string, int>)) as Dictionary<string, int>;
                    }
                }
                catch (Exception e)
                {
                    MyMod.Log.Error(e, "Error reading death counter");
                }
            }
            
            if (_deathCount == null)
            {
                MyMod.Log.Debug("Creating new death counter");
                _deathCount = new Dictionary<string, int>();
            }
            Save();
        }

        public void Save()
        {
            MyMod.Log.Debug("Saving death counter");
            try
            {
                using (var file = File.CreateText(FileName))
                {
                    Serializer.Serialize(file, _deathCount);
                }
            }
            catch (Exception e)
            {
                MyMod.Log.Error(e, "Error saving death counter");
            }
            MyMod.Log.Debug("Saved death counter");
        }
    }
}