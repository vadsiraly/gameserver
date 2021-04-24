using GameServer.Model;
using GameServer.Model.BaseTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    public class UnitManager
    {
        private readonly List<Unit> _units;

        public UnitManager()
        {
            _units = JsonConvert.DeserializeObject<List<Unit>>(Resources.Units);
        }

        public Unit Get(string name)
        {
            return CloningService.Clone(_units.Single(x => x.Name == name));
        }

        public Unit GetById(int id)
        {
            return CloningService.Clone(_units.Single(x => x.Id == id));
        }
    }
}
