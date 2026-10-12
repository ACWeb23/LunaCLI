namespace Zenless
{
    class HoshimiMiyabi : Character
    {
        public HoshimiMiyabi(int mindNum)
            : base(1091, "Hoshimi Miyabi", 'S', Specialty.Anomaly, attackType.Slash, atkElement.Ice, mindNum)
        {
            this.baseHP = 7673;
            this.baseATK = 805;
            this.baseDEF = 606;
            this.baseImpact = 86;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 116;
            this.AnomalyProficiency = 148;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Hoshimi_Miyabi_Portrait.webp");
        }
    }
}