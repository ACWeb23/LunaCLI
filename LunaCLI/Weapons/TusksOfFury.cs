namespace Zenless
{
    class TusksOfFury : Weapon
    {
        public TusksOfFury()
            : base(21042, "Tusks of Fury", 713, advancedStats.Impact, 0.18)
        {
            ImagePath = Portrait("Tusks_of_Fury.webp");
            // Refinement 1: An Interrupt or Perfect Dodge grants squad DMG +18% for 20s.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.18,
                    applyConditions.Defensive_Assist, 20,
                    target.squad)
            });
        }
    }
}
