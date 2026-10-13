namespace Zenless
{
    abstract partial class Disk_Drive
    {
        /// <summary>
        /// 2-piece and 4-piece effects for every drive set that has a portrait.
        /// Percent values are fractions. A conditional buff is listed on the agent and
        /// is not added to the combat totals until its condition is Always or an
        /// Anomaly Mastery threshold the wearer already meets.
        /// </summary>
        private static void DefineSetBonuses()
        {
            Define(driveSet.Feathered_Fate,
                new[] { Stat(advancedStats.Anomaly_Proficiency, 30, applyConditions.Always, 0) },
                new[]
                {
                    Stat(advancedStats.Anomaly_Proficiency, 50, applyConditions.EnterBattle, 15),
                    Damage(damageBonus.Anomaly, 0.15, applyConditions.EnterBattle, 15, target.self)
                });

            Define(driveSet.Freedom_Blues,
                new[] { Stat(advancedStats.Anomaly_Proficiency, 30, applyConditions.Always, 0) },
                new[] { Damage(damageBonus.Anomaly, 0.20, applyConditions.EX_SP_ATK, 8, target.enemy) });

            Define(driveSet.Shadow_Harmony,
                new[]
                {
                    Damage(damageBonus.All, 0.15, applyConditions.Aftershock, 0, target.self),
                    Damage(damageBonus.All, 0.15, applyConditions.dashATK, 0, target.self)
                },
                new[]
                {
                    Stat(advancedStats.ATK_Percent, 0.04, applyConditions.Aftershock, 15),
                    Stat(advancedStats.Crit_Rate, 0.04, applyConditions.Aftershock, 15),
                    Stat(advancedStats.ATK_Percent, 0.04, applyConditions.dashATK, 15),
                    Stat(advancedStats.Crit_Rate, 0.04, applyConditions.dashATK, 15)
                });

            Define(driveSet.Branch_And_Blade_Song,
                new[] { Stat(advancedStats.Crit_Damage, 0.16, applyConditions.Always, 0) },
                new[]
                {
                    new Buff(
                        specialResource.nill, 115,
                        advancedStats.Crit_Damage, 0.30,
                        damageBonus.nill, 0,
                        applyConditions.adv_Stat_Threshold, 0,
                        target.self),
                    Stat(advancedStats.Crit_Rate, 0.12, applyConditions.freeze, 15),
                    Stat(advancedStats.Crit_Rate, 0.12, applyConditions.shatter, 15)
                });

            Define(driveSet.Woodpecker_Electro,
                new[] { Stat(advancedStats.Crit_Rate, 0.08, applyConditions.Always, 0) },
                new[] { Stat(advancedStats.ATK_Percent, 0.09, applyConditions.Crit_Hit, 6) });

            Define(driveSet.Puffer_Electro,
                new[] { Stat(advancedStats.PEN_Ratio, 0.08, applyConditions.Always, 0) },
                new[]
                {
                    Damage(damageBonus.All, 0.20, applyConditions.Ultimate, 0, target.self),
                    Stat(advancedStats.ATK_Percent, 0.15, applyConditions.Ultimate, 12)
                });

            Define(driveSet.Shockstar_Disco,
                new[] { Stat(advancedStats.Impact, 0.06, applyConditions.Always, 0) },
                new[]
                {
                    Stat(advancedStats.Daze, 0.20, applyConditions.BasicATK, 0),
                    Stat(advancedStats.Daze, 0.20, applyConditions.dashATK, 0),
                    Stat(advancedStats.Daze, 0.20, applyConditions.Dodge_Counter, 0)
                });

            Define(driveSet.Hormone_Punk,
                new[] { Stat(advancedStats.ATK_Percent, 0.10, applyConditions.Always, 0) },
                new[]
                {
                    Stat(advancedStats.ATK_Percent, 0.25, applyConditions.EnterBattle, 10),
                    Stat(advancedStats.ATK_Percent, 0.25, applyConditions.Switch_In, 10)
                });

            Define(driveSet.Swing_Jazz,
                new[] { Stat(advancedStats.EnergyRegen, 0.20, applyConditions.Always, 0) },
                new[]
                {
                    Damage(damageBonus.All, 0.15, applyConditions.Chain_Attack, 12, target.squad),
                    Damage(damageBonus.All, 0.15, applyConditions.Ultimate, 12, target.squad)
                });

            Define(driveSet.Soul_Rock,
                new[] { Stat(advancedStats.DEF_Percent, 0.16, applyConditions.Always, 0) },
                new[] { Noted("After losing HP, DMG taken is reduced by 40% for 2.5s. Once every 15s.") });

            Define(driveSet.Inferno_Metal,
                new[] { Damage(damageBonus.Fire, 0.10, applyConditions.Always, 0, target.self) },
                new[] { Stat(advancedStats.Crit_Rate, 0.28, applyConditions.Burn, 8) });

            Define(driveSet.Chaotic_Metal,
                new[] { Damage(damageBonus.Ether, 0.10, applyConditions.Always, 0, target.self) },
                new[]
                {
                    Stat(advancedStats.Crit_Damage, 0.20, applyConditions.Always, 0),
                    Stat(advancedStats.Crit_Damage, 0.055, applyConditions.Corruption, 8)
                });

            Define(driveSet.Thunder_Metal,
                new[] { Damage(damageBonus.Electric, 0.10, applyConditions.Always, 0, target.self) },
                new[] { Stat(advancedStats.ATK_Percent, 0.28, applyConditions.Shock, 0) });

            Define(driveSet.Polar_Metal,
                new[] { Damage(damageBonus.Ice, 0.10, applyConditions.Always, 0, target.self) },
                new[]
                {
                    Damage(damageBonus.All, 0.20, applyConditions.BasicATK, 0, target.self),
                    Damage(damageBonus.All, 0.20, applyConditions.dashATK, 0, target.self),
                    Damage(damageBonus.All, 0.20, applyConditions.freeze, 12, target.self),
                    Damage(damageBonus.All, 0.20, applyConditions.shatter, 12, target.self)
                });

            Define(driveSet.Fanged_Metal,
                new[] { Damage(damageBonus.Physical, 0.10, applyConditions.Always, 0, target.self) },
                new[] { Damage(damageBonus.All, 0.35, applyConditions.Assault, 12, target.self) });

            Define(driveSet.Chaos_Jazz,
                new[] { Stat(advancedStats.Anomaly_Proficiency, 30, applyConditions.Always, 0) },
                new[]
                {
                    Damage(damageBonus.Fire, 0.15, applyConditions.Always, 0, target.self),
                    Damage(damageBonus.Electric, 0.15, applyConditions.Always, 0, target.self),
                    Damage(damageBonus.All, 0.20, applyConditions.off_Field, 5, target.self)
                });

            Define(driveSet.Proto_Punk,
                new[] { Stat(advancedStats.Shield_Effect, 0.15, applyConditions.Always, 0) },
                new[] { Damage(damageBonus.All, 0.15, applyConditions.Defensive_Assist, 10, target.squad) });

            Define(driveSet.Astral_Voice,
                new[] { Stat(advancedStats.ATK_Percent, 0.10, applyConditions.Always, 0) },
                new[] { Damage(damageBonus.All, 0.08, applyConditions.Quick_Assist, 15, target.squad) });

            Define(driveSet.Phaethons_Melody,
                new[] { Stat(advancedStats.Anomaly_Mastery, 0.08, applyConditions.Always, 0) },
                new[]
                {
                    Stat(advancedStats.Anomaly_Proficiency, 45, applyConditions.EX_SP_ATK, 8),
                    Noted("If another squad member uses an EX Special, Ether DMG +25%.")
                });

            Define(driveSet.Yunkui_Tales,
                new[] { Stat(advancedStats.HP_Percent, 0.10, applyConditions.Always, 0) },
                new[]
                {
                    Stat(advancedStats.Crit_Rate, 0.04, applyConditions.EX_SP_ATK, 15),
                    Stat(advancedStats.Crit_Rate, 0.04, applyConditions.Chain_Attack, 15),
                    Stat(advancedStats.Crit_Rate, 0.04, applyConditions.Ultimate, 15),
                    Noted("At 3 stacks, Sheer DMG +10%.")
                });

            Define(driveSet.King_of_the_Summit,
                new[] { Stat(advancedStats.Daze, 0.06, applyConditions.Always, 0) },
                new[]
                {
                    Stat(advancedStats.Crit_Damage, 0.15, applyConditions.EX_SP_ATK, 15, target.squad),
                    Stat(advancedStats.Crit_Damage, 0.15, applyConditions.Chain_Attack, 15, target.squad),
                    Stat(advancedStats.Crit_Damage, 0.15, applyConditions.Crit_Rate_At_Least_50, 15, target.squad)
                });

            Define(driveSet.Moonlight_Lullaby,
                new[] { Stat(advancedStats.EnergyRegen, 0.20, applyConditions.Always, 0) },
                new[]
                {
                    Damage(damageBonus.All, 0.18, applyConditions.EX_SP_ATK, 25, target.squad),
                    Damage(damageBonus.All, 0.18, applyConditions.Ultimate, 25, target.squad)
                });

            Define(driveSet.Dawns_Bloom,
                new[] { Damage(damageBonus.Basic, 0.15, applyConditions.Always, 0, target.self) },
                new[]
                {
                    Damage(damageBonus.Basic, 0.20, applyConditions.BasicATK, 0, target.self),
                    Damage(damageBonus.Basic, 0.20, applyConditions.EX_SP_ATK, 25, target.self)
                });

            Define(driveSet.The_Sky_Ablaze,
                new[] { Damage(damageBonus.Ether, 0.10, applyConditions.Always, 0, target.self) },
                new[]
                {
                    Stat(advancedStats.Crit_Damage, 0.30, applyConditions.Always, 0),
                    Stat(advancedStats.ATK_Percent, 0.10, applyConditions.EX_SP_ATK, 30),
                    Stat(advancedStats.ATK_Percent, 0.10, applyConditions.Ultimate, 30)
                });

            Define(driveSet.White_Water_Ballad,
                new[] { Damage(damageBonus.Physical, 0.10, applyConditions.Always, 0, target.self) },
                new[]
                {
                    Stat(advancedStats.Crit_Rate, 0.10, applyConditions.Ether_Veil, 15),
                    Stat(advancedStats.Crit_Rate, 0.10, applyConditions.Ether_Veil, 30),
                    Stat(advancedStats.ATK_Percent, 0.10, applyConditions.Ether_Veil, 30)
                });

            Define(driveSet.Thorned_Rose,
                new[] { Stat(advancedStats.DEF_Percent, 0.16, applyConditions.Always, 0) },
                new[]
                {
                    Damage(damageBonus.All, 0.15, applyConditions.Always, 0, target.self),
                    Noted("CRIT Rate +8% when DEF is at least 1,000, or +16% when DEF is at least 1,800.")
                });

            Define(driveSet.Bunny_in_Wonderland,
                new[] { Stat(advancedStats.HP_Percent, 0.10, applyConditions.Always, 0) },
                new[]
                {
                    Damage(damageBonus.All, 0.06, applyConditions.EX_SP_ATK, 25, target.squad),
                    Damage(damageBonus.All, 0.06, applyConditions.Defensive_Assist, 25, target.squad)
                });

            Define(driveSet.Shining_Aria,
                new[] { Damage(damageBonus.Ether, 0.10, applyConditions.Always, 0, target.self) },
                new[]
                {
                    Stat(advancedStats.Anomaly_Proficiency, 36, applyConditions.BasicATK, 8),
                    Damage(damageBonus.All, 0.25, applyConditions.Stunned, 18, target.self)
                });

            Define(driveSet.Wuthering_Salon,
                new[] { Damage(damageBonus.Wind, 0.10, applyConditions.Always, 0, target.self) },
                new[]
                {
                    Stat(advancedStats.Anomaly_Proficiency, 25, applyConditions.EX_SP_ATK, 40),
                    Damage(damageBonus.All, 0.18, applyConditions.Windswept, 40, target.self)
                });

            Define(driveSet.Notes_From_the_Chained,
                new[] { Damage(damageBonus.Ice, 0.10, applyConditions.Always, 0, target.self) },
                new[]
                {
                    Stat(advancedStats.Anomaly_Proficiency, 48, applyConditions.Abloom, 30),
                    Damage(damageBonus.Anomaly, 0.16, applyConditions.freeze, 30, target.self)
                });
        }
    }
}
