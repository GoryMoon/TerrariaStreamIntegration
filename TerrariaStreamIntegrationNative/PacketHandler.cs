﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
 using NLog;
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
                MyMod.Log.Error(e, "Error handling packet");
            }
        }

        public void BroadcastToAllPlayer(BasePacket packet)
        {
            if (Utils.IsClient())
            {
                packet.Handle(-1);
            } 
            else
            {
                Send(packet);
            }
        }
        
        public void SendToPlayer(int player, BasePacket packet)
        {
            if (Utils.IsClient())
            {
                packet.Handle(-1);
            } 
            else
            {
                Send(packet, player);
            }
        }

        public void SendToServer(BasePacket packet)
        {
            if (Utils.IsServer())
            {
                MyMod.Log.Debug("Already on server, handle here");
                packet.Handle(Main.myPlayer);
            }
            else
            {
                MyMod.Log.Debug("Sending packet to server");
                Send(packet);
            }
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