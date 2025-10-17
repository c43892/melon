using Melon.Gameplay;
using Melon.Scene;
using System;
using TMPro;
using UnityEngine;

namespace Melon.Effect
{
    public class EBattleChar : EAnimation
    {
        public virtual string AniName { get; set; } = null;

        public SBattleChar SBattleChar
        {
            set
            {
                sBattleChar = value;
                Animation = sBattleChar.GetComponent<Animation>();
            }
        } SBattleChar sBattleChar;

        public override void Play(Action onStopped)
        {
            base.Play(onStopped);
            if (AniName != null)
                Animation.Play(AniName);
        }
    }

    public class EBattleCharActing : EBattleChar
    {
        public EBattleCharActing() => AniName = "Acting";
    }
}
