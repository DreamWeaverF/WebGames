using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    [Serializable]
    public class SerializationDictionary<TKey, TValue> : Dictionary<TKey,TValue>, ISerializationCallbackReceiver
    {
        [SerializeField,HideInInspector]
        private List<TKey> m_keys;
        [SerializeField]
        private List<TValue> m_values;

        public SerializationDictionary(List<TKey> keys,List<TValue> values)
        {
            m_keys = keys;
            m_values = values;
        }
        public void OnBeforeSerialize()
        {

        }
        public void OnAfterDeserialize()
        {
            this.Clear();
            if(m_keys == null || m_values == null)
            {
                return;
            }
            int count = Math.Min(m_keys.Count, m_values.Count);
            for (var i = 0; i < count; ++i)
            {
                this.Add(m_keys[i], m_values[i]);
            }
        }
    }
}
