namespace Zenless
{
    class FlightOfFancy : Weapon
    {
        public FlightOfFancy()
            : base(21013, "Flight of Fancy", 713, advancedStats.Anomaly_Proficiency, 90)
        {
            ImagePath = Portrait("Flight_of_Fancy.webp");
            // Refinement 1: Anomaly Buildup Rate +40%. Ether DMG grants Anomaly Proficiency +20, up to 6 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Mastery, 0.40,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Proficiency, 20,
                    damageBonus.nill, 0,
                    applyConditions.BasicATK, 5,
                    target.self)
            });
        }
    }
}
