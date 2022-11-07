using GameCommon;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

namespace GameLaunch
{
    
    public class LaunchController : MonoBehaviour
    {
        [SerializeField]
        private LaunchType m_launchType;
        [SerializeField]
        private LaunchSettings m_launchSettings;

        private readonly string m_clientSceneName = "Loading";
        private readonly string m_serverSceneName = "Server";

        void Start()
        {
            string methodName = "";
            switch (m_launchType)
            {
                case LaunchType.Client:
                    methodName = nameof(LoadClientScene);
                    break;
                case LaunchType.Server:
                    methodName = nameof(LoadServerScene);
                    break;
                case LaunchType.Dev:
                    methodName = nameof(LoadDevScene);
                    break;
            }
            StartCoroutine(methodName);
        }
        IEnumerator LoadDevScene()
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_serverSceneName, LoadSceneMode.Additive);
            yield return handle;
            handle = Addressables.LoadSceneAsync(m_clientSceneName, LoadSceneMode.Additive);
            yield return handle;
        }
        IEnumerator LoadClientScene()
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_clientSceneName);
            yield return handle;
        }
        IEnumerator LoadServerScene()
        {
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_serverSceneName);
            yield return handle;
        }
    }
}
