using Framework;
using Melon.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine.Rendering;

namespace Melon.Gameplay
{
    public class BattleCharAIActionLoop : IBattleCharAI
    {
        public BattleChar Owner { get; private set; }

        public bool Active { get => Owner.Hp > 0; }

        public readonly List<BattleAction> Actions = new();

        int currentActionIndex = 0;

        public BattleCharAIActionLoop(BattleChar owner)
        {
            Owner = owner;
        }

        public void Init()
        {
        }

        public void Act()
        {
            currentActionIndex = 0;
            var aciontsCount = Actions.Count;

            if (!Active || aciontsCount == 0)
                return;

            currentActionIndex = currentActionIndex % aciontsCount;
            var nextAction = Actions[currentActionIndex];
            currentActionIndex++;

            Owner.Battle.Run(nextAction);
        }
    }
}
