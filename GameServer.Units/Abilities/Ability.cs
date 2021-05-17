using GameServer.Model.Abilities.Damages;
using GameServer.Model.Abilities.Effects;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities
{
    public abstract class Ability
    {
        protected Random _random;
        protected int _activeCooldown = 0;

        public event EventHandler<AbilityUseEventArgs> BeforeAbilityUseEvent;
        public event EventHandler<AbilityUseEventArgs> AfterAbilityUseEvent;

        public Ability(Unit owner, Random random)
        {
            Owner = owner;
            _random = random;
        }

        public Unit Owner { get; protected set; }
        public int Id { get; protected set; } = -1;
        public string Reference { get; protected set; } = string.Empty;
        public string Name { get; protected set; } = string.Empty;
        public string Description { get; protected set; } = string.Empty;

        public bool IsActive { get; protected set; } = true;

        public int ManaCost { get; protected set; } = 0;
        public int Cooldown { get; protected set; } = 0;
        public Damage Damage { get; protected set; } = Damage.Zero;
        public bool CanCriticalHit { get; protected set; } = true;

        public List<Effect> Buffs { get; protected set; } = new List<Effect>();
        public List<Effect> Debuffs { get; protected set; } = new List<Effect>();
        public double EffectChance { get; protected set; } = 1;

        public virtual bool Available => _activeCooldown == 0;

        public virtual void Tick()
        {
            if (_activeCooldown > 0)
            {
                --_activeCooldown;
            }
        }

        public virtual void Use(List<Unit> targets)
        {
            if (!IsActive) return;

            ModifiedDamage modifiedDamage;
            if (CanCriticalHit)
            {
                modifiedDamage = new ModifiedDamage((this, Damage.TryCrit(Owner.CriticalHitChance, Owner.CriticalHitMultiplier, _random)));
            }
            else
            {
                modifiedDamage = new ModifiedDamage((this, Damage));
            }

            BeforeAbilityUse(new AbilityUseEventArgs(Owner, targets, modifiedDamage));

            foreach (var target in targets)
            {
                target.TakeDamage(this, modifiedDamage);

                foreach (var buff in Buffs)
                {
                    buff.ApplyEffect(target);
                }

                foreach (var debuff in Debuffs)
                {
                    debuff.ApplyEffect(target);
                }
            }

            _activeCooldown = Cooldown;

            AfterAbilityUse(new AbilityUseEventArgs(Owner, targets, modifiedDamage));
        }

        public virtual void BeforeAbilityUse(AbilityUseEventArgs e)
        {
            BeforeAbilityUseEvent?.Invoke(this, e);
        }

        public virtual void AfterAbilityUse(AbilityUseEventArgs e)
        {
            AfterAbilityUseEvent?.Invoke(this, e);
        }
    }
}
