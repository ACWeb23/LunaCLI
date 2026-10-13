namespace Zenless
{
    class ElegantVanity : Weapon
    {
        public ElegantVanity()
            : base(21011, "Elegant Vanity", 713, advancedStats.ATK_Percent, 0.30)
        {
            ImagePath = Portrait("Elegant_Vanity.webp");
            // Refinement 1: Spending 25 or more Energy grants squad DMG +10% for 20s, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.10,
                    applyConditions.EX_SP_ATK, 20,
                    target.squad)
            });
        }
    }
}
