using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Dreamwear
{
    public class Launch : MonoBehaviourEx
    {
        private readonly string m_sceneName = "Loading";

        void Awake()
        {
            StartCoroutine("LoadScene");
        }

        IEnumerator LoadScene()
        {
            AsyncOperation asy = SceneManager.LoadSceneAsync(m_sceneName);
            asy.allowSceneActivation = true;
            yield return asy;

        }
    }
}
