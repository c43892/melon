using Framework.Unity;
using Melon.Effect;
using Melon.Gameplay;
using Melon.Scene;
using Melon.UI;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;

namespace Melon.Test
{
    public class TestBattleEffects : MonoBehaviour
    {
        [SerializeField]
        TestBattleScene TestBattleScene;

        [SerializeField]
        public EffectManager EffManager;

        private void Start()
        {
            TestBattleScene.gameObject.SetActive(true);

            var battle = TestBattleScene.Battle;
            EffManager.EffectLoader = (name, onLoaded) =>
            {
                var assetPath = $"Assets/Prefabs/Scene/Effects/{name}/{name}.prefab";
                StartCoroutine(ResourceManager.LoadAssetCoroutine<Effect.Effect>(assetPath, 
                    (effect) => onLoaded?.Invoke(effect == null ? null : Instantiate(effect))));
            };

            EffManager.GetBattleCharTransform = (battleChar) => TestBattleScene.BattleScene.GetSBattleChar(battleChar)?.transform;
            EffManager.RegisterBattle(battle);
        }
    }
}
