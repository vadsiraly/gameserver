using GameServer.Interfaces;

namespace GameServer.Interfaces.Events
{
    public class EffectEventArgs
    {
        public EffectEventArgs(IUnit self, IEffect effect)
        {
            Self = self;
            Effect = effect;
        }

        public IUnit Self { get; private set; }
        public IEffect Effect { get; private set; }
    }
}
