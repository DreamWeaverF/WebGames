using GameCommon;
using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace GameLaunch
{
    
    public class LaunchController : AMonoBehaviour
    {
        [SerializeField]
        private LaunchType m_launchType;
        [SerializeField]
        private LaunchSettings m_launchSettings;

        private readonly string m_loadingSceneName = "Loading";
        private readonly string m_serverSceneName = "Server";
        protected override void OnInit()
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
            Debug.Log("��ʼDevģʽ");
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_serverSceneName, LoadSceneMode.Additive);
            yield return handle;
            handle = Addressables.LoadSceneAsync(m_loadingSceneName, LoadSceneMode.Additive);
            yield return handle;
        }
        IEnumerator LoadClientScene()
        {
            Debug.Log("��ʼClientģʽ");
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_loadingSceneName);
            yield return handle;
        }
        IEnumerator LoadServerScene()
        {
            Debug.Log("��ʼServerģʽ");
            AsyncOperationHandle<SceneInstance> handle = Addressables.LoadSceneAsync(m_serverSceneName);
            yield return handle;
        }


    }
}
