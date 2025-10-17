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
        public BattleChar Owner
        {
           get => owner;
           set
           {
                owner = value;
                foreach(var action in Actions)
                    action.Owner = value;
            }
        } BattleChar owner = null;

        public bool Active { get => Owner.Hp > 0; }

        public readonly List<BattleAction> Actions = new();

        int currentActionIndex = 0;

        public void Init()
        {
            currentActionIndex = 0;
        }

        public void Act()
        {
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
