using ModernUO.Serialization;
using System;
using Server.Items;

namespace Server.Mobiles
{
    [SerializationGenerator(0, false)]
    public partial class ThiefGuildmaster : BaseGuildmaster
    {
        [Constructible]
        public ThiefGuildmaster() : base("thief")
        {
            SetSkill(SkillName.DetectHidden, 75.0, 98.0);
            SetSkill(SkillName.Hiding, 65.0, 88.0);
            SetSkill(SkillName.Lockpicking, 85.0, 100.0);
            SetSkill(SkillName.Snooping, 90.0, 100.0);
            SetSkill(SkillName.Poisoning, 60.0, 83.0);
            SetSkill(SkillName.Stealing, 90.0, 100.0);
            SetSkill(SkillName.Fencing, 75.0, 98.0);
            SetSkill(SkillName.Stealth, 85.0, 100.0);
            SetSkill(SkillName.RemoveTrap, 85.0, 100.0);
        }

        public override NpcGuild NpcGuild => NpcGuild.ThievesGuild;

        public override TimeSpan JoinAge => TimeSpan.FromDays(7.0);

        public override void InitOutfit()
        {
            base.InitOutfit();

            if (Utility.RandomBool())
            {
                AddItem(new Kryss());
            }
            else
            {
                AddItem(new Dagger());
            }
        }

        public override bool CheckCustomReqs(PlayerMobile pm)
        {
            if (pm.Young)
            {
                SayTo(pm, 502089); // You cannot be a member of the Thieves' Guild while you are Young.
                return false;
            }

            if (pm.Kills > 0)
            {
                SayTo(pm, 501050); // This guild is for cunning thieves, not oafish cutthroats.
                return false;
            }

            if (pm.Skills.Stealing.Base < 60.0)
            {
                SayTo(pm, 501051); // You must be at least a journeyman pickpocket to join this elite organization.
                return false;
            }

            return true;
        }

        public override void SayWelcomeTo(Mobile m)
        {
            SayTo(m, 1008053); // Welcome to the guild! Stay to the shadows, friend.
        }

        public override bool HandlesOnSpeech(Mobile from)
        {
            if (from.InRange(Location, 2))
            {
                return true;
            }

            return base.HandlesOnSpeech(from);
        }

        public override void OnSpeech(SpeechEventArgs e)
        {
            var from = e.Mobile;

            if (!e.Handled && from is PlayerMobile pm && pm.InRange(Location, 2) && e.HasKeyword(0x1F)) // *disguise*
            {
                if (pm.NpcGuild == NpcGuild.ThievesGuild)
                {
                    SayTo(pm, 501839); // That particular item costs 700 gold pieces.
                }
                else
                {
                    SayTo(pm, 501838); // I don't know what you're talking about.
                }

                e.Handled = true;
            }

            base.OnSpeech(e);
        }

        public override bool OnGoldGiven(Mobile from, Gold dropped)
        {
            if (from is PlayerMobile pm && dropped.Amount == 700)
            {
                if (pm.NpcGuild == NpcGuild.ThievesGuild)
                {
                    pm.AddToBackpack(new DisguiseKit());

                    dropped.Delete();
                    return true;
                }
            }

            return base.OnGoldGiven(from, dropped);
        }
    }
}
