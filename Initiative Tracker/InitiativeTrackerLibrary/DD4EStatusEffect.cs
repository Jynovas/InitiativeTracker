using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeTrackerLibrary
{
    public enum DD4EStatusEffectType
    {
        [Description("Ongoing Damage")]
        OngoingDamage,
        Regeneration,
        ArmorClassUp,
        ArmorClassDown,
        FortitudeUp,
        FortitudeDown,
        ReflexUp,
        ReflexDown,
        WillUp,
        WillDown,
        DefenseUp,
        DefenseDown,
        DamageUp,
        DamageDown,
        AccuracyUp,
        AccuracyDown,
        Resistance,
        Vulnerability,
        Blinded,
        Dazed,
        Deafened,
        Dominated,
        Helpless,
        Immobilized,
        Marked,
        Petrified,
        Prone,
        Restrained,
        Slowed,
        Stunned,
        Surprised,
        Unconscious,
        Weakened
    }

    public enum DD4EStatusEffectDuration
    {
        StartOfSourceTurn,
        StartOfMyTurn,
        EndOfSourceTurn,
        EndOfSourceNextTurn,
        EndOfMyTurn,
        EndOfMyNextTurn,
        EndOfMyTurnSustain,
        EndOfMyNextTurnSustain,
        EndOfEncounter,
        SaveEnds
    }

    public class DD4EStatusEffect
    {
        /// <summary>
        /// The type of status effect being inflicted.
        /// </summary>
        public DD4EStatusEffectType Type { get; protected set; }
        /// <summary>
        /// Combatant who caused the status effect.
        /// </summary>
        public String Source { get; protected set; }
        /// <summary>
        /// Ending point of status effect.
        /// </summary>
        public DD4EStatusEffectDuration Duration { get; set; }
        /// <summary>
        /// Modifier in case of use of save ends. (Most should be zero (0)).
        /// </summary>
        public int SaveModifier { get; set; }

        public DD4EStatusEffect(DD4EStatusEffectType type, string source, DD4EStatusEffectDuration duration)
        {
            Type = type;
            Source = source;
            Duration = duration;
        }

        public virtual bool IsSameEffect(DD4EStatusEffect other)
        {
            return this.Type == other.Type;
        }
    }

    public class DD4EOngoingDamage : DD4EDamageModifier
    {
        public DD4EOngoingDamage(string source, DD4EDamageType damageType, int damageAmount, DD4EStatusEffectDuration duration)
            : base(DD4EStatusEffectType.OngoingDamage, source, damageType, damageAmount, duration)
        {

        }
        public DD4EOngoingDamage(string source, DD4EDamageType damageType, int damageAmount)
            : this(source, damageType, damageAmount, DD4EStatusEffectDuration.SaveEnds)
        {

        }
    }

    public class DD4ERegeneration : DD4EDamageModifier
    {
        public DD4ERegeneration(string source, int damageAmount, DD4EStatusEffectDuration duration)
            : base(DD4EStatusEffectType.OngoingDamage, source, DD4EDamageType.None, damageAmount, duration)
        {

        }
        public DD4ERegeneration(string source, int damageAmount)
            : this(source, damageAmount, DD4EStatusEffectDuration.EndOfEncounter)
        {

        }

        public override bool IsSameEffect(DD4EStatusEffect other)
        {
            return this.Type == other.Type;
        }
    }

    public class DD4EDamageModifier : DD4EStatusEffect
    {
        public DD4EDamageType DamageType { get; protected set; }
        public int DamageAmount { get; protected set; }

        public DD4EDamageModifier(DD4EStatusEffectType type, string source, DD4EDamageType damageType, int damageAmount, DD4EStatusEffectDuration duration)
            : base(type, source, duration)
        {
            DamageType = damageType;
            DamageAmount = damageAmount;
        }

        public override bool IsSameEffect(DD4EStatusEffect other)
        {
            if (base.IsSameEffect(other))
            {
                var otherDamage = (other as DD4EDamageModifier);
                if (otherDamage != null)
                    return this.DamageType == otherDamage.DamageType;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
    }
}
