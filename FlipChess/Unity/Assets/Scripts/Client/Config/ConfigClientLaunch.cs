using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameClient
{
    [Serializable]
    public class ConfigClientLaunchElement : AConfigElement
    {
        public LaunchType LaunchType;
        public string ServerAddress;
    }
    [GenerateAutoClass]
    public class ConfigClientLaunch : AConfig<ConfigClientLaunchElement>
    {
        [SerializeField]
        protected LaunchSettings m_launchSettings;

        private Dictionary<LaunchType,ConfigClientLaunchElement> m_keyValuePairs = new Dictionary<LaunchType,ConfigClientLaunchElement>();
        public string ServerAddress
        {
            get 
            {
                if (!TryGetValue(m_launchSettings.LaunchType, out ConfigClientLaunchElement element))
                {
                    return "";
                }
                return element.ServerAddress;
            }
        }
        protected override void InitElementData()
        {
            m_keyValuePairs.Clear();
            for (int index = 0; index < m_datas.Count; index++)
            {
                ConfigClientLaunchElement element = m_datas[index];
                if (element == null)
                {
                    continue;
                }
                m_keyValuePairs.Add(element.LaunchType, element);
            }
        }
        public bool TryGetValue(LaunchType type, out ConfigClientLaunchElement value)
        {
            return m_keyValuePairs.TryGetValue(type, out value);
        }
    }
}
