using GameServer.Model.Abilities.ConcreteAbilities;
using GameServer.Model.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.Damages
{
    public class ModifiedDamage
    {
        public ModifiedDamage((Ability Source, Damage Damage) baseDamage)
        {
            BaseDamage = baseDamage;
            if (!BaseDamage.Damage.IsZero)
            {
                Modifications.Add(BaseDamage);
            }
        }

        public (Ability Source, Damage Damage) BaseDamage { get; private set; }
        public List<(Ability Source, Damage Damage)> Modifications { get; private set; } = new List<(Ability Source, Damage Damage)>();
        public DamageReduction Reduction { get; private set; } = DamageReduction.None;

        public void AddReduction(DamageReduction reduction)
        {
            // Physical + Composite
            var armorReduction = Damage.Zero;
            double multiplier;
            if (reduction.Armor > 0)
            {
                multiplier = 100 / (100 + reduction.Armor);
            }
            else
            {
                multiplier = 2 - 100 / (100 - reduction.Armor);
            }

            armorReduction.Physical = BaseDamage.Damage.Physical - BaseDamage.Damage.Physical * multiplier;
            armorReduction.Composite = (BaseDamage.Damage.Composite / 2) - (BaseDamage.Damage.Composite / 2) * multiplier;

            Modifications.Add((new ArmorReduction(null, null), armorReduction));

            // Magical + Composite
            var resistanceReduction = Damage.Zero;
            if (reduction.Resistance > 0)
            {
                multiplier = 100 / (100 + reduction.Resistance);
            }
            else
            {
                multiplier = 2 - 100 / (100 - reduction.Resistance);
            }

            resistanceReduction.Magical = BaseDamage.Damage.Magical - BaseDamage.Damage.Magical * multiplier;
            armorReduction.Composite = (BaseDamage.Damage.Composite / 2) - (BaseDamage.Damage.Composite / 2) * multiplier;

            Modifications.Add((new ResistanceReduction(null, null), armorReduction));
        }

        public Damage Aggregate()
        {
            var aggregateDamage = new Damage(reduction: Reduction);
            foreach(var damageSource in Modifications)
            {
                aggregateDamage += damageSource.Damage;
            }

            return aggregateDamage;
        }

        public Damage AggregateRaw()
        {
            var aggregateDamage = new Damage();
            foreach (var damageSource in Modifications)
            {
                aggregateDamage += damageSource.Damage;
            }

            return aggregateDamage;
        }

        public static ModifiedDamage Zero => new ModifiedDamage((null, Damage.Zero));

        public ModifiedDamageSnapshot Snapshot()
        {
            var snapshot = new ModifiedDamageSnapshot();

            snapshot.BaseDamage = (BaseDamage.Source.Reference, BaseDamage.Damage.Snapshot());
            snapshot.Modifications = Modifications.Select(x => (x.Source.Reference, x.Damage.Snapshot())).ToList();

            return snapshot;
        }
    }
}
