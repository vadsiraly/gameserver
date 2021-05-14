using GameServer.Model.Snapshots;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.Damages
{
    public class CombinedDamage
    {
        public CombinedDamage((Ability Source, Damage Damage) baseDamage)
        {
            BaseDamage = baseDamage;
            if (!BaseDamage.Damage.IsZero)
            {
                DamageCollection.Add(BaseDamage);
            }
        }

        public (Ability Source, Damage Damage) BaseDamage { get; private set; }
        public List<(Ability Source, Damage Damage)> DamageCollection { get; private set; } = new List<(Ability Source, Damage Damage)>();
        public DamageReduction Reduction { get; private set; } = DamageReduction.None;

        public void Reduce(DamageReduction reduction)
        {
            Reduction = reduction;
            foreach(var damage in DamageCollection.Select(x => x.Damage))
            {
                damage.Reduce(Reduction);
            }
        }

        public Damage Aggregate()
        {
            var aggregateDamage = new Damage(reduction: Reduction);
            foreach(var damageSource in DamageCollection)
            {
                aggregateDamage += damageSource.Damage;
            }

            return aggregateDamage;
        }

        public Damage AggregateRaw()
        {
            var aggregateDamage = new Damage();
            foreach (var damageSource in DamageCollection)
            {
                aggregateDamage += damageSource.Damage;
            }

            return aggregateDamage;
        }

        public static CombinedDamage Zero => new CombinedDamage((null, Damage.Zero));

        public CombinedDamageSnapshot Snapshot()
        {
            var snapshot = new CombinedDamageSnapshot();

            snapshot.BaseDamage = (BaseDamage.Source.Reference, BaseDamage.Damage.Snapshot());
            snapshot.DamageCollection = DamageCollection.Select(x => (x.Source.Reference, x.Damage.Snapshot())).ToList();

            return snapshot;
        }
    }
}
