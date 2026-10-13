namespace Zenless
{
    class RoaringFurnace : Weapon
    {
        public RoaringFurnace()
            : base(21030, "Roaring Fur-nace", 713, advancedStats.ATK_Percent, 0.30)
        {
            ImagePath = Portrait("Roaring_Fur-nace.webp");
            // Refinement 1: A Fire Chain Attack or Ultimate grants squad DMG +10% for 30s, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.10,
                    applyConditions.Chain_Attack, 30,
                    target.squad),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.10,
                    applyConditions.Ultimate, 30,
                    target.squad)
            });
        }
    }
}
