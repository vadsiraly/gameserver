using GameServer.Interfaces;

namespace GameServer.Damages
{
    public class ArmorReduction : IDamageSource
    {
        public int Id { get; }
        public string Reference { get; }
        public string Name { get; }

        public ArmorReduction()
        {
            Id = 3;

            Reference = "target_armor_reduction";
            Name = "Target Armor Reduction";
        }
    }
}
