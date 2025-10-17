using Melon.Gameplay;
using Melon.Scene;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;
using static UnityEngine.GraphicsBuffer;

namespace Melon.Effect
{
    public interface ITextEffect
    {
        string Text { set; }
    }

    public abstract class Effect : MonoBehaviour
    {
        public abstract void Play(Action onStopped);

        public abstract void Stop();

        public event Action<Effect> OnEnded = null;
    }

    public class EffectManager : MonoBehaviour
    {
        [SerializeField]
        Transform SceneEffectRoot;

        public Func<BattleChar, SBattleChar> GetSBattleChar = null;

        public Action<string, Action<Effect>> EffectLoader = null;

        public event Action OnAllEffectsEnded = null;

        readonly List<Effect> effectsInPlaying = new();

        void WhenEffectEnded(Effect effect)
        {
            effectsInPlaying.Remove(effect);
            if (effectsInPlaying.Count == 0)
                OnAllEffectsEnded?.Invoke();
        }

        public void AddEmptyEffect()
        {
            var effectNode = new GameObject("Effect");
            var effect = effectNode.AddComponent<EEmpty>();
            effectNode.transform.SetParent(SceneEffectRoot, false);
            effect.transform.SetParent(effectNode.transform, false);
            effect.gameObject.SetActive(true);
            effectsInPlaying.Add(effect);
            effect.Play(() =>
            {
                effectNode.transform.SetParent(null);
                Destroy(effect);
                Destroy(effectNode);

                WhenEffectEnded(effect);
            });
        }

        public void RegisterBattle(Battle bt)
        {
            void playTargetEffect(string effectType, BattleChar target, Action<Effect> onLoaded)
            {
                EffectLoader(effectType, (effect) =>
                {
                    if (target == null)
                        return;

                    var effectNode = new GameObject("Effect");
                    effectNode.transform.SetParent(SceneEffectRoot, false);
                    effectNode.transform.position = GetSBattleChar(target).transform.position;

                    effect.transform.SetParent(effectNode.transform, false);
                    effect.gameObject.SetActive(true);
                    effectsInPlaying.Add(effect);
                    effect.Play(() =>
                    {
                        effectNode.transform.SetParent(null);
                        Destroy(effect);
                        Destroy(effectNode);

                        WhenEffectEnded(effect);
                    });

                    onLoaded?.Invoke(effect);
                });
            }

            void playOwnerActingEfect(BattleChar battleChar)
            {
                var sBattleChar = GetSBattleChar(battleChar);
                var effectNode = new GameObject("Effect");
                var effect = effectNode.AddComponent<EBattleCharActing>();
                effect.SBattleChar = sBattleChar;
                effectNode.transform.SetParent(SceneEffectRoot, false);
                effect.transform.SetParent(effectNode.transform, false);
                effect.gameObject.SetActive(true);
                effectsInPlaying.Add(effect);
                effect.Play(() =>
                {
                    effectNode.transform.SetParent(null);
                    Destroy(effect);
                    Destroy(effectNode);

                    WhenEffectEnded(effect);
                });
            }

            void damageEffect(Damage damage)
            {
                // Play damage effect
                foreach (var target in damage.Targets)
                {
                    if (target != null && damage.FinalDamage.ContainsKey(target))
                    {
                        playTargetEffect("Damage", target, 
                            effect => (effect as ITextEffect).Text = "-" + damage.FinalDamage[target].ToString());
                    }
                }

                if (damage.Owner != null)
                    playOwnerActingEfect(damage.Owner);
            }

            void healingEffect(Healing healing)
            {
                // Play healing effect
                foreach (var target in healing.Targets)
                    playTargetEffect("Healing", target, effect => (effect as ITextEffect).Text = "+" + healing.Amount.ToString());

                if (healing.Owner != null)
                    playOwnerActingEfect(healing.Owner);
            }

            void blockEffect(Block block)
            {
                // Play block effect
                foreach (var target in block.Targets)
                    playTargetEffect("Block", target, effect => (effect as ITextEffect).Text = "+" + block.Amount.ToString());

                if (block.Owner != null)
                    playOwnerActingEfect(block.Owner);
            }

            bt.RegisterOn<Damage>(damageEffect);
            bt.RegisterOn<CardDamage>(damageEffect);
            bt.RegisterOn<Healing>(healingEffect);
            bt.RegisterOn<CardHealing>(healingEffect);
            bt.RegisterOn<Block>(blockEffect);
            bt.RegisterOn<CardBlock>(blockEffect);
        }
    }
}
