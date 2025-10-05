using Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace Melon.Gameplay
{
    public class Battle : IActionRunner
    {
        public BattleChar[] Ours { get; } = new BattleChar[4];

        public BattleChar[] Theirs { get; } = new BattleChar[4];

        public List<Rule> Rules { get; private set; } = new();

        public void PlayCards(IEnumerable<Card> cards)
        {
            BattleContext context = new() { Battle = this };

            Run(new PlayingCards() { Cards = cards.ToList() }, context);
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

        public void Run(BattleAction action, BattleContext context)
        {
            var type = action.GetType();
            if (beforeActions.ContainsKey(type))
                foreach (var before in beforeActions[type])
                    before(action);

            foreach (var rule in Rules)
                rule.BeforeAction(action, context);

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
