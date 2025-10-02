using Melon.Gameplay;
using Mono.Cecil.Cil;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Melon.UI
{
    public class UICard : MonoBehaviour
    {
        [SerializeField]
        RectTransform FrameRect;

        [SerializeField]
        Image SuitIconTopLeft;

        [SerializeField]
        Image SuitIconBottomRight;

        [SerializeField]
        Image SuitIconCenter;

        [SerializeField]
        Text NumTopLeft;

        [SerializeField]
        Text NumBottomRight;

        public event System.Action OnClicked = null;

        [SerializeField]
        public void _OnClicked()
        {
            OnClicked?.Invoke();
        }

        public Card Card
        {
            get => card;
            set
            {
                card = value;

                Color color = getColorBySuit(card.Suit);
                SuitIconTopLeft.color = color;
                SuitIconCenter.color = color;
                SuitIconBottomRight.color = color;

                string text = getTextByValue(card.Value);
                NumTopLeft.text = text;
                NumBottomRight.text = text;
            }
        } Card card;

        public bool Selected
        {
            get => selected;
            set
            {
                selected = value;
                FrameRect.offsetMin = new Vector2(0, selected ? 25 : 0);
                FrameRect.offsetMax = new Vector2(0, selected ? 25 : 0);
            }
        } bool selected = false;

        static Color getColorBySuit(CardSuit suit)
        {
            return suit switch
            {
                CardSuit.Hearts => Color.red,
                CardSuit.Diamonds => Color.pink,
                CardSuit.Clubs => Color.blue,
                CardSuit.Spades => Color.black,
                _ => Color.white,
            };
        }

        static string getTextByValue(CardValue value)
        {
            return value switch
            {
                CardValue.King => "K",
                CardValue.Queen => "Q",
                CardValue.Jack => "J",
                CardValue.Ten => "10",
                CardValue.Nine => "9",
                CardValue.Eight => "8",
                CardValue.Seven => "7",
                CardValue.Six => "6",
                CardValue.Five => "5",
                CardValue.Four => "4",
                CardValue.Three => "3",
                CardValue.Two => "2",
                CardValue.One => "A",
                _ => "?",
            };
        }
    }
}
