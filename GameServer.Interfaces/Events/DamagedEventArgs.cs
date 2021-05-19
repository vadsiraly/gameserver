using GameServer.Interfaces;

namespace GameServer.Interfaces.Events
{
    public class DamagedEventArgs
    {
        public DamagedEventArgs(IAbility source, IModifiedDamage modifiedDamage)
        {
            Source = source;
            ModifiedDamage = modifiedDamage;
        }

        public IAbility Source { get; private set; }
        public IModifiedDamage ModifiedDamage { get; private set; }
    }
}
