using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

namespace GameClient
{
    public class SceneController : AMonoBehaviour
    {
        [SynchronizeMethod(SyncName = SyncName.LoadScene)]
        private void LoadScene(string sceneName)
        {
            StartCoroutine("LoadSceneCoroutine", sceneName);
        }
        IEnumerator LoadSceneCoroutine(string sceneName)
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(sceneName);
            yield return handle;
        }
    }
}
