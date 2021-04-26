using GameServer.Model.BaseTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Services
{
	public static class CloningService
	{
		public static T Clone<T>(this T source)
		{
			// Don't serialize a null object, simply return the default for that object
			if (ReferenceEquals(source, null))
			{
				return default(T);
			}

			var deserializeSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace };
			var serializeSettings = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
			return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(source, serializeSettings), deserializeSettings);
		}
		public static Unit Clone(this Unit source)
		{
			// Don't serialize a null object, simply return the default for that object
			if (ReferenceEquals(source, null))
			{
				return default(Unit);
			}

			var copy = new Unit(
				source.Id, 
				source.Name, 
				source.Health, 
				source.Mana, 
				source.MaxHealth, 
				source.MaxMana, 
				source.Damage, 
				source.Armor, 
				source.Resistance, 
				source.Speed, 
				source.CriticalChance, 
				source.CriticalMultiplier,  
				source.Abilities.Select(x => x.Reference).ToList());

			return copy;
		}
	}
}
