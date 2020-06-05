﻿using System;
using System.IO;
using Terraria;

namespace TerrariaStreamIntegration.Packets
{
    public sealed class InternalPacket : BinaryWriter
    {
        private byte[] _buf;
        private ushort _len;

        internal InternalPacket(int capacity = 256) : base(new MemoryStream(capacity))
        {
            Write((ushort) 0);
            Write((byte)250);
        }
        public void Send(int toClient = -1, int ignoreClient = -1)
        {
            Finish();
            if (Main.netMode == 1)
            {
                Netplay.Connection.Socket.AsyncSend(_buf, 0, _len, SendCallback);
            }
            else if (toClient != -1)
            {
                Netplay.Clients[toClient].Socket.AsyncSend(_buf, 0, _len, SendCallback);
            }
            else
            {
                for (var index = 0; index < 256; ++index)
                    if (index != ignoreClient && Netplay.Clients[index].IsConnected() &&
                        NetMessage.buffer[index].broadcast)
                        Netplay.Clients[index].Socket.AsyncSend(_buf, 0, _len, SendCallback);
            }
        }

        
        
        private void SendCallback(object state) {}

        private void Finish()
        {
            if (_buf != null)
                return;
            if (OutStream.Position > ushort.MaxValue)
                throw new Exception($"Packet too large: {OutStream.Position} - {ushort.MaxValue}");
            _len = (ushort) OutStream.Position;
            Seek(0, SeekOrigin.Begin);
            Write(_len);
            Close();
            _buf = ((MemoryStream) OutStream).GetBuffer();
        }
    }
}