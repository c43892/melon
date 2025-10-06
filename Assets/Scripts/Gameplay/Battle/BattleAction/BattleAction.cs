using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melon.Gameplay
{
    public interface ICardAction
    {
        Card Card { get; set; }
    }

    public interface ITargets
    {
        List<BattleChar> Targets { get; }
    }

    public interface IAmount
    {
        Fixed64 Amount { get; set; }
    }

    public interface IActionRunner
    {
        void Run(BattleAction action);

        void RegisterBefore<T>(Action<T> beforeAction) where T : BattleAction;

        void RegisterOn<T>(Action<T> onAction) where T : BattleAction;

        void RegisterAfter<T>(Action<T> afterAction) where T : BattleAction;
    }

    public class BattleAction
    {
        public virtual void Apply()
        {
            // do nothing
        }
    }
}
