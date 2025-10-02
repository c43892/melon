using Framework;
using Melon.Gameplay;
using System;
using TMPro;
using UnityEngine;

namespace Melon.Scene
{
    public class SBattleChar : MonoBehaviour
    {
        [SerializeField]
        public Transform CharBodyRoot;

        [SerializeField]
        SpriteRenderer HpBar;
        
        [SerializeField]
        SpriteRenderer HealingBar;

        [SerializeField]
        TextMeshPro HpValue;

        [SerializeField]
        SpriteRenderer BlockIcon;

        [SerializeField]
        TextMeshPro BlockValue;

        public BattleChar BattleChar
        {
            get => btChar;
            set
            {
                btChar = value;
                RefreshAttrs();
            }
        } BattleChar btChar = null;

        public void ReverseX(bool reverse)
        {
            CharBodyRoot.localScale = new Vector3(reverse ? -1 : 1, 1, 1);
        }

        public void RefreshAttrs()
        {
            Fixed64 hpPercentage = 0;
            Fixed64 hpXOffset = 0;

            if (btChar.Hp <= 0 || btChar.MaxHp <= 0)
                HpBar.gameObject.SetActive(false);
            else
            {
                HpBar.gameObject.SetActive(true);
                hpPercentage = btChar.Hp / btChar.MaxHp;
                hpXOffset = (1 - hpPercentage) / 2f;
                HpBar.transform.localScale = new Vector3((float)hpPercentage, 1, 1);
                HpBar.transform.localPosition = new Vector3(-(float)hpXOffset, 0, -0.2f);
            }

            if (btChar.HpHealingTop <= 0 || btChar.MaxHp <= 0)
                HealingBar.gameObject.SetActive(false);
            else
            {
                HealingBar.gameObject.SetActive(true);
                var healingPercentage = btChar.HpHealingTop > 0 && btChar.MaxHp > 0 ? btChar.HpHealingTop / btChar.MaxHp : 0;
                var healingXOffset = (1 - healingPercentage) / 2f;
                HealingBar.transform.localScale = new Vector3((float)healingPercentage, 1, 1);
                HealingBar.transform.localPosition = new Vector3(-(float)healingXOffset, 0, -0.1f);
            }

            HpValue.text = btChar.Hp.ToString();

            if (btChar.Block <= 0)
            {
                BlockValue.gameObject.SetActive(false);
                BlockIcon.gameObject.SetActive(false);
            }
            else
            {
                BlockValue.gameObject.SetActive(true);
                BlockIcon.gameObject.SetActive(true);
                BlockValue.text = btChar.Block.ToString();
            }
        }
    }
}
