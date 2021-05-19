using GameServer.Interfaces;
using GameServer.Model.Units;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Battles
{
    public enum TargetType
    {
        Enemy,
        Friendly,
        Any
    }

    public class TargetManager
    {
        private Random _random;
        private ITeam _attackers;
        private ITeam _defenders;

        public TargetManager(ITeam attackers, ITeam defenders, Random random)
        {
            _random = random;
            _attackers = attackers;
            _defenders = defenders;
        }

        List<IUnit> GetTargets(IUnit source, int count, TargetType type)
        {
            IEnumerable<IUnit> targets;
            if (_attackers.IsMember(source))
            {
                if (type == TargetType.Enemy)
                {
                    targets = _defenders.Units.OrderBy(x => _random.Next()).Take(count);
                }
                else if (type == TargetType.Friendly)
                {
                    targets = _attackers.Units.OrderBy(x => _random.Next()).Take(count);
                }
                else
                {
                    targets = _defenders.Units.Union(_attackers.Units).OrderBy(x => _random.Next()).Take(count);
                }
            }
            else
            {
                if (type == TargetType.Enemy)
                {
                    targets = _attackers.Units.OrderBy(x => _random.Next()).Take(count);
                }
                else if (type == TargetType.Friendly)
                {
                    targets = _defenders.Units.OrderBy(x => _random.Next()).Take(count);
                }
                else
                {
                    targets = _defenders.Units.Union(_attackers.Units).OrderBy(x => _random.Next()).Take(count);
                }
            }

            return targets.ToList();
        }
    }
}
