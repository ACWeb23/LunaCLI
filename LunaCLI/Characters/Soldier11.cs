namespace Zenless
{
    class Soldier11 : Character
    {
        public Soldier11(int mindNum)
            : base(1041, "Soldier 11", 'S', Specialty.Attack, attackType.Slash, atkElement.Fire, mindNum)
        {
            this.baseHP = 7673;
            this.baseATK = 813;
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
            this.ImagePath = Portrait("Soldier_11_Portrait.webp");
        }
    }
}