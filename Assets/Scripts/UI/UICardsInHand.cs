using Melon.Gameplay;
using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

namespace Melon.UI
{
    public class UICardsInHand : MonoBehaviour
    {
        [SerializeField]
        RectTransform CardContainer;

        readonly List<UICard> uiCards = new();

        public Func<UICard> UICardLoader = null;

        public event Action OnCardSelectedChanged = null;

        public int Count { get => uiCards.Count; }

        public Card[] SelectedCards { get => uiCards.Where(c => c.Selected).Select(c => c.Card).ToArray(); }

        public void AddCard(Card card)
        {
            UICard uiCard = UICardLoader();
            uiCard.Card = card;
            uiCard.gameObject.SetActive(true);
            uiCard.transform.SetParent(CardContainer);
            uiCard.Selected = false;

            uiCards.Add(uiCard);
            uiCard.OnClicked += () =>
            {
                uiCard.Selected = !uiCard.Selected;

                var cardsSelected = uiCards.Where(c => c.Selected).Select(c => c.Card);
                OnCardSelectedChanged?.Invoke();
            };
        }

        public void RemoveCard(Card card)
        {
            var uiCard = uiCards.FirstOrDefault(c => c.Card == card);
            if (uiCard != null)
            {
                uiCards.Remove(uiCard);
                Destroy(uiCard.gameObject);
            }

            OnCardSelectedChanged?.Invoke();
        }

        public void Clear()
        {
            foreach (var uiCard in uiCards)
                Destroy(uiCard.gameObject);

            uiCards.Clear();
        }
    }
}
