using GameServer.Interfaces;
using GameServer.Interfaces.Snapshots;
using System;

namespace GameServer.Damages
{
    public class Damage : IDamage
    {
        public double Physical { get; set; }
        public double Magical { get; set; }
        public double Composite { get; set; }
        public double Pure { get; set; }

        public bool IsCritical { get; set; } = false;

        public Damage() : this(0, 0, 0, 0) { }

        public Damage(IDamage damage)
        {
            Physical = damage.Physical;
            Magical = damage.Magical;
            Composite = damage.Composite;
            Pure = damage.Pure;

            IsCritical = damage.IsCritical;
        }

        public Damage(double physical = 0, double magical = 0, double composite = 0, double pure = 0)
        {
            Physical = physical;
            Magical = magical;
            Composite = composite;
            Pure = pure;
        }

        public double Sum 
        {
            get
            {
                return Physical + Magical + Composite + Pure;
            }
        }

        public bool IsZero => Sum == 0;

        public IDamage TryCrit(double criticalChance, double criticalMultiplier, Random random)
        {
            var damage = (Damage)this.MemberwiseClone();
            if (random.NextDouble() < criticalChance)
            {
                damage.Multiply(criticalMultiplier);
                IsCritical = true;
            }

            return damage;
        }

        public static IDamage Zero
        {
            get => new Damage();
        }

        public void Add(IDamage other)
        { 
            Physical += other.Physical;
            Magical += other.Magical;
            Composite += other.Composite;
            Pure += other.Pure;
        }

        public void Multiply(double amount)
        {
            Physical *= amount;
            Magical *= amount;
            Composite *= amount;
            Pure *= amount;
        }

        public override string ToString()
        {
            var physical = $"{Physical:F2}P";
            var magical = $"{Magical:F2}M";
            var composite = $"{Composite:F2}C";
            var pure = $"{Pure:F2}R";

            var crit = IsCritical ? " (CRIT)" : "";

            return $"{Sum:F2} {(Sum == 0 ? "" : $"({physical} {magical} {composite} {pure})")}{crit}";
        }

        public DamageSnapshot Snapshot()
        {
            var snapshot = new DamageSnapshot();

            snapshot.Physical = Physical;
            snapshot.Magical = Magical;
            snapshot.Composite = Composite;
            snapshot.Pure = Pure;

            snapshot.IsCritical = IsCritical;

            return snapshot;
        }
    }
}
