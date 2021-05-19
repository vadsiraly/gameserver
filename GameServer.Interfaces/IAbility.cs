using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameServer.Interfaces.Events;

namespace GameServer.Interfaces
{
    public interface IAbility : IDamageSource
    {
        public event EventHandler<AbilityUseEventArgs> BeforeAbilityUseEvent;
        public event EventHandler<AbilityUseEventArgs> AfterAbilityUseEvent;

        public IUnit Owner { get; }
        public int Id { get; }
        public string Reference { get; }
        public string Name { get; }
        public string Description { get; }

        public bool IsActive { get; }

        public int ManaCost { get; }
        public int Cooldown { get; }
        public IDamage Damage { get; }
        public bool CanCriticalHit { get; }

        public List<IEffect> Buffs { get; }
        public List<IEffect> Debuffs { get; }
        public double EffectChance { get; }

        public bool Available { get; }

        public void Tick();

        public void Use(List<ITargetable> targets);
    }
}
