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

    public class BattleAction
    {
        public virtual void Apply()
        {
            // do nothing
        }
    }
}
