namespace Zenless
{
    class TheBrimstone : Weapon
    {
        public TheBrimstone()
            : base(21038, "The Brimstone", 684, advancedStats.ATK_Percent, 0.30)
        {
            ImagePath = Portrait("The_Brimstone.webp");
            // Refinement 1: A Basic Attack, Dash Attack, or Dodge Counter grants ATK +3.5% for 8s, up to 8 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.ATK_Percent, 0.035,
                    damageBonus.nill, 0,
                    applyConditions.BasicATK, 8,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.ATK_Percent, 0.035,
                    damageBonus.nill, 0,
                    applyConditions.dashATK, 8,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.ATK_Percent, 0.035,
                    damageBonus.nill, 0,
                    applyConditions.Dodge_Counter, 8,
                    target.self)
            });
        }
    }
}
