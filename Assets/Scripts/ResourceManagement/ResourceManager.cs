namespace Framework.Unity
{
    // ResourceManager.cs
    // 简单统一的异步资源加载器：编辑器Play支持用项目路径；正式运行/Addressables支持用key。
    // 用法示例：
    //   // Addressables（推荐，打包可用）
    //   var go = await ResourceManager.InstantiateAsync("MyPrefabKey");
    //   // 编辑器里走项目路径（仅编辑器Play可用）
    //   var go2 = await ResourceManager.InstantiateAsync("Assets/Prefabs/MyPrefab.prefab");
    //   // 释放
    //   ResourceManager.ReleaseInstance(go);  // Addressables实例会走 Addressables.ReleaseInstance
    //   ResourceManager.Release(prefabAsset); // Addressables 资源；编辑器路径加载的则忽略
    //
    // 注意：要在打包环境工作，请务必使用 Addressables key，而不是 Assets 路径。

    using System;
    using System.Collections.Generic;
    using System.Collections;
    using System.Threading.Tasks;
    using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

#if ENABLE_ADDRESSABLES || UNITY_ADDRESSABLES // 任一宏都行，根据你的项目设置
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
#endif

    public static class ResourceManager
    {
        // 跟踪Addressables的加载句柄，便于Release
#if ENABLE_ADDRESSABLES || UNITY_ADDRESSABLES
    private static readonly Dictionary<object, AsyncOperationHandle> _assetHandles = new();
    private static readonly HashSet<GameObject> _instancedByAddressables = new();
#endif

        /// <summary>
        /// 异步加载资产（Prefab、Sprite、AudioClip等），Addressables优先；编辑器下可用Assets路径。
        /// </summary>
        public static async Task<T> LoadAssetAsync<T>(string keyOrAssetsPath) where T : UnityEngine.Object
        {
            if (string.IsNullOrEmpty(keyOrAssetsPath))
                throw new ArgumentException("keyOrAssetsPath 不能为空。");

            // 优先：编辑器下允许用 Assets 路径（仅在编辑器Play有效）
#if UNITY_EDITOR
            if (IsAssetsPath(keyOrAssetsPath))
            {
                // 保持异步语义：让渡一帧
                await Task.Yield();
                var asset = AssetDatabase.LoadAssetAtPath<T>(keyOrAssetsPath);
                if (asset == null)
                    throw new Exception($"在编辑器中，未找到资源：{keyOrAssetsPath}");
                return asset;
            }
#endif

            // 运行时/或你传的是 Addressables key
#if ENABLE_ADDRESSABLES || UNITY_ADDRESSABLES
        var handle = Addressables.LoadAssetAsync<T>(keyOrAssetsPath);
        await handle.Task;
        if (handle.Status != AsyncOperationStatus.Succeeded)
            throw new Exception($"Addressables 加载失败：{keyOrAssetsPath}");

        // 记录句柄，便于 Release
        _assetHandles[keyOrAssetsPath] = handle;
        return handle.Result;
#else
            throw new NotSupportedException(
                $"未启用 Addressables 且不是 Assets 路径：{keyOrAssetsPath}。请在编辑器使用 Assets/ 路径，" +
                $"或启用 Addressables 并改用 key。");
#endif
        }

        /// <summary>
        /// 异步实例化 Prefab。支持传入 Addressables key 或编辑器下的 Assets 路径。
        /// </summary>
        public static async Task<GameObject> InstantiateAsync(
            string keyOrAssetsPath,
            Transform parent = null,
            Vector3? position = null,
            Quaternion? rotation = null,
            bool worldSpace = false)
        {
            if (string.IsNullOrEmpty(keyOrAssetsPath))
                throw new ArgumentException("keyOrAssetsPath 不能为空。");

#if UNITY_EDITOR
            if (IsAssetsPath(keyOrAssetsPath))
            {
                await Task.Yield(); // 维持异步
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(keyOrAssetsPath);
                if (prefab == null)
                    throw new Exception($"在编辑器中，未找到 Prefab：{keyOrAssetsPath}");

                var pos = position ?? Vector3.zero;
                var rot = rotation ?? Quaternion.identity;

                GameObject instance;
                if (parent == null)
                {
                    instance = UnityEngine.Object.Instantiate(prefab, pos, rot);
                }
                else
                {
                    if (worldSpace)
                        instance = UnityEngine.Object.Instantiate(prefab, pos, rot, parent);
                    else
                    {
                        instance = UnityEngine.Object.Instantiate(prefab, parent);
                        instance.transform.localPosition = pos;
                        instance.transform.localRotation = rot;
                    }
                }
                return instance;
            }
#endif

            // Addressables 实例化（打包或你用key时）
#if ENABLE_ADDRESSABLES || UNITY_ADDRESSABLES
        AsyncOperationHandle<GameObject> handle;
        if (position.HasValue || rotation.HasValue)
        {
            var pos = position ?? Vector3.zero;
            var rot = rotation ?? Quaternion.identity;
            // worldSpace参数由是否将 parentWorldPositionStays 设置为 !worldSpace 来近似处理
            handle = Addressables.InstantiateAsync(keyOrAssetsPath, pos, rot, parent);
        }
        else
        {
            handle = Addressables.InstantiateAsync(keyOrAssetsPath, parent, worldSpace);
        }

        await handle.Task;
        if (handle.Status != AsyncOperationStatus.Succeeded)
            throw new Exception($"Addressables 实例化失败：{keyOrAssetsPath}");

        _instancedByAddressables.Add(handle.Result);
        return handle.Result;
#else
            throw new NotSupportedException(
                $"未启用 Addressables 且不是 Assets 路径：{keyOrAssetsPath}。请在编辑器使用 Assets/ 路径，" +
                $"或启用 Addressables 并改用 key。");
#endif
        }

        /// <summary>
        /// 释放通过 LoadAssetAsync 加载的 Addressables 资源。
        /// 对编辑器路径加载的资产无需释放（忽略）。
        /// </summary>
        public static void Release(string keyOrAssetsPath)
        {
            if (string.IsNullOrEmpty(keyOrAssetsPath)) return;

#if ENABLE_ADDRESSABLES || UNITY_ADDRESSABLES
        if (_assetHandles.TryGetValue(keyOrAssetsPath, out var handle))
        {
            if (handle.IsValid())
                Addressables.Release(handle);
            _assetHandles.Remove(keyOrAssetsPath);
        }
#endif
            // 对于编辑器路径加载的 AssetDatabase 资源，不需要显式释放
        }

        /// <summary>
        /// 释放通过 InstantiateAsync( Addressables ) 实例化出来的对象；编辑器路径实例则直接 Destroy。
        /// </summary>
        public static void ReleaseInstance(GameObject instance)
        {
            if (instance == null) return;

#if ENABLE_ADDRESSABLES || UNITY_ADDRESSABLES
        if (_instancedByAddressables.Contains(instance))
        {
            Addressables.ReleaseInstance(instance);
            _instancedByAddressables.Remove(instance);
            return;
        }
#endif
            // 不是 Addressables 实例（多半是编辑器路径实例）
            UnityEngine.Object.Destroy(instance);
        }

        /// <summary>
        /// 简单判断是否是项目内路径（仅编辑器可用）。
        /// </summary>
        private static bool IsAssetsPath(string s)
            => s.StartsWith("Assets/", StringComparison.Ordinal);

        /// <summary>
        /// 一键卸载：释放所有记录的 Addressables 资源与实例。
        /// </summary>
        public static void ReleaseAll()
        {
#if ENABLE_ADDRESSABLES || UNITY_ADDRESSABLES
        foreach (var kv in _assetHandles)
        {
            var handle = kv.Value;
            if (handle.IsValid())
                Addressables.Release(handle);
        }
        _assetHandles.Clear();

        foreach (var go in _instancedByAddressables)
        {
            if (go) Addressables.ReleaseInstance(go);
        }
        _instancedByAddressables.Clear();
#endif
        }

        /// <summary>
        /// 协程兼容版本：将异步加载资产包装为 IEnumerator，可用于 StartCoroutine。
        /// </summary>
        public static IEnumerator LoadAssetCoroutine<T>(
            string keyOrAssetsPath,
            System.Action<T> onComplete,
            System.Action<Exception> onError = null) where T : UnityEngine.Object
        {
            var task = LoadAssetAsync<T>(keyOrAssetsPath);

            while (!task.IsCompleted)
                yield return null;

            if (task.IsFaulted)
            {
                onError?.Invoke(task.Exception?.GetBaseException());
            }
            else
            {
                onComplete?.Invoke(task.Result);
            }
        }
    }
}
