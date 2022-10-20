using MessagePack;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace GameCommon
{
    public abstract class AConfigElement
    {
        public int Id;
    }

    [System.Serializable]
    public abstract class AConfig<T1> : ScriptableObject, ISerializationCallbackReceiver where T1 : AConfigElement
    {
        [SerializeField]
        protected List<T1> m_datas = new List<T1>();
        [System.NonSerialized]
        private Dictionary<int, T1> m_elements = new Dictionary<int, T1>();
        public void OnAfterDeserialize()
        {
            InitElementData();
        }
        public void OnBeforeSerialize()
        {
        }
        protected virtual void InitElementData()
        {
            m_elements.Clear();
            for (int index = 0; index < m_datas.Count; index++)
            {
                T1 element = m_datas[index];
                if (element == null)
                {
                    continue;
                }
                m_elements.Add(element.Id, element);
            }
        }
        public bool TryGetValue(int id,out T1 value)
        {
            return m_elements.TryGetValue(id, out value);
        }
    }
}
