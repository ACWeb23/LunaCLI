using System.Collections.Generic;

namespace Zenless
{
    class Ode_Of_Resurrected_Wings : Weapon
    {
        private double AP;
        private Dictionary<string, double> Passive;

        public Ode_Of_Resurrected_Wings()
            : base(14158, "Ode Of Resurrected Wings", 743, advancedStats.ATK_Percent, 0.36)
        {
            // (Original code declared new local variables here, which shadowed the fields
            //  and left them at 0/null. Assigning to the fields fixes that.)
            AP = 96;

            // passive
            /*
             * if Refringe Special damage +20%
             * Damage Bonus + 30%
             * Cooldown 30s
             */
            Passive = new Dictionary<string, double>
            {
                { "Refringe", 1 },
                { "Special Damage Bonus", 20 },
                { "All Damage Bonus", 30 },
                { "Cooldown", 30 }
            };
        }

        public (double, Dictionary<string, double>) returnPassive()
        {
            return (AP, Passive);
        }
    }
}
