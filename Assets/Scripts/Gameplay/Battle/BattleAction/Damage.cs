using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melon.Gameplay
{
    public class Damage : BattleAction, ITargets, IAmount, ICardAction
    {
        public Card Card { get; set; } = null;

        public List<BattleChar> Targets { get; set; } = null;

        public Fixed64 Amount { get; set; }

        public override void Apply()
        {
            foreach (var target in Targets)
            {
                if (target == null || target.Hp <= 0)
                    continue;

                var finalDamage = Amount - target.Block;
                target.Block -= Amount;
                if (target.Block < 0)
                    target.Block = 0;

                if (finalDamage > 0)
                {
                    target.HpHealingTop = target.Hp; // take damage will reduce the healing top
                    target.Hp -= finalDamage;
                    if (target.Hp < 0)
                        target.Hp = 0;
                }
            }
        }
    }
}
