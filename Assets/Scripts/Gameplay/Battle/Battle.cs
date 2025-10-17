using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace Melon.Gameplay
{
    public class Battle : IActionRunner
    {
        BattleHero[] Heros { get; } = new BattleHero[4];

        BattleMonster[] Monsters { get; } = new BattleMonster[4];

        public List<Rule> Rules { get; private set; } = new();

        readonly BattleContext context = null;

        public Battle()
        {
            context = new BattleContext() { Battle = this };
        }

        public void SetHero(int index, BattleHero battleChar)
        {
            Heros[index] = battleChar;
            battleChar.Battle = this;
        }

        public void SetMonster(int index, BattleMonster battleChar)
        {
            Monsters[index] = battleChar;
            battleChar.Battle = this;
        }

        public BattleHero[] GetHeros() => Heros;

        public BattleMonster[] GetMonsters() => Monsters;

        public void PlayCards(IEnumerable<Card> cards)
        {
            Run(new PlayingCards() { Cards = cards.ToList() });
        }

        public void PlayerEndTurn()
        {
            Run(new PlayerEndTurn());
        }

        public bool MonsterAct(int i)
        {
            var enemy = Monsters[i];
            var ai = (enemy as IBattleCharWithAI)?.AI;
            if (ai == null || !ai.Active)
                return false;

            ai.Act();
            return true;
        }
        
        public void MonsterEndTurn()
        {
            Run(new EnemyEndTurn());
        }
        public void AddRule(Rule rule)
        {
            rule.Runner = this;
            Rules.Add(rule);
        }

        public void RemoveRule(Rule rule)
        {
            rule.Runner = null;
            Rules.Remove(rule);
        }

        readonly Dictionary<Type, List<Action<BattleAction>>> beforeActions = new();
        readonly Dictionary<Type, List<Action<BattleAction>>> onActions = new();
        readonly Dictionary<Type, List<Action<BattleAction>>> afterActions = new();

        public void RegisterBefore<T>(Action<T> beforeAction) where T : BattleAction
        {
            var type = typeof(T);
            if (!beforeActions.ContainsKey(type))
                beforeActions[type] = new();

            beforeActions[type].Add(a => beforeAction(a as T));
        }

        public void RegisterOn<T>(Action<T> onAction) where T : BattleAction
        {
            var type = typeof(T);
            if (!onActions.ContainsKey(type))
                onActions[type] = new();

            onActions[type].Add(a => onAction(a as T));
        }

        public void RegisterAfter<T>(Action<T> afterAction) where T : BattleAction
        {
            var type = typeof(T);
            if (!afterActions.ContainsKey(type))
                afterActions[type] = new();

            afterActions[type].Add(a => afterAction(a as T));
        }

        public void Run(BattleAction action)
        {
            var type = action.GetType();
            if (beforeActions.ContainsKey(type))
                foreach (var before in beforeActions[type])
                    before(action);

            foreach (var rule in Rules)
                rule.BeforeAction(action, context);

            action.Prepare();
            action.Apply();
            if (onActions.ContainsKey(type))
                foreach (var on in onActions[type])
                    on(action);

            foreach (var rule in Rules)
                rule.AfterAction(action, context);

            if (afterActions.ContainsKey(type))
                foreach (var after in afterActions[type])
                    after(action);
        }
    }
}
