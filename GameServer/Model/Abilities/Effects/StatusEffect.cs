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
        private int duration;

        public StatusEffect(string name, int duration, Status status)
        {
            Name = name;
            Duration = duration;
            Status = status;
        }

        public override string Name { get; }
        public override int Duration { get => duration; protected set => duration = value; }

        public Status Status { get; }

        public override void ApplyEffect(IUnit target)
        {
            target.AddDebuff(this);
            target.AddStatus(Status);
        }

        public override void RemoveEffect(IUnit target)
        {
            target.RemoveStatus(Status);
        }
    }
}
