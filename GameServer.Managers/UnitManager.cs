using GameServer.Model.Units;
using System;
using System.Collections.Generic;

namespace GameServer.Managers
{
    public static class UnitManager
    {
        public static List<string> GetAvailableUnits()
        {
            var units = new List<string>();
            units.Add("Gulp");
            units.Add("Lyra");
            units.Add("SecretiveGirl");
            units.Add("PlagueDoctor");

            return units;
        }

        public static Unit GetUnit(string name)
        {
            Type t = Type.GetType($"GameServer.Model.Units.ConcreteUnits.{name}");
            return Activator.CreateInstance(t) as Unit;
        }
    }
}
