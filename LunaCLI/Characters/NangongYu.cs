namespace Zenless
{
    class NangongYu : Character
    {
        public NangongYu(int mindNum)
            : base(1511, "Nangong Yu", 'S', Specialty.Stun, attackType.Strike, atkElement.Ether, mindNum)
        {
            this.baseHP = 8373;
            this.baseATK = 671;
            this.baseDEF = 622;
            this.baseImpact = 120;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 90;
            this.AnomalyProficiency = 93;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Nangong_Yu_Portrait.webp");
        }
    }
}