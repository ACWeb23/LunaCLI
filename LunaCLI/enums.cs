namespace Zenless
{
    public enum atkElement { nill, Physical, Fire, Ice, Electric, Wind, Ether, Lumiflux }
    public enum damageBonus { nill, Physical, Fire, Ice, Electric, Wind, Ether, Lumiflux, All, Anomaly }
    public enum Specialty { nill, Attack, Stun, Anomaly, Support, Defense, Rupture, Armorer }
    public enum attackType { Slash, Strike, Pierce }
    public enum target { self, squad, enemy}

    /// <summary>
    /// The specialResource enum holds the name of special resources, it may need to be reworked if an overide is 
    /// written for each Agents with special resources.
    /// </summary>
    public enum specialResource { nill, voidflare, whiteThunder,}

    /// <summary>
    ///  applyConditions enum holds a list of all conditions that could trigger an event or buff.
    /// </summary>
    public enum applyConditions { Always, on_Field, off_Field, EX_SP_ATK, Refringe, EnterBattle, Aftershock, dashATK, BasicATK, max_Stacks, adv_Stat_Threshold, shatter, freeze, silver_Star, white_Thunder, }

    /// <summary>
    /// The advanced stats enum holds the core stats that all characters have.
    /// </summary>
    public enum advancedStats
    {
        ATK_Percent,
        Anomaly_Proficiency,
        EnergyRegen,
        Impact,
        HP_Percent,
        DEF_Percent,
        PEN_Ratio,
        Crit_Rate,
        Crit_Damage,
        Anomaly_Mastery,
        Damage_bonus,
        ATK,
        DEF,
        HP,
        PEN,
        nill
    }
    /// <summary>
    /// the driveSet enum holds the names of specific drive sets.
    /// </summary>
    public enum driveSet
    {
        Feathered_Fate,
        Freedom_Blues,
        Shadow_Harmony,
        Branch_And_Blade_Song
    } // Add more when finished with basic definitions.

    /// <summary>
    /// The struct buff holds a single buff and what condtion triggers it.
    /// </summary>

    public struct Buff
    {
        public specialResource UniqueResource { get; set; }
        public double resourceAmmount { get; set; }
        // Advanced Stat
        public advancedStats statUp { get; set; }
        public double statValue { get; set; }
        public damageBonus damage_buff { get; set; }
        public double bonus_ammount { get; set; }

        public applyConditions buff_Condition { get; set; }
        public double buff_Duration { get; set; }
        public target buff_Target { get; set; }

        public Buff()
        {
            UniqueResource = specialResource.nill;
            resourceAmmount = 0;
            statUp = advancedStats.nill;
            statValue = 0;
            damage_buff = damageBonus.All;
            bonus_ammount = 0;
            buff_Condition = applyConditions.Always;
            buff_Duration = 0;
            buff_Target = target.self;
        }

        public Buff(specialResource uniqueResource, double resourceAmmount,
                    advancedStats AdStat, double bonus_ADV,
                    damageBonus damage_buff, double bonus_ammount,
                    applyConditions buff_Condition, double buff_Duration,
                    target bTarget)
        {
            this.UniqueResource = uniqueResource;
            this.resourceAmmount = resourceAmmount;
            this.statUp = AdStat;
            this.statValue = bonus_ADV;
            this.damage_buff = damage_buff;
            this.bonus_ammount = bonus_ammount;
            this.buff_Condition = buff_Condition;
            this.buff_Duration = buff_Duration;
            this.buff_Target = bTarget;
        }
    }
}
