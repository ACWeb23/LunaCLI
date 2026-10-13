namespace Zenless
{
    class ChiefSidekick : Weapon
    {
        public ChiefSidekick()
            : base(21004, "Chief Sidekick", 713, advancedStats.Crit_Rate, 0.24)
        {
            ImagePath = Portrait("Chief_Sidekick.webp");
            // Refinement 1: A Fire EX Special grants all agents DMG +12.5% for 30s, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.125,
                    applyConditions.EX_SP_ATK, 30,
                    target.squad)
            });
        }
    }
}
