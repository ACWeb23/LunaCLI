namespace Zenless
{
    class QingmingBirdcage : Weapon
    {
        public QingmingBirdcage()
            : base(21028, "Qingming Birdcage", 743, advancedStats.HP_Percent, 0.30)
        {
            ImagePath = Portrait("Qingming_Birdcage.webp");
            // Refinement 1: CRIT Rate +20%. An EX Special grants Ether DMG +8% per stack, up to 2.
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
                    damageBonus.Ether, 0.08,
                    applyConditions.EX_SP_ATK, 15,
                    target.self)
            });
        }
    }
}
