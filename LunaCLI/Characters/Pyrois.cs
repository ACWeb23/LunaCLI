namespace Zenless
{
    class Pyrois : Character
    {
        public Pyrois(int mindNum)
            : base(1551, "Pyrois", 'S', Specialty.Attack, attackType.Slash, atkElement.Ether, mindNum)
        {
            this.baseHP = 7673;
            this.baseATK = 849;
            this.baseDEF = 612;
            this.baseImpact = 94;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 94;
            this.AnomalyProficiency = 93;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.0;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Pyrois_Portrait.webp");
        }
    }
}