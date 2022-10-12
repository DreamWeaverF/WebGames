using MessagePack;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    [MessagePackObject]
    public abstract class ConfigBaseElement
    {
        [Key(1)]
        public int Id { get; set; }
    }

    [System.Serializable]
    public abstract class ConfigBase<T1> : ScriptableObject, ISerializationCallbackReceiver where T1 : ConfigBaseElement
    {
        public List<T1> m_datas = new List<T1>();
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
