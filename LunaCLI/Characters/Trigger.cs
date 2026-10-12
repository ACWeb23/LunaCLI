namespace Zenless
{
    class Trigger : Character
    {
        public Trigger(int mindNum)
            : base(1361, "Trigger", 'S', Specialty.Stun, attackType.Pierce, atkElement.Electric, mindNum)
        {
            this.baseHP = 7923;
            this.baseATK = 675;
            this.baseDEF = 600;
            this.baseImpact = 113;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 96;
            this.AnomalyProficiency = 95;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Trigger_Portrait.webp");
        }
    }
}