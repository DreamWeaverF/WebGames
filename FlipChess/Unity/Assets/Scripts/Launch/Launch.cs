using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameLaunch
{
    public class Launch : MonoBehaviour
    {
        private readonly string m_loadingSceneName = "Loading";
        private readonly string m_serverSceneName = "Server";

        void Awake()
        {
            StartCoroutine("LoadScene");
        }

        IEnumerator LoadScene()
        {
            AsyncOperation asy = SceneManager.LoadSceneAsync(m_serverSceneName, LoadSceneMode.Additive);
            yield return asy;
            yield return 1000;
            asy = SceneManager.LoadSceneAsync(m_loadingSceneName, LoadSceneMode.Additive);
            yield return asy;
            asy = SceneManager.UnloadSceneAsync(this.gameObject.name);
            yield return asy;

        }
    }
}
