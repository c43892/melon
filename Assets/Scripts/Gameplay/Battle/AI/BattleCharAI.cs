using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Framework;

namespace Melon.Gameplay
{
    public interface IBattleCharAI 
    {
        BattleChar Owner { get; set; }

        bool Active { get; }

        void Init();

        void Act();
    }

    public interface IBattleCharWithAI
    {
        IBattleCharAI AI { get; }
    }
}
