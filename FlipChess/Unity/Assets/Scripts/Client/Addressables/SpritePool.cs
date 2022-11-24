using ET;
using GameCommon;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameClient
{
    struct SpritePoolData
    {
        public Sprite Sprite;
        public int RefCount;
    }
    [GenerateAutoClass]
    public class SpritePool : ScriptableObject
    {
        private Dictionary<string, SpritePoolData> m_spriteDatas = new Dictionary<string, SpritePoolData>();
        private List<string> m_waitRemoves = new List<string>();
        private readonly int m_maxCacheCount = 5;
        public async ETTask Load(string address)
        {
            if (m_spriteDatas.TryGetValue(address, out SpritePoolData value))
            {
                return;
            }
            value = new SpritePoolData();
            value.Sprite = await Addressables.LoadAssetAsync<Sprite>(address).Task;
            m_spriteDatas.Add(address, value);
            m_waitRemoves.Add(address);
        }   
        public Sprite Fetch(string address)
        {
            if (!m_spriteDatas.TryGetValue(address, out SpritePoolData value))
            {
                return null;
            }
            value.RefCount += 1;
            if (m_waitRemoves.Contains(address))
            {
                m_waitRemoves.Remove(address);
            }
            return value.Sprite;
        }
        public void Recycle(string address)
        {
            if (!m_spriteDatas.TryGetValue(address, out SpritePoolData value))
            {
                return;
            }
            value.RefCount -= 1;
            if(value.RefCount > 0)
            {
                return;
            }
            m_waitRemoves.Add(address);
            if(m_waitRemoves.Count < m_maxCacheCount)
            {
                return;
            }
            if (m_spriteDatas.TryGetValue(m_waitRemoves[0],out value))
            {
                Addressables.Release(value.Sprite);
                value.Sprite = null;
                m_spriteDatas.Remove(address);
            }
            m_waitRemoves.RemoveAt(0);
        }
    }
}
