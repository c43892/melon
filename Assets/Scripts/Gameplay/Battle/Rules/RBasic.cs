
using System.Collections.Generic;
using System.Linq;

namespace Melon.Gameplay
{
    public class RBasic : Rule
    {
        public override void AfterAction(BattleAction act, BattleContext context)
        {
            if (act is not PlayingCards playingCards)
                return;

            var heros = context.Battle.GetHeros();
            var monsters = context.Battle.GetMonsters();
            var cards = playingCards.Cards;

            var spadeOwner = heros.FirstOrDefault(c => c != null && c.CardSuite == CardSuit.Spades);
            var heartOwner = heros.FirstOrDefault(c => c != null && c.CardSuite == CardSuit.Hearts);
            var clubOwner = heros.FirstOrDefault(c => c != null && c.CardSuite == CardSuit.Clubs);
            var diamondOwner = heros.FirstOrDefault(c => c != null && c.CardSuite == CardSuit.Diamonds);

            foreach (var card in cards)
            {
                Battle bt = context.Battle;

                switch (card.Suit)
                {
                    case CardSuit.Spades:
                        var target = monsters.FirstOrDefault(c => c != null && c.Hp > 0);
                        if (target != null)
                            Runner.Run(new CardDamage() { Owner = spadeOwner, TargetSelector = new TargetSelectorFirstOpponent(), Amount = (int)card.Value, Card = card });
                        break;
                    case CardSuit.Hearts:
                        Runner.Run(new CardHealing() { Owner = heartOwner, TargetSelector = new TargetSelectorAllTeamMates(), Amount = (int)card.Value, Card = card });
                        break;
                    case CardSuit.Clubs:
                        Runner.Run(new CardDamage() { Owner = clubOwner, TargetSelector = new TargetSelectorAllOpponents(), Amount = (int)card.Value, Card = card });
                        break;
                    case CardSuit.Diamonds:
                        Runner.Run(new CardBlock() { Owner = diamondOwner, TargetSelector = new TargetSelectorAllTeamMates(), Amount = (int)card.Value, Card = card });
                        break;
                    default:
                        break;
                }
            }
        }

        public override bool IsValid(BattleChar actor, IEnumerable<Card> cards)
        {
            // null actor means the player is acting, or the actor will be the acting monster
            return actor == null && cards != null && (IsSameKind(cards) || IsFlush(cards) || IsStraight(cards));
        }

        // The same value
        public bool IsSameKind(IEnumerable<Card> cards)
        {
            CardValue firstValue = CardValue.Unknown;
            foreach (var card in cards)
            {
                if (firstValue == CardValue.Unknown)
                    firstValue = card.Value;
                else if (card.Value != firstValue)
                    return false;
            }

            return firstValue != CardValue.Unknown;
        }

        // Three or more cards of the same suit
        public bool IsFlush(IEnumerable<Card> cards)
        {
            var count = 0;
            CardSuit firstSuit = CardSuit.Unknown;
            foreach (var card in cards)
            {
                if (firstSuit == CardSuit.Unknown)
                    firstSuit = card.Suit;
                else if (card.Suit != firstSuit)
                    return false;

                count++;
            }

            return firstSuit != CardSuit.Unknown && count >= 3;
        }

        // Three or more cards in a sequence
        public bool IsStraight(IEnumerable<Card> cards)
        {
            List<CardValue> values = new();
            foreach (var card in cards)
            {
                if (card.Value == CardValue.Unknown)
                    return false;

                values.Add(card.Value);
            }

            if (values.Count < 3)
                return false;

            values.Sort();
            for (int i = 1; i < values.Count; i++)
            {
                if (values[i] != values[i - 1] + 1 && !(values[i] == CardValue.King && values[i - 1] == CardValue.One))
                    return false;
            }

            return true;
        }
    }
}
