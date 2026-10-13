namespace Zenless
{
    class Ode_Of_Resurrected_Wings : Weapon
    {
        public Ode_Of_Resurrected_Wings()
            : base(14158, "Ode Of Resurrected Wings", 743, advancedStats.ATK_Percent, 0.36)
        {
            ImagePath = Portrait("Ode_of_Resurrected_Wings.webp");
            // Refinement 1: Anomaly Proficiency +96.
            // A Refringe reaction grants Anomaly DMG +20% and squad DMG +30% for 30s.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Proficiency, 96,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Anomaly, 0.20,
                    applyConditions.Refringe, 30,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.30,
                    applyConditions.Refringe, 30,
                    target.squad)
            });
        }
    }
}
