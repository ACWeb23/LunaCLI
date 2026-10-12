namespace Zenless
{
    class OrphieMagnussonAndMagus : Character
    {
        public OrphieMagnussonAndMagus(int mindNum)
            : base(1301, "Orphie Magnusson & Magus", 'S', Specialty.Attack, attackType.Pierce, atkElement.Fire, mindNum)
        {
            this.baseHP = 7755;
            this.baseATK = 854;
            this.baseDEF = 612;
            this.baseImpact = 93;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 92;
            this.AnomalyProficiency = 90;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Orphie_Magnusson_%26_Magus_Portrait.webp");
        }
    }
}