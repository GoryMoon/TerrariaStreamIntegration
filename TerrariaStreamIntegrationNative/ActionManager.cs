﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Humanizer;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Terraria;
using TerrariaStreamIntegration.Actions;

namespace TerrariaStreamIntegration
{
    public class ActionManager
    {
        private Dictionary<string, Type> _actions = new Dictionary<string, Type>();
        private ConcurrentQueue<BaseAction> _actionQueue = new ConcurrentQueue<BaseAction>();

        public ActionManager()
        {
            AddAction(typeof(DropStar));
            AddAction(typeof(DropItem));
            AddAction(typeof(HealPlayer));
            AddAction(typeof(FillMana));
            AddAction(typeof(StartInvasion));
            AddAction(typeof(StartBloodMoon));
            AddAction(typeof(SpawnNpc));
            AddAction(typeof(AddBuff));
            AddAction(typeof(ToggleBuff));
            AddAction(typeof(GiveItem));
            AddAction(typeof(DropBombs));
            AddAction(typeof(SetTime));
            AddAction(typeof(SwitchLocation));
            AddAction(typeof(LaunchFirework));
            AddAction(typeof(StartEclipse));
        }

        ~ActionManager()
        {
            _actions.Clear();
            _actions = null;
            _actionQueue = null;
        }

        private void AddAction(Type action)
        {
            if (!typeof(BaseAction).IsAssignableFrom(action)) return;

            var type = action.Name.Underscore();
            _actions.Add(type, action);
        }

        public void HandleAction(string rawAction, Player sender, int whoAmI)
        {
            MyMod.Log.Debug($"ActionManager: Handling action {rawAction} from {sender.name}");
            try
            {
                var o = JsonConvert.DeserializeObject<JObject>(rawAction);
                var type = (string) o["type"];
                
                var actionType = _actions[type];
                if (actionType != null)
                {
                    var actionObj = o.ToObject(actionType);
                    if (actionObj is BaseAction action)
                    {
                        action.SetPlayers(Utils.GetTargets(sender, action.Target));
                        _actionQueue.Enqueue(action);
                    }
                }
            }
            catch (Exception e)
            {
                MyMod.Log.Error(e, "Error parsing action");
            }
        }

        public void HandleMessage(string message)
        {
            MyMod.Log.Debug($"ActionManager: Handling message {message}");
            _actionQueue.Enqueue(new MessageAction(message));
        }

        public void Update()
        {
            if (_actionQueue.TryDequeue(out var action))
            {
                if (action.TryAfter.HasValue)
                {
                    if (action.TryAfter.Value > DateTime.Now)
                    {
                        _actionQueue.Enqueue(action);
                        return;
                    }

                    action.TryAfter = null;
                }
                
                var response = action.Handle();
                switch (response)
                {
                    case ActionResponse.Retry:
                        _actionQueue.Enqueue(action);
                        break;
                    case ActionResponse.Done:
                    {
                        if (action.GetType() != typeof(MessageAction))
                        {
                            Utils.SendChatMessage($"{action.From} ran action {action.GetType().Name.Humanize()}", Color.Orange);
                        }
                        break;
                    }
                }
            }
        }
    }

    public enum ActionResponse
    {
        Done,
        Retry
    }
}