using Melon.Gameplay;
using System;
using UnityEngine;
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
    }

    public class EffectManager : MonoBehaviour
    {
        [SerializeField]
        Transform SceneEffectyRoot;

        public Func<BattleChar, Transform> GetBattleCharTransform = null;

        public Action<string, Action<Effect>> EffectLoader = null;

        public void RegisterBattle(Battle bt)
        {
            Action<string, BattleChar, Action<Effect>> playTargetEffect = (effectType, target, onLoaded) =>
            {
                EffectLoader(effectType, (effect) =>
                {
                    if (target == null)
                        return;

                    var effectNode = new GameObject("Effect");
                    effectNode.transform.SetParent(SceneEffectyRoot, false);
                    effectNode.transform.position = GetBattleCharTransform(target).position;

                    effect.transform.SetParent(effectNode.transform, false);
                    effect.gameObject.SetActive(true);
                    effect.Play(() =>
                    {
                        effectNode.transform.SetParent(null);
                        Destroy(effect);
                        Destroy(effectNode);
                    });

                    onLoaded?.Invoke(effect);
                });
            };

            bt.RegisterOn<Damage>(damage =>
            {
                // Play damage effect
                foreach (var target in damage.Targets)
                    playTargetEffect("Damage", target, effect => (effect as ITextEffect).Text = "-" + damage.Amount.ToString());
            });

            bt.RegisterOn<Healing>(healing =>
            {
                // Play healing effect
                foreach (var target in healing.Targets)
                    playTargetEffect("Healing", target, effect => (effect as ITextEffect).Text = "+" + healing.Amount.ToString());
            });

            bt.RegisterOn<Block>(block =>
            {
                // Play block effect
                foreach (var target in block.Targets)
                    playTargetEffect("Block", target, effect => (effect as ITextEffect).Text = "+" + block.Amount.ToString());
            });
        }
    }
}
