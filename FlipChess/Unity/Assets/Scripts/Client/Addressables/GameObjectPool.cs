using GameCommon;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameClient
{
    [GenerateAutoClass]
    public class GameObjectPool : ScriptableObject
    {
        private Dictionary<string, Queue<GameObject>> m_gameobjects = new Dictionary<string, Queue<GameObject>>();
        private List<string> m_waitRemoves = new List<string>();
        private readonly int m_maxCacheCount = 10;

        public async Task<GameObject> Fetch(string address, Transform parent, Vector3 position, Quaternion rotation, Vector3 scale)
        {
            if (!m_gameobjects.TryGetValue(address, out Queue<GameObject> value))
            {
                value = new Queue<GameObject>();
                m_gameobjects.Add(address, value);
            }
            GameObject go;
            if (value.Count > 0)
            {
                go = value.Dequeue();
                go.transform.SetParent(parent, false);
            }
            else
            {
                go = await Addressables.InstantiateAsync(address, parent, false).Task;
                go.name = address;
            }
            if(go == null)
            {
                Debug.LogError($"找不到资源 {address}");
                return null;
            }
            go.transform.localPosition = position;
            go.transform.localRotation = rotation;
            go.transform.localScale = scale;
            go.SetActive(true);
            if (m_waitRemoves.Contains(address))
            {
                m_waitRemoves.Remove(address);
            }
            return go;
        }
        public void Recycle(GameObject go)
        {
            if (go == null)
            {
                return;
            }
            if (!m_gameobjects.TryGetValue(go.name, out Queue<GameObject> value))
            {
                return;
            }
            go.SetActive(false);
            go.transform.SetParent(null, false);
            value.Enqueue(go);
            m_waitRemoves.Add(go.name);
            if(m_waitRemoves.Count < m_maxCacheCount)
            {
                return;
            }
            if (m_gameobjects.TryGetValue(m_waitRemoves[0], out value))
            {
                if(value.Count > 0)
                {
                    go = value.Dequeue();
                    Addressables.ReleaseInstance(go);
                }
                if(value.Count <= 0)
                {
                    m_gameobjects.Remove(m_waitRemoves[0]);
                }
            }
            m_waitRemoves.RemoveAt(0);
        }
    }
}
