using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Interfaces
{
    public interface ITargetable
    {
        public string Name { get; }
        public ITeam Team { get; }

        public void AddBuff(IEffect effect);

        public void RemoveBuff(IEffect effect);

        public void AddDebuff(IEffect effect);

        public void RemoveDebuff(IEffect effect);

        public void AddStatus(IAbility source, Status status);

        public void RemoveStatus(IAbility source, Status status);

        public IModifiedDamage TakeDamage(IAbility source, IModifiedDamage modifiedDamage);

        public IModifiedDamage TakeEffectDamage(IAbility source, IModifiedDamage modifiedDamage);

        public IModifiedDamage Heal(IAbility source, IModifiedDamage modifiedDamage);
    }
}
