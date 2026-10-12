namespace Zenless
{
    class VivianBanshee : Character
    {
        public VivianBanshee(int mindNum)
            : base(1331, "Vivian Banshee", 'S', Specialty.Anomaly, attackType.Slash, atkElement.Ether, mindNum)
        {
            this.baseHP = 7673;
            this.baseATK = 805;
            this.baseDEF = 606;
            this.baseImpact = 86;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 108;
            this.AnomalyProficiency = 118;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Vivian_Banshee_Portrait.webp");
        }
    }
}