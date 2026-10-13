namespace Zenless
{
    class AngelInTheShell : Weapon
    {
        public AngelInTheShell()
            : base(21001, "Angel in the Shell", 713, advancedStats.Anomaly_Mastery, 0.30)
        {
            ImagePath = Portrait("Angel_in_the_Shell.webp");
            // Refinement 1: Anomaly Proficiency +90. On-field or on a Special, DMG against Anomaly-afflicted enemies +20% and Anomaly DMG +10%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Proficiency, 90,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.20,
                    applyConditions.EX_SP_ATK, 15,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Anomaly, 0.10,
                    applyConditions.EX_SP_ATK, 15,
                    target.self)
            });
        }
    }
}
