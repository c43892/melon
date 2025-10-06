using Melon.Gameplay;
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Melon.UI
{
    public class UIBattle : MonoBehaviour
    {
        [SerializeField]
        Button BtnPlay;

        [SerializeField]
        public UICardsInHand UICardsInHand;

        public Func<BattleChar, IEnumerable<Card>, bool> Validator = null;

        public UnityEvent OnCardsPlay = null;

        private void Awake()
        {
            UICardsInHand.OnCardSelectedChanged += OnCardsSelectedChanged;
        }

        private void Start()
        {
            UICardsInHand.gameObject.SetActive(true);
            OnCardsSelectedChanged();
        }

        public void OnCardsSelectedChanged()
        {
            var cards = UICardsInHand.SelectedCards;
            BtnPlay.interactable = Validator?.Invoke(null, cards) ?? false;
        }

        public void PlayCards()
        {
            OnCardsPlay?.Invoke();
        }
    }
}
