using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Abilities
{
    public class AbilityHealing
    {
        public AbilityHealing(double healing, double crit, double bonus, double bonusNoCrit)
        {
            HealingPart = healing;
            CriticalPart = crit;
            BonusHealingPart = bonus;
            BonusHealingNoCritPart = bonusNoCrit;
        }

        public double HealingPart { get; set; }
        public double CriticalPart { get; set; }
        public double BonusHealingPart { get; set; }
        public double BonusHealingNoCritPart { get; set; }

        public double Healing
        {
            get
            {
                return HealingPart + CriticalPart + BonusHealingPart + BonusHealingNoCritPart;
            }
        }
    }
}
