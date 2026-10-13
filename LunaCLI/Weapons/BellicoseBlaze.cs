namespace Zenless
{
    class BellicoseBlaze : Weapon
    {
        public BellicoseBlaze()
            : base(21002, "Bellicose Blaze", 713, advancedStats.EnergyRegen, 0.60)
        {
            ImagePath = Portrait("Bellicose_Blaze.webp");
            // Refinement 1: CRIT Rate +20%. A Fire Aftershock ignores a portion of DEF, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.20,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self)
            });
        }
    }
}
