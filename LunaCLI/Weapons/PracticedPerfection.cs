namespace Zenless
{
    class PracticedPerfection : Weapon
    {
        public PracticedPerfection()
            : base(21027, "Practiced Perfection", 713, advancedStats.ATK_Percent, 0.30)
        {
            ImagePath = Portrait("Practiced_Perfection.webp");
            // Refinement 1: Inflicting Assault grants Physical DMG +20% for 20s, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Physical, 0.20,
                    applyConditions.Assault, 20,
                    target.self)
            });
        }
    }
}
