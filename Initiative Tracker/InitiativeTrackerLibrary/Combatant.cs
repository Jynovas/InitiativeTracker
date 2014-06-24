using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeTrackerLibrary
{
    public class Combatant : INotifyPropertyChanged
    {
        #region Variables
        static Random random;
        int initiative;
        #endregion

        #region Properties
        public int Initiative
        {
            get { return initiative; }
            set
            {
                initiative = value;
                OnPropertyChanged("Initiative");
            }
        }
        public String Name { get; set; }
        public int ID { get; set; }
        public bool IsPlayer { get; set; }
        protected static Random Random
        {
            get
            {
                if (random == null)
                    random = new Random();

                return random;
            }
        }
        public String CombatName
        {
            get
            {
                if (IsPlayer)
                {
                    return Name;
                }
                else
                {
                    return Name + " [" + ID + "]";
                }
            }
        }
        #endregion

        #region Events
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        #region Constructors
        public Combatant()
        {
            Name = "Combatant";
            Initiative = 0;
            ID = 0;
        }
        public Combatant(string name)
        {
            Name = name;
        }
        public Combatant(string name, int init)
            : this(name)
        {
            Initiative = init;
        }
        #endregion

        #region Methods
        public virtual void RollInitiative()
        {
            Initiative = Random.Next();
        }

        #region Event Handling
        protected virtual void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }
        protected void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            if (PropertyChanged != null)
                PropertyChanged.Invoke(this, args);
        }
        #endregion
        #endregion
    }
}
