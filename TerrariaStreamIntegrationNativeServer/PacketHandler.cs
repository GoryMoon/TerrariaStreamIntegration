﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria;
using TerrariaStreamIntegration.Packets;

namespace TerrariaStreamIntegration
{
    public class PacketHandler
    {
        private readonly Dictionary<byte, Type> _packetTypes = new Dictionary<byte, Type>();

        public void RegisterPacket(byte id, Type type)
        {
            if (!typeof(BasePacket).IsAssignableFrom(type)) return;
            
            _packetTypes.Add(id, type);
        }

        public void HandlePacket(BinaryReader reader, int whoAmI)
        {
            try
            {
                var id = reader.ReadByte();
                var packetType = _packetTypes[id];
                var instance = (BasePacket) Activator.CreateInstance(packetType);
                var packet = instance.Decode(reader);
                packet.Handle(whoAmI);
            }
            catch (Exception e)
            {
                MyModServer.Log.Error(e, "Error handling packet");
            }
        }

        public void BroadcastToAllPlayer(BasePacket packet)
        {
            Send(packet);
        }
        
        public void SendToPlayer(int player, BasePacket packet)
        {
            Send(packet, player);
        }

        private void Send(BasePacket packet, int player = -1)
        {
            var internalPacket = new InternalPacket();
            var packetType = packet.GetType();
            var packetId = _packetTypes.FirstOrDefault(x => x.Value == packetType).Key;
            
            internalPacket.Write(packetId);
            packet.Encode(internalPacket);
            internalPacket.Send(player);
        }
        
    }
}