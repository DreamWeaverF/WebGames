using ET;
using GameCommon;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Networking;

namespace GameClient
{
    enum LoadType
    {
        None,
        Addressable,
        Url,
    }
    struct TexturePoolData
    {
        public Texture Texture;
        public int RefCount;
        public LoadType LoadType;
    }
    [GenerateAutoClass]
    public class TexturePool : ScriptableObject
    {
        private Dictionary<string, TexturePoolData> m_textureDatas = new Dictionary<string, TexturePoolData>();
        private List<string> m_waitRemoves = new List<string>();
        private readonly int m_maxCacheCount = 5;

        public async ETTask LoadAddressable(string address)
        {
            if (m_textureDatas.TryGetValue(address, out TexturePoolData value))
            {
                return;
            }
            value = new TexturePoolData();
            value.Texture = await Addressables.LoadAssetAsync<Texture>(address).Task;
            value.LoadType = LoadType.Addressable;
            m_textureDatas.Add(address, value);
            m_waitRemoves.Add(address);
        }

        public IEnumerator LoadUrl(string address)
        {
            if (m_textureDatas.TryGetValue(address, out TexturePoolData value))
            {
                yield break;
            }
            using(UnityWebRequest webRequest = new UnityWebRequest(address, "GET", new DownloadHandlerTexture(), null))
            {
                yield return webRequest.SendWebRequest();
                if (!string.IsNullOrEmpty(webRequest.error))
                {
                    Debug.LogError(webRequest.error);
                    yield break;
                }
                value = new TexturePoolData();
                value.Texture = (webRequest.downloadHandler as DownloadHandlerTexture).texture;
                value.LoadType = LoadType.Url;
                m_textureDatas.Add(address, value);
                m_waitRemoves.Add(address);
            }
        }
        public Texture Fetch(string address)
        {
            if (!m_textureDatas.TryGetValue(address, out TexturePoolData value))
            {
                return null;
            }
            value.RefCount += 1;
            if (m_waitRemoves.Contains(address))
            {
                m_waitRemoves.Remove(address);
            }
            return value.Texture;
        }
        public void Recycle(string address)
        {
            if (!m_textureDatas.TryGetValue(address, out TexturePoolData value))
            {
                return;
            }
            value.RefCount -= 1;
            if (value.RefCount > 0)
            {
                return;
            }
            m_waitRemoves.Add(address);
            if (m_waitRemoves.Count < m_maxCacheCount)
            {
                return;
            }
            if (m_textureDatas.TryGetValue(m_waitRemoves[0], out value))
            {
                switch (value.LoadType)
                {
                    case LoadType.Url:
                        Destroy(value.Texture);
                        break;
                    case LoadType.Addressable:
                        Addressables.Release(value.Texture);
                        break;
                }
                value.Texture = null;
                m_textureDatas.Remove(address);
            }
            m_waitRemoves.RemoveAt(0);
        }
    }
}
