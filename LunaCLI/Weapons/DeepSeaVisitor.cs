namespace Zenless
{
    class DeepSeaVisitor : Weapon
    {
        public DeepSeaVisitor()
            : base(21009, "Deep Sea Visitor", 713, advancedStats.Crit_Rate, 0.24)
        {
            ImagePath = Portrait("Deep_Sea_Visitor.webp");
            // Refinement 1: Ice DMG +25%. A Basic Attack and an Ice Dash Attack each grant CRIT Rate +10%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Ice, 0.25,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.10,
                    damageBonus.nill, 0,
                    applyConditions.BasicATK, 8,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.10,
                    damageBonus.nill, 0,
                    applyConditions.dashATK, 15,
                    target.self)
            });
        }
    }
}
