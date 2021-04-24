using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.BaseTypes
{
    public class Team
    {
        public List<Unit> Units { get; set; } = new List<Unit>();

        public void ApplyEffect(Unit source, List<Effect> Effects, Random random)
        {
            foreach (var effect in Effects)
            {
                switch (effect.Target)
                {
                    case EffectTarget.RandomFriendly:
                        effect.Apply(source, source.Team.GetRandomAliveUnit(random), random);
                        break;
                    case EffectTarget.RandomEnemy:
                        effect.Apply(source, GetRandomAliveUnit(random), random);
                        break;
                    case EffectTarget.AllEnemy:
                        foreach(var enemy in Units)
                        {
                            effect.Apply(source, enemy, random);
                        }
                        break;
                    case EffectTarget.AllFriendly:
                        foreach (var friendly in source.Team.Units)
                        {
                            effect.Apply(source, friendly, random);
                        }
                        break;
                    default:
                        break;
                }
            }
        }

        public Unit GetRandomAliveUnit(Random random)
        {
            return Units.GetRandomElement(random, x => !x.IsDead());
        }

        public void Add(Unit u)
        {
            if (Units.Count < 6)
            {
                Units.Add(u);
                u.Team = this;
            }
        }
    }
}
