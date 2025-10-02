
using Framework;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Melon.Gameplay
{
    public interface IEquipRule
    {
        Equip Equip { get; }
    }

    public interface IRuleRunner
    {
        void Run(BattleAction action, BattleContext context);
    }

    public class Rule
    {
        public IRuleRunner Runner { get; set; }

        public virtual bool IsValid(IEnumerable<Card> cards) => false;

        public virtual void BeforeAction(BattleAction action, BattleContext context) { }

        public virtual void AfterAction(BattleAction action, BattleContext context) { }
    }
}
