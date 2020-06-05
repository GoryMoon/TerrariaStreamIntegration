﻿using System;
using Newtonsoft.Json;
using Terraria;

namespace TerrariaStreamIntegration.Actions
{
    public abstract class BaseAction
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "from")]
        public string From;
        
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, PropertyName = "target")]
        public string Target;

        [JsonIgnore]
        private Player[] _players = new Player[0];
        
        [JsonIgnore]
        protected Player Player => _players.Length > 0 ? _players[Main.rand.Next(_players.Length)]: Main.LocalPlayer;

        public DateTime? TryAfter = null;

        internal void SetPlayers(Player[] players)
        {
            _players = players;
        }
        
        protected void TryLater(TimeSpan time)
        {
            TryAfter = DateTime.Now + time;
        }
        
        public abstract ActionResponse Handle();
    }
}