namespace Zenless
{
    class Seed : Character
    {
        public Seed(int mindNum)
            : base(1461, "Seed", 'S', Specialty.Attack, attackType.Slash, atkElement.Electric, mindNum)
        {
            this.baseHP = 7673;
            this.baseATK = 854;
            this.baseDEF = 612;
            this.baseImpact = 93;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 94;
            this.AnomalyProficiency = 93;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Seed_Portrait.webp");
        }
    }
}