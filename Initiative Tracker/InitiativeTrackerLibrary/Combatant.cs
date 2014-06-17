using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InitiativeTrackerLibrary
{
    public class Combatant
    {
        #region Variables
        static Random random;
        #endregion

        #region Properties
        public int Initiative { get; set; }
        public String Name { get; set; }
        protected static Random Random
        {
            get
            {
                if (random == null)
                    random = new Random();

                return random;
            }
        }
        #endregion

        #region Constructors
        public Combatant()
        {
            Name = "Combatant";
            Initiative = 0;
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
        #endregion
    }
}
