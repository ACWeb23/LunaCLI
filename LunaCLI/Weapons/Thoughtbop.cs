namespace Zenless
{
    class Thoughtbop : Weapon
    {
        public Thoughtbop()
            : base(21040, "Thoughtbop", 713, advancedStats.EnergyRegen, 0.60)
        {
            ImagePath = Portrait("Thoughtbop.webp");
            // Refinement 1: A Physical EX Special grants squad DMG +12.5% for 40s, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.125,
                    applyConditions.EX_SP_ATK, 40,
                    target.squad)
            });
        }
    }
}
