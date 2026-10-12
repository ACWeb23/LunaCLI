namespace Zenless
{
    class Qingyi : Character
    {
        public Qingyi(int mindNum)
            : base(1251, "Qingyi", 'S', Specialty.Stun, attackType.Strike, atkElement.Electric, mindNum)
        {
            this.baseHP = 8250;
            this.baseATK = 683;
            this.baseDEF = 612;
            this.baseImpact = 118;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 94;
            this.AnomalyProficiency = 93;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Qingyi_Portrait.webp");
        }
    }
}