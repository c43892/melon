
using Framework;
using System.Collections.Generic;

namespace Melon.Gameplay
{

    public class RSpadePlus : Rule, IEquipRule
    {
        public Fixed64 PlusAmount { get; set; } = 0;

        public Equip Equip { get; set; }

        public override void BeforeAction(BattleAction action, BattleContext context)
        {
            var cardRule = action as ICardAction;
            var card = cardRule?.Card;
            if (card?.Suit == CardSuit.Spades && action is IAmount)
                (action as IAmount).Amount += PlusAmount;
        }
    }
}
