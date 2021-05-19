using GameServer.Interfaces;
using GameServer.Interfaces.Snapshots;
using GameServer.Model.Abilities.ConcreteAbilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Damages
{
    public class ModifiedDamage : IModifiedDamage
    {
        public ModifiedDamage((IDamageSource Source, IDamage Damage) baseDamage)
        {
            BaseDamage = baseDamage;
        }

        public (IDamageSource Source, IDamage Damage) BaseDamage { get; private set; }
        public List<(IDamageSource Source, IDamage Damage)> Modifications { get; private set; } = new List<(IDamageSource Source, IDamage Damage)>();

        public void AddReduction(double armor, double resistance)
        {
            // Physical + Composite
            var armorReduction = Damage.Zero;
            double multiplier;
            if (armor > 0)
            {
                multiplier = 100 / (100 + armor);
            }
            else
            {
                multiplier = 2 - 100 / (100 - armor);
            }

            armorReduction.Physical = BaseDamage.Damage.Physical * multiplier - BaseDamage.Damage.Physical;
            armorReduction.Composite = (BaseDamage.Damage.Composite / 2) * multiplier - (BaseDamage.Damage.Composite / 2);

            if (!armorReduction.IsZero)
            {
                Modifications.Add((new ArmorReduction(), armorReduction));
            }

            // Magical + Composite
            var resistanceReduction = Damage.Zero;
            if (resistance > 0)
            {
                multiplier = 100 / (100 + resistance);
            }
            else
            {
                multiplier = 2 - 100 / (100 - resistance);
            }

            resistanceReduction.Magical = BaseDamage.Damage.Magical * multiplier - BaseDamage.Damage.Magical;
            resistanceReduction.Composite = (BaseDamage.Damage.Composite / 2) * multiplier - (BaseDamage.Damage.Composite / 2);

            if (!resistanceReduction.IsZero)
            {
                Modifications.Add((new ResistanceReduction(), resistanceReduction));
            }
        }

        public IDamage Aggregate()
        {
            var baseDamage = new Damage(BaseDamage.Damage);
            foreach(var damageSource in Modifications)
            {
                baseDamage.Add(damageSource.Damage);
            }

            return baseDamage;
        }

        public static IModifiedDamage Zero => new ModifiedDamage((new UnknownSource(), Damage.Zero));

        public ModifiedDamageSnapshot Snapshot()
        {
            var snapshot = new ModifiedDamageSnapshot();

            snapshot.BaseDamage = (BaseDamage.Source.Reference, BaseDamage.Damage.Snapshot());
            snapshot.Modifications = Modifications.Select(x => (x.Source.Reference, x.Damage.Snapshot())).ToList();

            return snapshot;
        }
    }
}
