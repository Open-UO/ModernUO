using System;
using Server.Targeting;

namespace Server.Spells.Sixth
{
    public class ExplosionSpell : MagerySpell, ISpellTargetingMobile
    {
        private static readonly SpellInfo _info = new(
            "Explosion",
            "Vas Ort Flam",
            230,
            9041,
            Reagent.Bloodmoss,
            Reagent.MandrakeRoot
        );

        public ExplosionSpell(Mobile caster, Item scroll = null) : base(caster, scroll, _info)
        {
        }

        public override SpellCircle Circle => SpellCircle.Sixth;

        public override Type[] DelayedDamageSpellFamilyStacking => AOSNoDelayedDamageStackingSelf;

        public override bool DelayedDamage => false;

        public void Target(Mobile m)
        {
            if (Core.SA && HasDelayedDamageContext(m))
            {
                DoHurtFizzle();
                return;
            }

            if (Caster.CanBeHarmful(m) && CheckSequence())
            {
                Mobile defender = m;

                SpellHelper.Turn(Caster, m);
                SpellHelper.CheckReflect((int)Circle, Caster, ref m);

                var t = new InternalTimer(this, Caster, defender, m).Start();
            }

            FinishSequence();
        }

        public override void OnCast()
        {
            Caster.Target = new SpellTargetMobile(this, TargetFlags.Harmful, Core.ML ? 10 : 12);
        }

        private class InternalTimer : Timer
        {
            private readonly Mobile m_Attacker;
            private readonly Mobile m_Defender;
            private readonly MagerySpell m_Spell;
            private readonly Mobile m_Target;

            public InternalTimer(MagerySpell spell, Mobile attacker, Mobile defender, Mobile target)
                : base(TimeSpan.FromSeconds(Core.AOS ? 3.0 : 2.5))
            {
                m_Spell = spell;
                m_Attacker = attacker;
                m_Defender = defender;
                m_Target = target;

                m_Spell?.StartDelayedDamageContext(m_Attacker, this);
            }

            protected override void OnTick()
            {
                if (m_Attacker.HarmfulCheck(m_Defender))
                {
                    double damage;

                    if (Core.AOS)
                    {
                        damage = m_Spell.GetNewAosDamage(40, 1, 5, m_Defender);
                    }
                    else
                    {
                        damage = Utility.Random(23, 22);

                        if (m_Spell.CheckResisted(m_Target))
                        {
                            damage *= 0.75;

                            m_Target.SendLocalizedMessage(501783); // You feel yourself resisting magical energy.
                        }

                        damage *= m_Spell.GetDamageScalar(m_Target);
                    }

                    m_Target.FixedParticles(0x36BD, 20, 10, 5044, EffectLayer.Head);
                    m_Target.PlaySound(0x307);

                    SpellHelper.Damage(m_Spell, m_Target, damage, 0, 100, 0, 0, 0);

                    m_Spell?.RemoveDelayedDamageContext(m_Attacker);
                }
            }
        }
    }
}
