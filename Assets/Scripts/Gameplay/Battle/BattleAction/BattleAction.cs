using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEngine.GraphicsBuffer;

namespace Melon.Gameplay
{
    public interface ICardAction
    {
        Card Card { get; set; }
    }

    public interface ITargets
    {
        ITargetSelector TargetSelector { get; }

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
        public BattleChar Owner { get; set; }

        public virtual void Prepare() { }

        public virtual void Apply()
        {
            // do nothing
        }
    }

    public class BattleTargetAction : BattleAction, ITargets
    {
        public ITargetSelector TargetSelector { get; set; } = null;

        public List<BattleChar> Targets { get; } = new();

        public override void Prepare()
        {
            SelectTargets();
        }

        public void SelectTargets()
        {
            if (TargetSelector == null)
                return;

            var teamMates = Owner.GetTeamMates();
            var opponents = Owner.GetOpponents();
            Targets.Clear();
            Targets.AddRange(TargetSelector.GetTargets(Owner, teamMates, opponents));
        }
    }
}
