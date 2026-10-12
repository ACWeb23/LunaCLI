namespace Zenless
{
    class YidhariMurphy : Character
    {
        public YidhariMurphy(int mindNum)
            : base(1051, "Yidhari Murphy", 'S', Specialty.Rupture, attackType.Strike, atkElement.Ice, mindNum)
        {
            this.baseHP = 8497;
            this.baseATK = 787;
            this.baseDEF = 448;
            this.baseImpact = 95;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 87;
            this.AnomalyProficiency = 90;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 0.0;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Yidhari_Murphy_Portrait.webp");
        }
    }
}