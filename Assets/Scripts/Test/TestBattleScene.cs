using Melon.Gameplay;
using Melon.Scene;
using Melon.UI;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Melon.Test
{
    public class TestBattleScene : MonoBehaviour
    {
        [SerializeField]
        TestUIBattle TestUIBattle;

        [SerializeField]
        UIEquipsInBattle UIEquipsInBattle;

        [SerializeField]
        UIEquip UIEquip;

        [SerializeField]
        SBattleScene BattleScene;

        [SerializeField]
        SBattleChar BattleChar;

        [SerializeField]
        GameObject Warrior;

        [SerializeField]
        GameObject Priest;

        [SerializeField]
        GameObject Mage;

        [SerializeField]
        GameObject Guard;

        [SerializeField]
        GameObject BigSlime;

        [SerializeField]
        GameObject SmallSlime;

        readonly Dictionary<CharType, GameObject> BattleChars = new ();

        private void Awake()
        {
            UIEquipsInBattle.UIEquipLoader = () => Instantiate(UIEquip);
        }

        private void Start()
        {
            TestUIBattle.gameObject.SetActive(true);

            BattleChars[CharType.Warrior] = Warrior;
            BattleChars[CharType.Priest] = Priest;
            BattleChars[CharType.Mage] = Mage;
            BattleChars[CharType.Guard] = Guard;
            BattleChars[CharType.SmallSlime] = SmallSlime;
            BattleChars[CharType.BigSlime] = BigSlime;

            SBattleScene.BattleCharLoader = (charType) =>
            {
                var battleChar = Instantiate(BattleChar);
                var charBody = Instantiate(BattleChars[charType]);
                charBody.transform.SetParent(battleChar.CharBodyRoot);
                charBody.transform.localPosition = Vector3.zero;
                charBody.transform.localScale = Vector3.one;
                charBody.SetActive(true);

                return battleChar;
            };

            var battle = new Battle();
            battle.AddRule(new RBasic());

            // equips and their rules
            UIEquipsInBattle.gameObject.SetActive(true);
            UIEquipsInBattle.AddEquip(new ESpadePlus() { PlusAmount = 1 });
            UIEquipsInBattle.AddEquip(new ESpadeTimes() { Times = 2 });


            foreach (var uiEquip in UIEquipsInBattle.UIEquips)
            {
                battle.AddRule(uiEquip.Equip.Rule);
                TestUIBattle.Rules.Add(uiEquip.Equip.Rule);
            }

            // heros
            battle.Ours[0] = new BattleChar() { Char = new Char() { Type = CharType.Warrior }, MaxHp = 10, Hp = 5, HpHealingTop = 7 };
            battle.Ours[1] = new BattleChar() { Char = new Char() { Type = CharType.Priest }, MaxHp = 10, Hp = 5, HpHealingTop = 7 };
            battle.Ours[2] = new BattleChar() { Char = new Char() { Type = CharType.Mage }, MaxHp = 10, Hp = 5, HpHealingTop = 7, Block = 5 };
            battle.Ours[3] = new BattleChar() { Char = new Char() { Type = CharType.Guard }, MaxHp = 10, Hp = 5, HpHealingTop = 7, Block = 10 };

            // monsters
            battle.Theirs[0] = new BattleChar() { Char = new Char() { Type = CharType.SmallSlime }, MaxHp = 10, Hp = 10 };
            battle.Theirs[2] = new BattleChar() { Char = new Char() { Type = CharType.BigSlime }, MaxHp = 10, Hp = 10 };

            BattleScene.gameObject.SetActive(true);
            BattleScene.Load(battle);

            TestUIBattle.UIBattle.OnCardsPlay.AddListener(() =>
            {
                var cards = TestUIBattle.UIBattle.UICardsInHand.SelectedCards;
                battle.PlayCards(cards);

                foreach (var btChar in battle.Ours)
                    BattleScene.GetSBattleChar(btChar)?.RefreshAttrs();

                foreach (var btChar in battle.Theirs)
                    BattleScene.GetSBattleChar(btChar)?.RefreshAttrs();
            });

            TestUIBattle.RefreshCardsInHand();
        }
    }
}
