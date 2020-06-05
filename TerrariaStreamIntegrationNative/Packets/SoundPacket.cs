﻿using System.IO;
 using Microsoft.Xna.Framework;
 using Terraria;
 using Terraria.Audio;

 namespace TerrariaStreamIntegration.Packets
{
    public class SoundPacket: BasePacket
    {
        private readonly int _id;
        private readonly Vector2 _position;
        private readonly int _style;

        public SoundPacket() {}

        public SoundPacket(int id, Vector2 position, int style = 1)
        {
            _id = id;
            _position = position;
            _style = style;
        }

        public override void Encode(BinaryWriter writer)
        {
            writer.Write(_id);
            writer.WriteVector2(_position);
            writer.Write((short)_style);
        }

        public override BasePacket Decode(BinaryReader reader)
        {
            return new SoundPacket(reader.ReadInt32(), reader.ReadVector2(), reader.ReadInt16());
        }

        public override void Handle(int whoAmI)
        {
            SoundEngine.PlaySound(_id, _position, _style);
        }
    }
}