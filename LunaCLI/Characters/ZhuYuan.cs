namespace Zenless
{
    class ZhuYuan : Character
    {
        public ZhuYuan(int mindNum)
            : base(1241, "Zhu Yuan", 'S', Specialty.Attack, attackType.Pierce, atkElement.Ether, mindNum)
        {
            this.baseHP = 7482;
            this.baseATK = 844;
            this.baseDEF = 600;
            this.baseImpact = 90;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 93;
            this.AnomalyProficiency = 92;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Zhu_Yuan_Portrait.webp");
        }
    }
}