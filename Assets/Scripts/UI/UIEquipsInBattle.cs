using Melon.Gameplay;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Melon.UI
{
    public class UIEquipsInBattle : MonoBehaviour
    {
        [SerializeField]
        RectTransform EquipContainer;

        readonly List<UIEquip> uiEquips = new();

        public int Count { get => uiEquips.Count; }

        public Func<UIEquip> UIEquipLoader = null;
        public List<UIEquip> UIEquips { get => uiEquips.ToList(); }

        public void AddEquip(Equip equip)
        {
            UIEquip uiEquip = UIEquipLoader();
            uiEquip.Equip = equip;
            uiEquip.gameObject.SetActive(true);
            uiEquip.Refresh();

            uiEquip.transform.SetParent(EquipContainer);

            uiEquips.Add(uiEquip);
        }

        public void RemoveEquip(Equip equip)
        {
            foreach (var uiEquip in uiEquips)
            {
                if (uiEquip.Equip == equip)
                {
                    uiEquips.Remove(uiEquip);
                    Destroy(uiEquip);
                    return;
                }
            }
        }

        public void Clear()
        {
            foreach (var uiEquip in uiEquips)
                Destroy(uiEquip);

            uiEquips.Clear();
        }
    }
}
