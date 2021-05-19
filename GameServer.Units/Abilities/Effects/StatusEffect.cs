using GameServer.Interfaces;
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
        public StatusEffect(Ability source, string name, int duration, Status status, double chance, Random random) : base(source, random)
        {
            Name = name;
            Duration = duration;
            Status = status;
            Chance = chance;
        }

        public Status Status { get; }
        public double Chance { get; private set; }

        public override void ApplyEffect(ITargetable target)
        {
            if (_random.NextDouble() < Chance)
            {
                target.AddDebuff(this);
                target.AddStatus(Source, Status);
            }
        }

        public override void RemoveEffect(ITargetable target)
        {
            target.RemoveStatus(Source, Status);
        }

        public override void Tick(ITargetable target, int stack)
        {
            if (--Duration <= 0)
            {
                RemoveEffect(target);
            }
        }
    }
}
