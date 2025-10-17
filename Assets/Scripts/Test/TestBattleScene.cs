using Melon.Effect;
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
        TestBattleEffects TestBattleEffects;

        [SerializeField]
        UIEquipsInBattle UIEquipsInBattle;

        [SerializeField]
        UIEquip UIEquip;

        [SerializeField]
        public SBattleScene BattleScene;

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

        public Battle Battle { get; private set; } = new();

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

            BattleScene.BattleCharLoader = (charType) =>
            {
                var battleChar = Instantiate(BattleChar);
                var charBody = Instantiate(BattleChars[charType]);
                charBody.transform.SetParent(battleChar.CharBodyRoot);
                charBody.transform.localPosition = Vector3.zero;
                charBody.transform.localScale = Vector3.one;
                charBody.SetActive(true);

                return battleChar;
            };

            Battle.AddRule(new RBasic());

            // equips and their rules
            UIEquipsInBattle.gameObject.SetActive(true);
            UIEquipsInBattle.AddEquip(new ESpadePlus() { PlusAmount = 1 });
            UIEquipsInBattle.AddEquip(new ESpadeTimes() { Times = 2 });


            foreach (var uiEquip in UIEquipsInBattle.UIEquips)
            {
                Battle.AddRule(uiEquip.Equip.Rule);
                TestUIBattle.Rules.Add(uiEquip.Equip.Rule);
            }

            // heros
            Battle.SetHero(0, new BattleHero() { Char = new Char() { Type = CharType.Warrior }, CardSuite = CardSuit.Spades, MaxHp = 10, Hp = 5, HpHealingTop = 7 });
            Battle.SetHero(1, new BattleHero() { Char = new Char() { Type = CharType.Priest }, CardSuite = CardSuit.Hearts, MaxHp = 10, Hp = 5, HpHealingTop = 7 });
            Battle.SetHero(2, new BattleHero() { Char = new Char() { Type = CharType.Mage }, CardSuite = CardSuit.Clubs, MaxHp = 10, Hp = 5, HpHealingTop = 7, Block = 5 });
            Battle.SetHero(3, new BattleHero() { Char = new Char() { Type = CharType.Guard }, CardSuite = CardSuit.Diamonds, MaxHp = 10, Hp = 5, HpHealingTop = 7, Block = 10 });

            // monsters

            var smallSlime = new BattleMonster() { Char = new Char() { Type = CharType.SmallSlime }, MaxHp = 10, Hp = 10, Attack = 2 };
            var ai4SmallSlime = new BattleCharAIActionLoop();
            ai4SmallSlime.Actions.Add(new Damage() { Amount = 2, TargetSelector = new TargetSelectorFirstOpponent() });
            smallSlime.AI = ai4SmallSlime;
            Battle.SetMonster(0, smallSlime);
            ai4SmallSlime.Init();

            var bigSlime = new BattleMonster() { Char = new Char() { Type = CharType.BigSlime }, MaxHp = 10, Hp = 10, Attack = 5 };            
            var ai4BigSlime = new BattleCharAIActionLoop();
            ai4BigSlime.Actions.Add(new Damage() { Amount = 3, TargetSelector = new TargetSelectorAllOpponents() });
            ai4BigSlime.Actions.Add(new Block() { Amount = 10, TargetSelector = new TargetSelectorOwner() });
            bigSlime.AI = ai4BigSlime;
            Battle.SetMonster(1, bigSlime);
            ai4BigSlime.Init();

            BattleScene.gameObject.SetActive(true);
            BattleScene.Load(Battle);

            var isHerosTurn = false;

            var heros = Battle.GetHeros();
            var monsters = Battle.GetMonsters();
            isHerosTurn = true;
            var monsterActingIndex = 0;
            TestUIBattle.UIBattle.OnCardsPlay.AddListener(() =>
            {
                TestUIBattle.UIBattle.BtnPlay.gameObject.SetActive(false);

                var cards = TestUIBattle.UIBattle.UICardsInHand.SelectedCards;
                Battle.PlayCards(cards);

                foreach (var btChar in heros)
                    BattleScene.GetSBattleChar(btChar)?.RefreshAttrs();

                foreach (var btChar in monsters)
                    BattleScene.GetSBattleChar(btChar)?.RefreshAttrs();

                Battle.PlayerEndTurn();
                TestBattleEffects.EffManager.AddEmptyEffect();
                isHerosTurn = false;
                monsterActingIndex = 0;
            });

            TestBattleEffects.EffManager.OnAllEffectsEnded += () =>
            {
                if (!isHerosTurn)
                {
                    if (monsterActingIndex < monsters.Count())
                    {
                        if (!Battle.MonsterAct(monsterActingIndex++))
                            TestBattleEffects.EffManager.AddEmptyEffect();
                    }
                    else
                    {
                        TestUIBattle.UIBattle.BtnPlay.gameObject.SetActive(true);
                        isHerosTurn = true;
                    }
                }

                foreach (var btChar in heros)
                    BattleScene.GetSBattleChar(btChar)?.RefreshAttrs();

                foreach (var btChar in monsters)
                    BattleScene.GetSBattleChar(btChar)?.RefreshAttrs();
            };

            TestUIBattle.RefreshCardsInHand();
        }
    }
}
