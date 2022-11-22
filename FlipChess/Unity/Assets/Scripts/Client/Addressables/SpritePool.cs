using ET;
using GameCommon;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace GameClient
{
    public struct SpritePoolData
    {
        public Sprite Sprite;
        public int RefCount;
    }

    [GenerateAutoClass]
    public class SpritePool : ScriptableObject
    {
        private Dictionary<string, SpritePoolData> m_sprites = new Dictionary<string, SpritePoolData>();
        private List<string> m_waitRemoves = new List<string>();
        private readonly int m_maxCacheCount = 5;

        public async ETTask Load(string address)
        {
            if (m_sprites.TryGetValue(address, out SpritePoolData value))
            {
                return;
            }
            value = new SpritePoolData();
            value.Sprite = await Addressables.LoadAssetAsync<Sprite>(address).Task;
            m_sprites.Add(address, value);
            m_waitRemoves.Add(address);
        }   

        public Sprite Fetch(string address)
        {
            if (!m_sprites.TryGetValue(address, out SpritePoolData value))
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
            if (!m_sprites.TryGetValue(address, out SpritePoolData value))
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
            if (m_sprites.TryGetValue(m_waitRemoves[0],out value))
            {
                Addressables.Release(value.Sprite);
                value.Sprite = null;
                m_sprites.Remove(address);
            }
            m_waitRemoves.RemoveAt(0);
        }
    }
}
