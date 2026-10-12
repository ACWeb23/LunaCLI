namespace Zenless
{
    class Zhao : Character
    {
        public Zhao(int mindNum)
            : base(1341, "Zhao", 'S', Specialty.Defense, attackType.Slash, atkElement.Ice, mindNum)
        {
            this.baseHP = 9117;
            this.baseATK = 690;
            this.baseDEF = 701;
            this.baseImpact = 93;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 93;
            this.AnomalyProficiency = 96;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Zhao_Portrait.webp");
        }
    }
}