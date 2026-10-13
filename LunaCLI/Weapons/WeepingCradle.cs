namespace Zenless
{
    class WeepingCradle : Weapon
    {
        public WeepingCradle()
            : base(21043, "Weeping Cradle", 684, advancedStats.PEN_Ratio, 0.24)
        {
            ImagePath = Portrait("Weeping_Cradle.webp");
            // Refinement 1: An attack increases squad DMG against the target by 10% for 3s.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.10,
                    applyConditions.BasicATK, 3,
                    target.squad)
            });
        }
    }
}
