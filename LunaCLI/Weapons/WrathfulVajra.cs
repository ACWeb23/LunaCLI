namespace Zenless
{
    class WrathfulVajra : Weapon
    {
        public WrathfulVajra()
            : base(21044, "Wrathful Vajra", 713, advancedStats.HP_Percent, 0.30)
        {
            ImagePath = Portrait("Wrathful_Vajra.webp");
            // Refinement 1: CRIT Rate +20%. An EX Special grants Fire Sheer DMG +9%, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.20,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Fire, 0.09,
                    applyConditions.EX_SP_ATK, 20,
                    target.self)
            });
        }
    }
}
