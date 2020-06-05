﻿using System.IO;
 using Microsoft.Xna.Framework;
 using Terraria;
 using Terraria.Audio;

 namespace TerrariaStreamIntegration.Packets
{
    public class BuffPacket: BasePacket
    {
        private readonly int _id;
        private readonly int _duration;
        private readonly bool _add;

        public BuffPacket() {}

        public BuffPacket(int id, int duration, bool add)
        {
            _id = id;
            _duration = duration;
            _add = add;
        }

        public override void Encode(BinaryWriter writer)
        {
            writer.Write(_id);
            writer.Write(_duration);
            writer.Write(_add);
        }

        public override BasePacket Decode(BinaryReader reader)
        {
            return new BuffPacket(reader.ReadInt32(), reader.ReadInt32(), reader.ReadBoolean());
        }

        public override void Handle(int whoAmI) {}
    }
}