namespace Zenless
{
    class BlazingLaurel : Weapon
    {
        public BlazingLaurel()
            : base(21003, "Blazing Laurel", 713, advancedStats.Impact, 0.18)
        {
            ImagePath = Portrait("Blazing_Laurel.webp");
            // Refinement 1: Quick Assist or Perfect Assist grants Impact +25% for 8s.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Impact, 0.25,
                    damageBonus.nill, 0,
                    applyConditions.Quick_Assist, 8,
                    target.self)
            });
        }
    }
}
