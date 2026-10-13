namespace Zenless
{
    class KnightsExtolment : Weapon
    {
        public KnightsExtolment()
            : base(21022, "Knight's Extolment", 713, advancedStats.Crit_Damage, 0.48)
        {
            ImagePath = Portrait("Knight%27s_Extolment.webp");
            // Refinement 1: A heavy Basic Attack or EX Special grants CRIT DMG +32% for 25s, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.32,
                    damageBonus.nill, 0,
                    applyConditions.BasicATK, 25,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.32,
                    damageBonus.nill, 0,
                    applyConditions.EX_SP_ATK, 25,
                    target.self)
            });
        }
    }
}
