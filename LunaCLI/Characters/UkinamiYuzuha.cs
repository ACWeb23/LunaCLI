namespace Zenless
{
    class UkinamiYuzuha : Character
    {
        public UkinamiYuzuha(int mindNum)
            : base(1411, "Ukinami Yuzuha", 'S', Specialty.Support, attackType.Strike, atkElement.Physical, mindNum)
        {
            this.baseHP = 8829;
            this.baseATK = 683;
            this.baseDEF = 612;
            this.baseImpact = 86;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 88;
            this.AnomalyProficiency = 93;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
        }
    }
}