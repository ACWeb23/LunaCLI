namespace Zenless
{
    class VelinaAirgid : Character
    {
        public VelinaAirgid(int mindNum)
            : base(1561, "Velina Airgid", 'S', Specialty.Anomaly, attackType.Slash, atkElement.Wind, mindNum)
        {
            this.baseHP = 7788;
            this.baseATK = 797;
            this.baseDEF = 612;
            this.baseImpact = 86;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 112;
            this.AnomalyProficiency = 111;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Velina_Airgid_Portrait.webp");
        }
    }
}