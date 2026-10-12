namespace Zenless
{
    class JuFufu : Character
    {
        public JuFufu(int mindNum)
            : base(1391, "Ju Fufu", 'S', Specialty.Stun, attackType.Strike, atkElement.Fire, mindNum)
        {
            this.baseHP = 8250;
            this.baseATK = 690;
            this.baseDEF = 597;
            this.baseImpact = 118;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 93;
            this.AnomalyProficiency = 96;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Ju_Fufu_Portrait.webp");
        }
    }
}