using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.BaseTypes
{
    public enum AbilityTarget
    {
        RandomFriendly,
        RandomEnemy,
        AllEnemy,
        AllFriendly
    }
    public class Ability
    {
        public string Name { get; set; }
        public AbilityTarget Target { get; set; }
        //public int TargetCount { get; set; } = 1;
        public List<Effect> Effects { get; set; }

        public void Use(Unit source, Team target, Random random)
        {
            switch (Target)
            {
                case AbilityTarget.RandomFriendly:
                    source.Team.GetRandomAliveUnit(random).ApplyEffect(source, Effects, random);
                    break;
                case AbilityTarget.RandomEnemy:
                    foreach (var effect in Effects)
                    {
                        target.GetRandomAliveUnit(random).ApplyEffect(source, Effects, random);
                    }
                    break;
                case AbilityTarget.AllEnemy:
                    foreach (var enemy in target.Units)
                    {
                        enemy.ApplyEffect(source, Effects, random);
                    }
                    break;
                case AbilityTarget.AllFriendly:
                    foreach (var friendly in source.Team.Units)
                    {
                        friendly.ApplyEffect(source, Effects, random);
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
