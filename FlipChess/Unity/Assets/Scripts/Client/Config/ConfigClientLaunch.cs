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
        public string ServerAddress;
    }
    [GenConfigClass]
    public class ConfigClientLaunch : AConfig<ConfigClientLaunchElement>
    {
        [SerializeField]
        protected LaunchType m_launchType;
        public string ServerAddress
        {
            get 
            {
                if (!TryGetValue((int)m_launchType, out ConfigClientLaunchElement element))
                {
                    return "";
                }
                return element.ServerAddress;
            }
        }
    }
}
