using System.Collections.Generic;
using UnityEngine;

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
        private Dictionary<int, T1> m_keyValuePairs = new Dictionary<int, T1>();
        public void OnAfterDeserialize()
        {
            InitElementData();
        }
        public void OnBeforeSerialize()
        {
        }
        protected virtual void InitElementData()
        {
            m_keyValuePairs.Clear();
            for (int index = 0; index < m_datas.Count; index++)
            {
                T1 element = m_datas[index];
                if (element == null)
                {
                    continue;
                }
                m_keyValuePairs.Add(element.Id, element);
            }
        }
        public bool TryGetValue(int id,out T1 value)
        {
            return m_keyValuePairs.TryGetValue(id, out value);
        }
    }
}
