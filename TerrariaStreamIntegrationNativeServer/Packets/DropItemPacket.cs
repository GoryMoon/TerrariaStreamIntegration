﻿using System.IO;
using Terraria;
 using Terraria.Audio;
 using Terraria.ID;

 namespace TerrariaStreamIntegration.Packets
{
    public class DropItemPacket: BasePacket
    {
        
        public DropItemPacket() {}

        public override void Encode(BinaryWriter writer)
        {}

        public override BasePacket Decode(BinaryReader reader)
        {
            return new DropItemPacket();
        }

        public override void Handle(int whoAmI)
        {
            Main.LocalPlayer.DropSelectedItem();
        }
    }
}