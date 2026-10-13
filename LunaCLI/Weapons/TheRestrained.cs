namespace Zenless
{
    class TheRestrained : Weapon
    {
        public TheRestrained()
            : base(21039, "The Restrained", 684, advancedStats.Impact, 0.18)
        {
            ImagePath = Portrait("The_Restrained.webp");
            // Refinement 1: A hit grants Basic Attack DMG +6% for 8s, up to 5 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Basic, 0.06,
                    applyConditions.BasicATK, 8,
                    target.self)
            });
        }
    }
}
