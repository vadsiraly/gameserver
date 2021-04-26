using GameServer.Model.BaseTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    public class AbilityFactory
    {
        private readonly List<Ability> _abilities;

        public AbilityFactory()
        {
            _abilities = JsonConvert.DeserializeObject<List<Ability>>(Resources.Abilities);
        }

        public Ability GetByName(string name)
        {
            return CloningService.Clone(_abilities.Single(x => x.Name == name));
        }

        public Ability GetByReference(string reference)
        {
            return CloningService.Clone(_abilities.Single(x => x.Reference == reference));
        }

        public Ability GetById(int id)
        {
            return CloningService.Clone(_abilities.Single(x => x.Id == id));
        }
    }
}
