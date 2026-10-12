namespace Zenless
{
    class AsabaHarumasa : Character
    {
        public AsabaHarumasa(int mindNum)
            : base(1201, "Asaba Harumasa", 'S', Specialty.Attack, attackType.Pierce, atkElement.Electric, mindNum)
        {
            this.baseHP = 7405;
            this.baseATK = 840;
            this.baseDEF = 600;
            this.baseImpact = 90;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 80;
            this.AnomalyProficiency = 95;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Asaba_Harumasa_Portrait.webp");
        }
    }
}