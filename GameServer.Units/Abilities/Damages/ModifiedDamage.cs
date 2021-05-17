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

            armorReduction.Physical = BaseDamage.Damage.Physical * multiplier - BaseDamage.Damage.Physical;
            armorReduction.Composite = (BaseDamage.Damage.Composite / 2) * multiplier - (BaseDamage.Damage.Composite / 2);

            if (!armorReduction.IsZero)
            {
                Modifications.Add((new ArmorReduction(null, null), armorReduction));
            }

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

            resistanceReduction.Magical = BaseDamage.Damage.Magical * multiplier - BaseDamage.Damage.Magical;
            resistanceReduction.Composite = (BaseDamage.Damage.Composite / 2) * multiplier - (BaseDamage.Damage.Composite / 2);

            if (!resistanceReduction.IsZero)
            {
                Modifications.Add((new ResistanceReduction(null, null), resistanceReduction));
            }
        }

        public Damage Aggregate()
        {
            var baseDamage = BaseDamage.Damage;
            foreach(var damageSource in Modifications)
            {
                baseDamage += damageSource.Damage;
            }

            return baseDamage;
        }

        public static ModifiedDamage Zero => new ModifiedDamage((new UnknownAbility(null, null), Damage.Zero));

        public ModifiedDamageSnapshot Snapshot()
        {
            var snapshot = new ModifiedDamageSnapshot();

            snapshot.BaseDamage = (BaseDamage.Source.Reference, BaseDamage.Damage.Snapshot());
            snapshot.Modifications = Modifications.Select(x => (x.Source.Reference, x.Damage.Snapshot())).ToList();

            return snapshot;
        }
    }
}
