﻿using System.IO;
using Terraria;
 using Terraria.Audio;
 using Terraria.ID;

 namespace TerrariaStreamIntegration.Packets
{
    public class ManaHealPacket: BasePacket
    {
        private readonly bool _heal;
        private readonly int _amount;
        
        public ManaHealPacket() {}

        public ManaHealPacket(bool heal, int amount)
        {
            _heal = heal;
            _amount = amount;
        }

        public override void Encode(BinaryWriter writer)
        {
            writer.Write(_heal);
            writer.Write((short)_amount);
        }

        public override BasePacket Decode(BinaryReader reader)
        {
            return new ManaHealPacket(reader.ReadBoolean(), reader.ReadInt16());
        }

        public override void Handle(int whoAmI)
        {
            if (_heal)
            {
                var player = Main.LocalPlayer;
                var amount = _amount;
                if (amount == 0)
                {
                    amount = player.statLifeMax2;
                }
                SoundEngine.PlaySound(SoundID.Item4);
                player.statLife += amount;
                if (Main.myPlayer == whoAmI)
                    player.HealEffect(amount);
                if (player.statLife > player.statLifeMax2)
                    player.statLife = player.statLifeMax2;
            }
            else
            {
                var player = Main.LocalPlayer;
                var amount = _amount;
                if (amount == 0)
                {
                    amount = player.statManaMax2;
                }
                player.statMana += amount;
                if (Main.myPlayer == whoAmI)
                    player.ManaEffect(amount);
                if (player.statMana > player.statManaMax2)
                    player.statMana = player.statManaMax2;
            }
        }
    }
}