using Framework;
using Melon.Test;
using Melon.UI;
using UnityEngine;

namespace Melon.Test
{
    public class TestUICardsInHand : MonoBehaviour
    {
        [SerializeField]
        UICard UICard;

        [SerializeField]
        UICardsInHand UICardsInHand;

        readonly RandomCardSet randomCardSet = new();

        private void Awake()
        {
            UICardsInHand.UICardLoader = () => Instantiate(UICard);
        }

        private void Start()
        {
            UICardsInHand.gameObject.SetActive(true);

            var cards = randomCardSet.GetNextN(5);
            foreach (var card in cards)
                UICardsInHand.AddCard(card);
        }
    }
}
