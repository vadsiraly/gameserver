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
        public List<Effect> Effects { get; set; }

        public void Use(Unit source, Unit target, Random random)
        {
            target.ApplyEffect(source, Effects, random);
        }
    }
}
