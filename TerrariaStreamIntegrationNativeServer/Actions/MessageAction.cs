﻿using Microsoft.Xna.Framework;
using Terraria.Chat;

namespace TerrariaStreamIntegration.Actions
{
    public class MessageAction: BaseAction
    {
        private readonly string _message;
        
        public MessageAction(string message)
        {
            _message = message;
        }

        public override ActionResponse Handle()
        {
            Utils.SendChatMessage(_message, Color.Yellow);
            return ActionResponse.Done;
        }
    }
}