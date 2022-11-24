using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;

namespace GameClient
{
    public class FImage : Image
    {
        [SerializeField]
        private SpritePool m_spritePool;
        [SerializeField]
        private List<string> m_spriteAddresss = new List<string>();

        private string m_loadAddress = "";
        private string m_pendingLoadAddress = "";
        void Update()
        {
            UpdateSprite();
        }
        private void UpdateSprite()
        {
            if (m_pendingLoadAddress == "")
            {
                return;
            }
            Sprite sprite = m_spritePool.Fetch(m_pendingLoadAddress);
            if(sprite == null)
            {
                return;
            }
            if (m_loadAddress != "")
            {
                this.sprite = null;
                m_spritePool.Recycle(m_loadAddress);
            }
            this.sprite = sprite;
            m_loadAddress = m_pendingLoadAddress;
            m_pendingLoadAddress = "";
        }
        public void SetSprite(string address,bool bForce = false)
        {
            LoadSprite(address, bForce);
        }
        public void SetSprite(int index, bool bForce = false)
        {
            if (m_spriteAddresss.Count <= index)
            {
                Debug.LogWarning($"{this.name} 找不到对应贴图 {index}");
                return;
            }
            LoadSprite(m_spriteAddresss[index], bForce);
        }
        private void LoadSprite(string address, bool bForce)
        {
            if (bForce && m_loadAddress != "")
            {
                this.sprite = null;
                m_spritePool.Recycle(m_loadAddress);
                m_loadAddress = "";
            }
            m_pendingLoadAddress = address;
            m_spritePool.Load(m_pendingLoadAddress).Coroutine();
        }
        protected override void OnDestroy()
        {
            m_pendingLoadAddress = "";
            if (m_loadAddress != "")
            {
                this.sprite = null;
                m_spritePool.Recycle(m_loadAddress);
                m_loadAddress = "";
            }
        }
    }
}
