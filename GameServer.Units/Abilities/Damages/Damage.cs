using GameServer.Model.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.Damages
{
    public class Damage
    {
        public double Physical { get; set; }
        public double Magical { get; set; }
        public double Composite { get; set; }
        public double Pure { get; set; }

        public bool IsCritical { get; set; } = false;

        public Damage() : this(0, 0, 0, 0) { }

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

        public Damage TryCrit(double criticalChance, double criticalMultiplier, Random random)
        {
            var damage = (Damage)this.MemberwiseClone();
            if (random.NextDouble() < criticalChance)
            {
                damage *= criticalMultiplier;
                IsCritical = true;
            }

            return damage;
        }

        public void Add(double physical = 0, double magical = 0, double composite = 0, double pure = 0)
        {
            Physical += physical;
            Magical += magical;
            Composite += composite;
            Pure += pure;
        }

        public static Damage Zero
        {
            get => new Damage();
        }

        public void Add(Damage other)
        {
            Add(other.Physical, other.Magical, other.Composite, other.Pure);
        }

        public static Damage operator+(Damage a, Damage b)
        {
            var damage = (Damage)a.MemberwiseClone();
            damage.Physical += b.Physical;
            damage.Magical += b.Magical;
            damage.Composite += b.Composite;
            damage.Pure += b.Pure;

            return damage;
        }

        public static Damage operator *(Damage a, Damage b)
        {
            var damage = (Damage)a.MemberwiseClone();
            damage.Physical *= b.Physical;
            damage.Magical *= b.Magical;
            damage.Composite *= b.Composite;
            damage.Pure *= b.Pure;

            return damage;
        }

        public static Damage operator *(Damage a, double b)
        {
            var damage = (Damage)a.MemberwiseClone();
            damage.Physical *= b;
            damage.Magical *= b;
            damage.Composite *= b;
            damage.Pure *= b;

            return damage;
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
