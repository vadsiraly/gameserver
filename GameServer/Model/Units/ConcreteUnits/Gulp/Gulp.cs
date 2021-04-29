using GameServer.Model.Abilities;
using GameServer.Model.Abilities.ConcreteAbilities;
using GameServer.Model.Abilities.ConcreteAbilities.Gulp;
using GameServer.Model.Abilities.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer.Model.Units.ConcreteUnits.Gulp
{
    public class Gulp : Unit
    {
        private double health;
        private double mana;
        private double speed;

        public Gulp(Random random)
        {
            Health = 50;
            Mana = 100;
            Speed = 80;

            BasicAttack = new BasicAttack(this, random);
            Abilities.Add(new DevourWeapon(this, random));
        }

        public override string Name => "Gulp";
        public override double Health { get => health; protected set => health = value; }
        public override double Mana { get => mana; protected set => mana = value; }
        public override double Speed { get => speed; protected set => speed = value; }

        public override IAbility BasicAttack { get; protected set; }
        public override List<IAbility> Abilities { get; protected set; } = new List<IAbility>();

        public override List<Effect> Buffs { get; protected set; }
        public override List<Effect> Debuffs { get; protected set; }

        public override void Attack(List<Unit> targets)
        {
            BeforeAttack(new AttackEventArgs(this, targets, BasicAttack));

            BasicAttack.Use(targets);

            AfterAttack(new AttackEventArgs(this, targets, BasicAttack));
        }

        public override void UseAbility(List<Unit> targets, IAbility ability)
        {
            BeforeAbilityUsed(new AttackEventArgs(this, targets, ability));



            AfterAbilityUsed(new AttackEventArgs(this, targets, ability));
        }
    }
}
