using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melon.Gameplay
{
    public class Block : BattleTargetAction, IAmount
    {
        public Fixed64 Amount { get; set; }

        public override void Apply()
        {
            foreach (var target in Targets)
            {
                if (target == null || target.Hp <= 0)
                    continue;

                target.Block += Amount;
            }
        }
    }
}
