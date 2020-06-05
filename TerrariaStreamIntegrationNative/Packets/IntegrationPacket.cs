﻿using System.IO;
using Terraria;

namespace TerrariaStreamIntegration.Packets
{
    public class IntegrationPacket: BasePacket
    {
        private readonly bool _message;
        private readonly string _data;
        private readonly string _player;
        
        public IntegrationPacket() {}

        public IntegrationPacket(string data, bool message, string player = null)
        {
            _data = data;
            _message = message;
            _player = player ?? Main.LocalPlayer.name;
        }

        public override void Encode(BinaryWriter writer)
        {
            writer.Write(_data);
            writer.Write(_message);
            writer.Write(_player);
        }

        public override BasePacket Decode(BinaryReader reader)
        {
            return new IntegrationPacket(reader.ReadString(), reader.ReadBoolean(), reader.ReadString());
        }

        public override void Handle(int whoAmI)
        {
            if (_message)
                MyMod.Instance.ActionManager.HandleMessage(_data);
            else
            {
                Player player;
                if ((player = Utils.GetPlayer(_player)) != null)
                {
                    MyMod.Instance.ActionManager.HandleAction(_data, player, whoAmI);   
                }
            }
        }
    }
}