using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeTrackerLibrary
{
    public enum DD4ESize
    {
        Medium = 0,
        Tiny,
        Small,
        Large,
        Huge,
        Gargantuan
    }

    public class DD4ECombatant : Combatant
    {
        #region Variables
        int baseArmorClass;
        int baseFortitude;
        int baseReflex;
        int baseWill;
        List<DD4EStatusEffect> statusEffects;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the combatant's size category.
        /// </summary>
        public DD4ESize Size { get; set; }
        public int TemporaryHP { get; set; }
        /// <summary>
        /// Returns the Combatant's current health points (HP).
        /// </summary>
        public int CurrentHP { get; protected set; }
        public int MaxHP { get; protected set; }
        public int InitiativeBonus { get; set; }
        public List<DD4EStatusEffect> StatusEffects
        {
            protected set { statusEffects = value; }
            get
            {
                if (statusEffects == null)
                    statusEffects = new List<DD4EStatusEffect>();

                return statusEffects;
            }
        }
        public bool IsBloodied { get { return CurrentHP < MaxHP / 2; } }
        #endregion

        #region Constructors
        public DD4ECombatant(string name, int maxHP, int ac, int fort, int refl, int will, int initBonus)
            : base(name)
        {
            MaxHP = maxHP;
            CurrentHP = MaxHP;
            baseArmorClass = ac;
            baseFortitude = fort;
            baseReflex = refl;
            baseWill = will;
            InitiativeBonus = initBonus;
        }
        #endregion

        #region Methods
        public void StartTurn()
        {
            // Take ongoing Damage
            foreach (DD4EStatusEffect status in StatusEffects.FindAll(s => s.Type == DD43StatusEffectType.OngoingDamage))
            {
                var damageStatus = (status as DD4EDamageModifier);
                if (damageStatus != null)
                {
                    TakeDamage(damageStatus.DamageType, damageStatus.DamageAmount);
                }
            }

            // End effects
            StatusEffects.RemoveAll(s => s.Duration == DD4EStatusEffectDuration.StartOfMyTurn);
        }
        public void OtherStartTurn(string combatant)
        {
            StatusEffects.RemoveAll(s => s.Duration == DD4EStatusEffectDuration.StartOfMyTurn && s.Source.Equals(combatant));
        }
        public void EndTurn()
        {
            // Regenerate Health
            foreach (DD4EStatusEffect status in StatusEffects.FindAll(s => s.Type == DD43StatusEffectType.Regeneration))
            {
                var damageStatus = (status as DD4EDamageModifier);
                if (damageStatus != null)
                {
                    Heal(damageStatus.DamageAmount);
                }
            }

            // End Effects
            StatusEffects.RemoveAll(s => s.Duration == DD4EStatusEffectDuration.EndOfMyTurn);

            for (int i = 0; i < StatusEffects.Count; i++)
            {
                if (StatusEffects[i].Duration == DD4EStatusEffectDuration.EndOfMyNextTurn)
                    StatusEffects[i].Duration = DD4EStatusEffectDuration.EndOfMyTurn;

                if (StatusEffects[i].Duration == DD4EStatusEffectDuration.EndOfMyNextTurnSustain)
                    StatusEffects[i].Duration = DD4EStatusEffectDuration.EndOfMyTurnSustain;
            }
        }
        public void OtherEndTurn(string combatant)
        {
            StatusEffects.RemoveAll(s => s.Duration == DD4EStatusEffectDuration.EndOfMyTurn && s.Source.Equals(combatant));

            for (int i = 0; i < StatusEffects.Count; i++)
            {
                if (StatusEffects[i].Duration == DD4EStatusEffectDuration.EndOfSourceNextTurn && StatusEffects[i].Source.Equals(combatant))
                    StatusEffects[i].Duration = DD4EStatusEffectDuration.EndOfSourceTurn;
            }
        }
        public void Heal(int health)
        {
            CurrentHP = Math.Min(MaxHP, CurrentHP + health);
        }
        public void TakeDamage(DD4EDamageType damageType, int damage)
        {
            int incomingDamage = damage;

            // Find Vulnerability Amount
            var vulnerabilityModifier = 0;
            foreach (DD4EStatusEffect status in StatusEffects.FindAll(s => s.Type == DD43StatusEffectType.Vulnerability))
            {
                var damageStatus = (status as DD4EDamageModifier);
                if (damageStatus != null)
                {
                    if ((damageStatus.DamageType & damageType) != DD4EDamageType.None)
                        vulnerabilityModifier = Math.Max(vulnerabilityModifier, damageStatus.DamageAmount);
                }
            }
            incomingDamage += vulnerabilityModifier;

            // Apply Resitances
            var resistanceModifier = 0;
            foreach (DD4EStatusEffect status in StatusEffects.FindAll(s => s.Type == DD43StatusEffectType.Resistance))
            {
                var damageStatus = (status as DD4EDamageModifier);
                if (damageStatus != null)
                {
                    if ((damageStatus.DamageType & damageType) != DD4EDamageType.None)
                        resistanceModifier = Math.Max(resistanceModifier, damageStatus.DamageAmount);
                }
            }
            incomingDamage -= resistanceModifier;

            if (TemporaryHP > 0)
            {
                if (TemporaryHP > incomingDamage)
                {
                    TemporaryHP -= incomingDamage;
                    incomingDamage = 0;
                }
                else
                {
                    incomingDamage -= TemporaryHP;
                    TemporaryHP = 0;
                }
            }

            if (incomingDamage > 0)
            {
                CurrentHP = Math.Max(0, CurrentHP - incomingDamage);
            }

            if (CurrentHP <= 0)
            {
                // Remove Regeneration and Ongoing Damage
                StatusEffects.RemoveAll(s => (s.Type == DD43StatusEffectType.OngoingDamage || s.Type == DD43StatusEffectType.Regeneration));
            }
        }
        public void ApplyStatusEffect(DD4EStatusEffect newStatusEffect)
        {
            // Extend the duration of Status Effect with similar effect
            if (StatusEffects.Exists(s => s.IsSameEffect(newStatusEffect)))
            {
                var newDamageEffect = (newStatusEffect as DD4EDamageModifier);
                if (newDamageEffect != null)
                {
                    var index = StatusEffects.IndexOf(StatusEffects.Find(s => s.IsSameEffect(newStatusEffect)));
                    if (newDamageEffect.DamageAmount > (StatusEffects[index] as DD4EDamageModifier).DamageAmount)
                        StatusEffects[index] = newStatusEffect;
                }
                else
                {
                    var index = StatusEffects.IndexOf(StatusEffects.Find(s => s.IsSameEffect(newStatusEffect)));
                    StatusEffects[index].Duration = newStatusEffect.Duration;
                }
            }
            else
            {
                    StatusEffects.Add(newStatusEffect);
            }
        }
        public override void RollInitiative()
        {
            Initiative = Random.Next(1, 20) + InitiativeBonus;
        }
        #endregion
    }
}
