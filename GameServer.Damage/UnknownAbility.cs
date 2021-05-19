using GameServer.Interfaces;

namespace GameServer.Damages
{
    public class UnknownSource : IDamageSource
    {
        public int Id { get; }
        public string Reference { get; }
        public string Name { get; }

        public UnknownSource()
        {
            Id = 4;

            Reference = "ability_unknown";
            Name = "Unknown ability";
        }
    }
}
