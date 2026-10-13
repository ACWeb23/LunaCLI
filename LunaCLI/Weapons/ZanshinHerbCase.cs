namespace Zenless
{
    class ZanshinHerbCase : Weapon
    {
        public ZanshinHerbCase()
            : base(21046, "Zanshin Herb Case", 713, advancedStats.Crit_Damage, 0.48)
        {
            ImagePath = Portrait("Zanshin_Herb_Case.webp");
            // Refinement 1: CRIT Rate +10%. Dash Attack Electric DMG +40%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.10,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Electric, 0.40,
                    applyConditions.dashATK, 0,
                    target.self)
            });
        }
    }
}
