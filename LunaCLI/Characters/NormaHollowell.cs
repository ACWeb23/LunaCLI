namespace Zenless
{
    class NormaHollowell : Character
    {
        public NormaHollowell(int mindNum)
            : base(1571, "Norma Hollowell", 'S', Specialty.Stun, attackType.Strike, atkElement.Fire, mindNum)
        {
            this.baseHP = 7799;
            this.baseATK = 687;
            this.baseDEF = 582;
            this.baseImpact = 106;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 99;
            this.AnomalyProficiency = 98;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Norma_Hollowell_Portrait.webp");
        }
    }
}