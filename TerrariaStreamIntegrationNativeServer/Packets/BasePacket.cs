﻿using System.IO;

namespace TerrariaStreamIntegration.Packets
{
    public abstract class BasePacket
    {

        public abstract void Encode(BinaryWriter writer);
        public abstract BasePacket Decode(BinaryReader reader);

        public abstract void Handle(int whoAmI);
    }
}