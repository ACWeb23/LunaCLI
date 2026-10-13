namespace Zenless
{
    class HellfireGears : Weapon
    {
        public HellfireGears()
            : base(21019, "Hellfire Gears", 684, advancedStats.Impact, 0.18)
        {
            ImagePath = Portrait("Hellfire_Gears.webp");
            // Refinement 1: An EX Special grants Impact +10% for 10s, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Impact, 0.10,
                    damageBonus.nill, 0,
                    applyConditions.EX_SP_ATK, 10,
                    target.self)
            });
        }
    }
}
