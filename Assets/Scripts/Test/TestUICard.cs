using Framework;
using Melon.Test;
using Melon.UI;
using UnityEngine;

namespace Melon.Test
{
    public class TestUICard : MonoBehaviour
    {
        [SerializeField]
        UICard UICard;

        readonly RandomCardSet randomCardSet = new();

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            UICard.gameObject.SetActive(true);
            UICard.Card = randomCardSet.GetNext();

            UICard.OnClicked += () =>
            {
                UICard.Card = randomCardSet.GetNext();
                UICard.Selected = !UICard.Selected;
            };
        }
    }
}
