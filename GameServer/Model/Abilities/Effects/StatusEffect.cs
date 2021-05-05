using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities.Effects
{
    public class StatusEffect : Effect
    {
        public StatusEffect(Unit owner, string name, int duration, Status status, double chance, Random random) : base(owner, random)
        {
            Name = name;
            Duration = duration;
            Status = status;
            Chance = chance;
        }

        public Status Status { get; }
        public int Duration { get; private set; }

        public override void ApplyEffect(Unit target)
        {
            if (_random.NextDouble() < Chance)
            {
                target.AddDebuff(this);
                target.AddStatus(Status);
            }
        }

        public override void RemoveEffect(Unit target)
        {
            target.RemoveStatus(Status);
        }

        public override void Tick(Unit target)
        {
            if (--Duration <= 0)
            {
                RemoveEffect(target);
            }
        }
    }
}
