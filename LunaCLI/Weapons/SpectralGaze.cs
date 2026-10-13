namespace Zenless
{
    class SpectralGaze : Weapon
    {
        public SpectralGaze()
            : base(21035, "Spectral Gaze", 713, advancedStats.Crit_Rate, 0.24)
        {
            ImagePath = Portrait("Spectral_Gaze.webp");
            // Refinement 1: An Electric Aftershock grants Impact +4% per stack, up to 3.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Impact, 0.04,
                    damageBonus.nill, 0,
                    applyConditions.Aftershock, 12,
                    target.self)
            });
        }
    }
}
