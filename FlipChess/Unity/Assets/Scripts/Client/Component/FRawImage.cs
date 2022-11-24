using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameClient
{
    public class FRawImage : RawImage
    {
        [SerializeField]
        private TexturePool m_texturePool;
        [SerializeField]
        private List<string> m_textureAddresss = new List<string>();

        private string m_loadAddress = "";
        private string m_pendingLoadAddress = "";
        void Update()
        {
            UpdateTexture();
        }
        private void UpdateTexture()
        {
            if (m_pendingLoadAddress == "")
            {
                return;
            }
            Texture texture = m_texturePool.Fetch(m_pendingLoadAddress);
            if (texture == null)
            {
                return;
            }
            if (m_loadAddress != "")
            {
                this.texture = null;
                m_texturePool.Recycle(m_loadAddress);
            }
            this.texture = texture;
            m_loadAddress = m_pendingLoadAddress;
            m_pendingLoadAddress = "";
        }
        public void SetTexture(string address,bool bForce = false)
        {
            LoadTexture(address,bForce);
        }
        public void SetTexture(int index, bool bForce = false)
        {
            if (m_textureAddresss.Count <= index)
            {
                Debug.LogWarning($"{this.name} 找不到对应贴图 {index}");
                return;
            }
            LoadTexture(m_textureAddresss[index], bForce);
        }
        private void LoadTexture(string address, bool bForce = false)
        {
            if (bForce && m_loadAddress != "")
            {
                this.texture = null;
                m_texturePool.Recycle(m_loadAddress);
                m_loadAddress = "";
            }
            m_texturePool.LoadAddressable(address).Coroutine();
        }
        public void SetUrlTexture(string url, bool bForce = false)
        {
            if (bForce && m_loadAddress != "")
            {
                this.texture = null;
                m_texturePool.Recycle(m_loadAddress);
                m_loadAddress = "";
            }
            m_texturePool.LoadUrl(url);
        }
        protected override void OnDestroy()
        {
            m_pendingLoadAddress = "";
            if (m_loadAddress != "")
            {
                this.texture = null;
                m_texturePool.Recycle(m_loadAddress);
                m_loadAddress = "";
            }
        }
    }
}
