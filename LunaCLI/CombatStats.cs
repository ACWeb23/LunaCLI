namespace Zenless
{
    /// <summary>
    /// Final combat numbers for one agent after gear and permanent set buffs.
    /// Ratios are fractions (0.05 = 5%). HP, ATK, DEF, Impact, and Anomaly stats are absolute.
    /// </summary>
    sealed class CombatStats
    {
        public double HP { get; init; }
        public double ATK { get; init; }
        public double DEF { get; init; }
        public double Impact { get; init; }
        public double CritRate { get; init; }
        public double CritDamage { get; init; }
        public double AnomalyProficiency { get; init; }
        public double AnomalyMastery { get; init; }
        public double PenRatio { get; init; }
        public double EnergyRegen { get; init; }
        public double DamageBonus { get; init; }
    }
}
