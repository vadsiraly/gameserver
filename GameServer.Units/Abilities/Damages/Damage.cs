﻿using GameServer.Model.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.Damages
{
    public class Damage
    {
        public double Physical { get; private set; }
        public double Magical { get; private set; }
        public double Composite { get; private set; }
        public double Pure { get; private set; }

        public bool IsCritical { get; private set; } = false;

        public DamageReduction Reduction { get; private set; } = DamageReduction.None;

        public Damage() : this(0, 0, 0, 0) { }

        public Damage(double physical = 0, double magical = 0, double composite = 0, double pure = 0, DamageReduction reduction = null)
        {
            Physical = physical;
            Magical = magical;
            Composite = composite;
            Pure = pure;
            Reduction = reduction ?? DamageReduction.None;
        }

        public double Sum 
        {
            get
            {
                var d = Mitigate();
                return d.Physical + d.Magical + d.Composite + d.Pure;
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

        public void Reduce(DamageReduction reduction)
        {
            Reduction = reduction;
        }

        private Damage Mitigate()
        {
            // Physical
            var damage = (Damage)this.MemberwiseClone();
            if (Reduction.Armor > 0)
            {
                var multiplier = 100 / (100 + Reduction.Armor);
                damage.Physical *= multiplier;
            }
            else
            {
                var multiplier = 2 - 100 / (100 - Reduction.Armor);
                damage.Physical *= multiplier;
            }
 
            // Magical
            if (Reduction.Resistance > 0)
            {
                var multiplier = 100 / (100 + Reduction.Resistance);
                damage.Magical *= multiplier;
            }
            else
            {
                var multiplier = 2 - 100 / (100 - Reduction.Resistance);
                damage.Magical *= multiplier;
            }

            // Composite
            if (Reduction.Armor > 0)
            {
                var multiplier = 100 / (100 + Reduction.Armor);
                damage.Composite = damage.Composite / 2 * multiplier;
            }
            else
            {
                var multiplier = 2 - 100 / (100 - Reduction.Armor);
                damage.Composite = damage.Composite / 2 * multiplier;
            }

            if (Reduction.Resistance > 0)
            {
                var multiplier = 100 / (100 + Reduction.Resistance);
                damage.Composite = damage.Composite / 2 * multiplier;
            }
            else
            {
                var multiplier = 2 - 100 / (100 - Reduction.Resistance);
                damage.Composite = damage.Composite / 2 * multiplier;
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
            a.Physical *= b.Physical;
            a.Magical *= b.Magical;
            a.Composite *= b.Composite;
            a.Pure *= b.Pure;

            return damage;
        }

        public static Damage operator *(Damage a, double b)
        {
            var damage = (Damage)a.MemberwiseClone();
            a.Physical *= b;
            a.Magical *= b;
            a.Composite *= b;
            a.Pure *= b;

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

            var mitigated = Mitigate();
            snapshot.PhysicalReduced = mitigated.Physical;
            snapshot.MagicalReduced = mitigated.Magical;
            snapshot.CompositeReduced = mitigated.Composite;
            snapshot.PureReduced = mitigated.Pure;

            return snapshot;
        }
    }
}
