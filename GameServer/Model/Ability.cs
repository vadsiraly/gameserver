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
        public List<EffectGroup> EffectGroups { get; set; }

        public void Use(Unit source, Team target, Random random)
        {
            foreach (var effectGroup in EffectGroups)
            {
                switch (effectGroup.Target)
                {
                    case EffectGroupTarget.RandomFriendly:
                        source.Team.GetRandomAliveUnit(random).ApplyEffect(source, effectGroup.Effects, random);
                        break;
                    case EffectGroupTarget.RandomEnemy:
                        target.GetRandomAliveUnit(random).ApplyEffect(source, effectGroup.Effects, random);
                        break;
                    case EffectGroupTarget.AllEnemy:
                        foreach (var enemy in target.Units)
                        {
                            enemy.ApplyEffect(source, effectGroup.Effects, random);
                        }
                        break;
                    case EffectGroupTarget.AllFriendly:
                        foreach (var friendly in source.Team.Units)
                        {
                            friendly.ApplyEffect(source, effectGroup.Effects, random);
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
