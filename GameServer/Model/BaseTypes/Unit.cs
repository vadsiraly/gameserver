using GameServer.Enumerations.Damage;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.BaseTypes
{
    public class Unit
    {
        [JsonIgnore]
        public Team Team { get; set; }

        public int Id { get; set; }
        public string Name { get; set; }
        public double Health { get; set; }
        public double Mana { get; set; }
        public double Damage { get; set; }
        public double Armor { get; set; }
        public double Resistance { get; set; }
        public double Speed { get; set; }
        public double CriticalChance { get; set; }
        public double CriticalMultiplier { get; set; }
        public bool IsDead() => Health <= 0;
        public Ability[] Abilities { get; set; } = new Ability[4];

        public List<(Unit Source, Effect Effect, int Duration)> BeforeRoundEffects { get; set; } = new List<(Unit Source, Effect Effect, int Duration)>();
        public List<(Unit Source, Effect Effect, int Duration)> AfterRoundEffects { get; set; } = new List<(Unit Source, Effect Effect, int Duration)>();
        public List<(Unit Source, Effect Effect, int Delay, int Duration)> DelayedEffects { get; set; } = new List<(Unit Source, Effect Effect, int Delay, int Duration)>();

        public void ProcessBeforeRoundEffects(Random random)
        {
            for(int i = 0; i< BeforeRoundEffects.Count; i++)
            {
                BeforeRoundEffects[i].Effect.Apply(BeforeRoundEffects[i].Source, this, random);
                BeforeRoundEffects[i] = (BeforeRoundEffects[i].Source, BeforeRoundEffects[i].Effect, BeforeRoundEffects[i].Duration - 1);
            }

            BeforeRoundEffects.RemoveAll(x => x.Duration == 0);
        }

        public void ProcessAfterRoundEffects(Random random)
        {
            for (int i = 0; i < BeforeRoundEffects.Count; i++)
            {
                AfterRoundEffects[i].Effect.Apply(AfterRoundEffects[i].Source, this, random);
                AfterRoundEffects[i] = (AfterRoundEffects[i].Source, AfterRoundEffects[i].Effect, AfterRoundEffects[i].Duration - 1);
            }

            AfterRoundEffects.RemoveAll(x => x.Duration == 0);
        }

        public void ApplyEffect(Unit source, List<Effect> effects, Random random)
        {
            foreach (var effect in effects)
            {
                switch (effect.Schedule)
                {
                    case EffectSchedule.Instant:
                        effect.Apply(source, this, random);
                        break;
                    case EffectSchedule.BeforeRound:
                        BeforeRoundEffects.Add((source, effect, effect.Duration));
                        break;
                    case EffectSchedule.AfterRound:
                        AfterRoundEffects.Add((source, effect, effect.Duration));
                        break;
                    case EffectSchedule.Delayed:
                        DelayedEffects.Add((source, effect, effect.Delay, effect.Duration));
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
