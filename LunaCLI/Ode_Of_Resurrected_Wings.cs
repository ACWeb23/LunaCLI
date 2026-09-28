using System.Collections.Generic;

namespace Zenless
{
    class Ode_Of_Resurrected_Wings : Weapon
    {
        private Buff[] paradiseLost = [];

        public Ode_Of_Resurrected_Wings()
            : base(14158, "Ode Of Resurrected Wings", 743, advancedStats.ATK_Percent, 0.36)
        {
            // passive
            /* AP + 96
             * if Refringe Special damage +20%
             * Damage Bonus + 30%
             * Cooldown 30s
             */

            Buff apUP = new Buff(
                specialResource.nill, 0,
                advancedStats.Anomaly_Proficiency, 96,
                damageBonus.nill, 0,
                applyConditions.Always, 0,
                target.self
                );
            Buff Refringe = new Buff(
                specialResource.nill, 0,
                advancedStats.nill, 0,
                damageBonus.Anomaly, 20,
                applyConditions.Refringe, 30,
                target.self
                );
            Buff RefringeSquad = new Buff(
                specialResource.nill, 0,
                advancedStats.nill, 0,
                damageBonus.Anomaly, 30,
                applyConditions.Refringe, 30,
                target.squad
                );
            paradiseLost.Append(apUP);
            paradiseLost.Append(Refringe);
            paradiseLost.Append(RefringeSquad);

            setPassive(paradiseLost);
        }
    }
}
