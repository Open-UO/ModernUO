using Server.Items;

namespace Server.Mobiles
{
    public class Ravager : BaseCreature
    {
        [Constructible]
        public Ravager() : base(AIType.AI_Melee)
        {
            Body = 314;
            BaseSoundID = 357;

            SetStr(251, 275);
            SetDex(101, 125);
            SetInt(66, 90);

            SetHits(161, 175);

            SetDamage(15, 20);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 50, 60);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 20, 30);

            SetSkill(SkillName.MagicResist, 50.1, 75.0);
            SetSkill(SkillName.Tactics, 75.1, 100.0);
            SetSkill(SkillName.Wrestling, 70.1, 90.0);

            Fame = 3500;
            Karma = -3500;

            VirtualArmor = 54;
        }

        public Ravager(Serial serial) : base(serial)
        {
        }

        public override string CorpseName => "a ravager corpse";

        public override string DefaultName => "a ravager";

        public override WeaponAbility GetWeaponAbility() =>
            Utility.RandomBool() ? WeaponAbility.Dismount : WeaponAbility.CrushingBlow;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        public override void Serialize(IGenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(IGenericReader reader)
        {
            base.Deserialize(reader);
            var version = reader.ReadInt();
        }
    }
}
