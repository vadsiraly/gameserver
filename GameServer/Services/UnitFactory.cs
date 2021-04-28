using GameServer.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
    public class UnitFactory
    {
        private readonly List<Unit> _units;

        public UnitFactory()
        {
            _units = JsonConvert.DeserializeObject<List<Unit>>(Resources.Units);
        }

        public Unit GetByName(string name)
        {
            return CloningService.Clone(_units.Single(x => x.Name == name));
        }

        public Unit GetById(int id)
        {
            return CloningService.Clone(_units.Single(x => x.Id == id));
        }
    }
}
