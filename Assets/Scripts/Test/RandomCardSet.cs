using Framework;
using Melon.Gameplay;

namespace Melon.Test
{
    public class RandomCardSet : ICardCollection
    {
        public Card GetNext() => GetNextN(1)[0];

        public Card[] GetNextN(int n)
        {
            // Generate n random cards
            Card[] cards = new Card[n];
            for (int i = 0; i < n; i++)
            {
                cards[i] = new()
                {
                    Suit = (CardSuit)UnityEngine.Random.Range(1, 5), // Assuming 4 types
                    Value = (CardValue)UnityEngine.Random.Range(1, 14) // Assuming values from 1 to 13
                };
            }

            return cards;
        }
    }
}
