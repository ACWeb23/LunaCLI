namespace Zenless
{
    class IceJadeTeapot : Weapon
    {
        public IceJadeTeapot()
            : base(21020, "Ice-Jade Teapot", 713, advancedStats.Impact, 0.18)
        {
            ImagePath = Portrait("Ice-Jade_Teapot.webp");
            // Refinement 1: A Basic Attack grants Impact +0.7% per stack. At 15 stacks, squad DMG +20%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Impact, 0.007,
                    damageBonus.nill, 0,
                    applyConditions.BasicATK, 8,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.20,
                    applyConditions.max_Stacks, 10,
                    target.squad)
            });
        }
    }
}
