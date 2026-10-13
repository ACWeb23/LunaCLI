namespace Zenless
{
    class HailstormShrine : Weapon
    {
        public HailstormShrine()
            : base(21016, "Hailstorm Shrine", 743, advancedStats.Crit_Rate, 0.24)
        {
            ImagePath = Portrait("Hailstorm_Shrine.webp");
            // Refinement 1: CRIT DMG +50%. An EX Special or a squad Anomaly grants Ice DMG +20% for 15s, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.50,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Ice, 0.20,
                    applyConditions.EX_SP_ATK, 15,
                    target.self)
            });
        }
    }
}
