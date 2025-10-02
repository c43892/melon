
using System.Collections.Generic;

namespace Melon.Gameplay
{
    public enum CardSuit
    {
        Unknown = 0,
        Spades  = 1,
        Hearts  = 2,
        Clubs   = 3,
        Diamonds = 4,
    }

    public enum CardValue
    {
        Unknown = 0,
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5,
        Six = 6,
        Seven = 7,
        Eight = 8,
        Nine = 9,
        Ten = 10,
        Jack = 11,
        Queen = 12,
        King = 13,
    }

    public class Card
    {
        public CardSuit Suit = CardSuit.Unknown;
        public CardValue Value = CardValue.Unknown;
    }
}
