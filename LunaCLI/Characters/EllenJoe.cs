namespace Zenless
{
    class EllenJoe : Character
    {
        public EllenJoe(int mindNum)
            : base(1191, "Ellen Joe", 'S', Specialty.Attack, attackType.Slash, atkElement.Ice, mindNum)
        {
            this.baseHP = 7673;
            this.baseATK = 863;
            this.baseDEF = 606;
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
            this.ImagePath = Portrait("Ellen_Joe_Portrait.webp");
        }
    }
}