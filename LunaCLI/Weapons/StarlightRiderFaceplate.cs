namespace Zenless
{
    class StarlightRiderFaceplate : Weapon
    {
        public StarlightRiderFaceplate()
            : base(21036, "Starlight Rider Faceplate", 713, advancedStats.HP_Percent, 0.30)
        {
            ImagePath = Portrait("Starlight_Rider_Faceplate.webp");
            // Refinement 1: CRIT Rate +20%. A Special Attack grants Physical Sheer DMG +10%, up to 2 stacks.
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
                    damageBonus.Physical, 0.10,
                    applyConditions.EX_SP_ATK, 30,
                    target.self)
            });
        }
    }
}
