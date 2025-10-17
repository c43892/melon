using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melon.Gameplay
{
    public class Damage : BattleTargetAction, IAmount
    {
        public Fixed64 Amount { get; set; }

        public readonly Dictionary<BattleChar, Fixed64> FinalDamage = new();

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

                FinalDamage[target] = 0;
                if (finalDamage > 0)
                {
                    target.HpHealingTop = target.Hp; // take damage will reduce the healing top
                    target.Hp -= finalDamage;
                    if (target.Hp < 0)
                        target.Hp = 0;

                    FinalDamage[target] = finalDamage;
                }
            }
        }
    }
}
