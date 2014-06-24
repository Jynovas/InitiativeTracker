using System;
using System.Collections.Generic;
using System.ComponentModel;
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

    public partial class DD4ECombatant : Combatant
    {
        #region Variables
        int baseArmorClass;
        int baseFortitude;
        int baseReflex;
        int baseWill;
        List<DD4EStatusEffect> statusEffects;
        int temporaryHealthPoints;
        int currentHealthPoints;
        string figurine;
        #endregion

        #region Properties
        /// <summary>
        /// Returns the combatant's size category.
        /// </summary>
        public DD4ESize Size { get; set; }
        public int TemporaryHP
        {
            get { return temporaryHealthPoints; }
            set
            {
                temporaryHealthPoints = value;
                OnPropertyChanged("TemporaryHP");
            }
        }
        /// <summary>
        /// Returns the Combatant's current health points (HP).
        /// </summary>
        public int CurrentHP
        {
            get { return currentHealthPoints; }
            protected set
            {
                currentHealthPoints = value;
                OnPropertyChanged("CurrentHP");
            }
        }
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
        public bool IsBloodied { get { return CurrentHP <= MaxHP / 2; } }
        public int ArmorClass
        {
            get
            {
                int armorClass = baseArmorClass;

                return armorClass;
            }
        }
        public int Fortitude
        {
            get
            {
                int fortitude = baseFortitude;

                return fortitude;
            }
        }
        public int Reflex
        {
            get
            {
                int reflex = baseReflex;

                return reflex;
            }
        }
        public int Will
        {
            get
            {
                int will = baseWill;

                return will;
            }
        }
        public String Figurine
        {
            get { return figurine; }
            set
            {
                figurine = value;
                OnPropertyChanged("Figurine");
            }
        }
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
            foreach (DD4EStatusEffect status in StatusEffects.FindAll(s => s.Type == DD4EStatusEffectType.OngoingDamage))
            {
                var damageStatus = (status as DD4EDamageModifier);
                if (damageStatus != null)
                {
                    TakeDamage(damageStatus.DamageType, damageStatus.DamageAmount);
                }
            }

            // End effects
            StatusEffects.RemoveAll(s => s.Duration == DD4EStatusEffectDuration.StartOfMyTurn);

            OnPropertyChanged("StatusEffects");
        }
        public void OtherStartTurn(string combatant)
        {
            StatusEffects.RemoveAll(s => s.Duration == DD4EStatusEffectDuration.StartOfMyTurn && s.Source.Equals(combatant));
            OnPropertyChanged("StatusEffects");
        }
        public void EndTurn()
        {
            // Regenerate Health
            foreach (DD4EStatusEffect status in StatusEffects.FindAll(s => s.Type == DD4EStatusEffectType.Regeneration))
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

            OnPropertyChanged("StatusEffects");
        }
        public void OtherEndTurn(string combatant)
        {
            StatusEffects.RemoveAll(s => s.Duration == DD4EStatusEffectDuration.EndOfMyTurn && s.Source.Equals(combatant));

            for (int i = 0; i < StatusEffects.Count; i++)
            {
                if (StatusEffects[i].Duration == DD4EStatusEffectDuration.EndOfSourceNextTurn && StatusEffects[i].Source.Equals(combatant))
                    StatusEffects[i].Duration = DD4EStatusEffectDuration.EndOfSourceTurn;
            }

            OnPropertyChanged("StatusEffects");
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
            foreach (DD4EStatusEffect status in StatusEffects.FindAll(s => s.Type == DD4EStatusEffectType.Vulnerability))
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
            // 1) Build List of Resistance
            var resistanceModifier = Int32.MaxValue; 
            DD4EDamageType resistanceList = DD4EDamageType.None;
            foreach (DD4EStatusEffect status in StatusEffects.FindAll(s => s.Type == DD4EStatusEffectType.Resistance))
            {
                var damageStatus = (status as DD4EDamageModifier);
                if (damageStatus != null)
                {
                    resistanceList |= damageStatus.DamageType;
                }
            }

            // 2) Figure out if they resist everything
            if ((resistanceList & damageType) == damageType)
            {
                // 3) If they resist everything, find lowest resistance value and do so
                foreach (DD4EStatusEffect status in StatusEffects.FindAll(s => s.Type == DD4EStatusEffectType.Resistance))
                {
                    var damageStatus = (status as DD4EDamageModifier);
                    if (damageStatus != null)
                    {
                        if ((damageStatus.DamageType & damageType) != DD4EDamageType.None)
                            resistanceModifier = Math.Min(resistanceModifier, damageStatus.DamageAmount);
                    }
                }

                // 4) If they have a resist all use the maximum between all and the current resistance
                if (StatusEffects.Exists(s => s.Type == DD4EStatusEffectType.Resistance && (s as DD4EDamageModifier).DamageType == DD4EDamageType.All))
                    resistanceModifier = Math.Max(resistanceModifier, (StatusEffects.Find(s => s.Type == DD4EStatusEffectType.Resistance && (s as DD4EDamageModifier).DamageType == DD4EDamageType.All) as DD4EDamageModifier).DamageAmount);

                // 5) Apply Resistance
                incomingDamage -= resistanceModifier;
            }

            if (incomingDamage > 0)
            {
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
            }

            if (incomingDamage > 0)
            {
                CurrentHP = Math.Max(0, CurrentHP - incomingDamage);
            }

            if (CurrentHP <= 0)
            {
                // Remove Regeneration and Ongoing Damage
                StatusEffects.RemoveAll(s => (s.Type == DD4EStatusEffectType.OngoingDamage || s.Type == DD4EStatusEffectType.Regeneration));
                OnPropertyChanged("StatusEffects");
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
            OnPropertyChanged("StatusEffects");
        }
        public override void RollInitiative()
        {
            Initiative = Random.Next(1, 20) + InitiativeBonus;
            OnPropertyChanged("Initiative");
        }
        #endregion
    }
}
