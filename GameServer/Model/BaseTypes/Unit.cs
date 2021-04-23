using GameServer.Enumerations.Damage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.BaseTypes
{
    public class Unit
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Health { get; set; }
        public double Mana { get; set; }
        public double Armor { get; set; }
        public double Resistance { get; set; }
        public double Speed { get; set; }
        public double CriticalChance { get; set; }
        public double CriticalMultiplier { get; set; }
        public bool IsDead() => Health <= 0;
        public Ability[] Abilities { get; set; } = new Ability[4];

        public void ApplyDamage(Damage damage)
        {
            var reducedDamage = double.NaN;
            if (damage.Type == DamageType.Physical)
            {
                reducedDamage = damage.Amount * (1 - Armor / 100);
            }

            if (damage.Type == DamageType.Magical)
            {
                reducedDamage = damage.Amount * (1 - Resistance / 100);
            }

            Health -= reducedDamage;
        }
    }
}
