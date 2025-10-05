using Melon.Gameplay;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Melon.Scene
{
    public class SBattleScene : MonoBehaviour
    {
        [SerializeField]
        Transform[] OurPositions;

        [SerializeField]
        Transform[] TheirPositions;

        [SerializeField]
        public Transform EffectRoot;

        readonly Dictionary<BattleChar, SBattleChar> BattleCharMap = new();

        public SBattleChar GetSBattleChar(BattleChar btChar) => (btChar != null && BattleCharMap.ContainsKey(btChar)) ? BattleCharMap[btChar] : null;

        public Func<CharType, SBattleChar> BattleCharLoader = null;

        public void Load(Battle battle)
        {
            for (int i = 0; i < battle.Ours.Length; i++)
            {
                var btChar = battle.Ours[i];
                if (btChar == null)
                    continue;

                var charRoot = OurPositions[i];
                var battleChar = BattleCharLoader(btChar.Char.Type);
                battleChar.transform.SetParent(charRoot);
                battleChar.transform.localPosition = Vector3.zero;
                battleChar.transform.localScale = Vector3.one;
                battleChar.gameObject.SetActive(true);

                battleChar.BattleChar = btChar;
                BattleCharMap[btChar] = battleChar;
            }

            for (int i = 0; i < battle.Theirs.Length; i++)
            {
                var btChar = battle.Theirs[i];
                if (btChar == null)
                    continue;

                var battleChar = BattleCharLoader(btChar.Char.Type);
                battleChar.transform.SetParent(TheirPositions[i]);
                battleChar.transform.localPosition = Vector3.zero;
                battleChar.transform.localScale = Vector3.one;
                battleChar.ReverseX(true);
                battleChar.gameObject.SetActive(true);

                battleChar.BattleChar = btChar;
                BattleCharMap[btChar] = battleChar;
            }
        }
    }
}
