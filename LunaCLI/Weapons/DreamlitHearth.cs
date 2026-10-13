namespace Zenless
{
    class DreamlitHearth : Weapon
    {
        public DreamlitHearth()
            : base(21010, "Dreamlit Hearth", 713, advancedStats.HP_Percent, 0.30)
        {
            ImagePath = Portrait("Dreamlit_Hearth.webp");
            // Refinement 1: Activating an Ether Veil grants squad DMG +25% and Max HP +15% for 45s.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.25,
                    applyConditions.Ether_Veil, 45,
                    target.squad),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.HP_Percent, 0.15,
                    damageBonus.nill, 0,
                    applyConditions.Ether_Veil, 45,
                    target.squad)
            });
        }
    }
}
