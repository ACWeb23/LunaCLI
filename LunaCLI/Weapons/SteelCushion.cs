namespace Zenless
{
    class SteelCushion : Weapon
    {
        public SteelCushion()
            : base(21037, "Steel Cushion", 684, advancedStats.Crit_Rate, 0.24)
        {
            ImagePath = Portrait("Steel_Cushion.webp");
            // Refinement 1: Physical DMG +20%. Hits from behind deal 25% more DMG.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Physical, 0.20,
                    applyConditions.Always, 0,
                    target.self)
            });
        }
    }
}
