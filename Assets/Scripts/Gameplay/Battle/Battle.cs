using Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace Melon.Gameplay
{
    public class Battle : IRuleRunner
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

        public void Run(BattleAction action, BattleContext context)
        {
            foreach (var rule in Rules)
                rule.BeforeAction(action, context);

            action.Apply();

            foreach (var rule in Rules)
                rule.AfterAction(action, context);
        }
    }
}
