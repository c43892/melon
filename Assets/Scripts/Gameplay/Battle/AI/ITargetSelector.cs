using System;
using System.Linq;

namespace Melon.Gameplay
{
    public interface ITargetSelector
    {
        public BattleChar[] GetTargets(BattleChar owner, BattleChar[] teamMates, BattleChar[] opponents);
    }

    // Select all allies
    public class TargetSelectorAllOpponents : ITargetSelector
    {
        public BattleChar[] GetTargets(BattleChar owner, BattleChar[] teamMates, BattleChar[] opponents)
            => opponents.Where(c => c != null && c.Hp > 0).ToArray();
    }

    // Select all allies
    public class TargetSelectorFirstOpponent : ITargetSelector
    {
        public BattleChar[] GetTargets(BattleChar owner, BattleChar[] teamMates, BattleChar[] opponents)
        {
            var aliveOpponents = opponents.Where(c => c != null && c.Hp > 0).ToArray();
            return aliveOpponents.Length > 0 ? new BattleChar[] { aliveOpponents[0] } : Array.Empty<BattleChar>();
        }
    }

    public class TargetSelectorOwner : ITargetSelector
    {
        public BattleChar[] GetTargets(BattleChar owner, BattleChar[] teamMates, BattleChar[] opponents)
            => new BattleChar[] { owner };
    }

    public class  TargetSelectorAllTeamMates : ITargetSelector
    {
        public BattleChar[] GetTargets(BattleChar owner, BattleChar[] teamMates, BattleChar[] opponents)
            => teamMates.Where(c => c != null && c.Hp > 0).ToArray();
    }
}
