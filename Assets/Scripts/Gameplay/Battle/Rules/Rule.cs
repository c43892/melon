
using Framework;
using JetBrains.Annotations;
using System.Collections.Generic;

namespace Melon.Gameplay
{
    public interface IEquipRule
    {
        Equip Equip { get; }
    }

    public class Rule
    {
        public IActionRunner Runner { get; set; }

        public virtual bool IsValid(IEnumerable<Card> cards) => false;

        public virtual void BeforeAction(BattleAction action, BattleContext context) { }

        public virtual void AfterAction(BattleAction action, BattleContext context) { }
    }
}
