using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.BaseTypes
{
    public class Ability
    {
        public string Name { get; set; }
        public Damage Damage { get; set; }

        public void Use(Unit source, Unit target, Random random)
        {
            var isCritical = random.NextDouble() < source.CriticalChance;
            var damage = isCritical ? (Damage as Damage) * source.CriticalMultiplier : Damage;
            
            target.ApplyDamage(damage);
            //Console.WriteLine($"{source.Name} hits {target.Name} with {Name} for {damage.Amount} damage{(isCritical ? " (critical)" : "")}");
        }
    }
}
