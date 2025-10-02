using Framework;
using Melon.Gameplay;
using Melon.Test;
using Melon.UI;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Melon.Test
{
    public class TestUIBattle : MonoBehaviour
    {
        [SerializeField]
        public UIBattle UIBattle;

        [SerializeField]
        UICard UICard;

        [SerializeField]
        UIEquip UIEquip;

        public List<Rule> Rules = new();

        readonly RandomCardSet randomCardSet = new();

        private void Awake()
        {
            UIBattle.UICardsInHand.UICardLoader = () => Instantiate(UICard);
        }

        private void Start()
        {
            UIBattle.gameObject.SetActive(true);

            Rules.Add(new RBasic());

            UIBattle.Validator = (cards) =>
            {
                foreach (var rule in Rules)
                {
                    if (rule.IsValid(cards))
                        return true;
                }

                return false;
            };

            UIBattle.OnCardsPlay.AddListener(() =>
            {
                foreach (var card in UIBattle.UICardsInHand.SelectedCards)
                    UIBattle.UICardsInHand.RemoveCard(card);

                while (UIBattle.UICardsInHand.Count < 5)
                    UIBattle.UICardsInHand.AddCard(randomCardSet.GetNext());
            });

            RefreshCardsInHand();
        }

        public void RefreshCardsInHand()
        {
            UIBattle.UICardsInHand.Clear();
            while (UIBattle.UICardsInHand.Count < 5)
                UIBattle.UICardsInHand.AddCard(randomCardSet.GetNext());
        }
    }
}
