using GameCommon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    [Serializable]
    public class ConfigServerLaunchElement : AConfigElement
    {
        public LaunchType LaunchType;
        public int ListenerPort;
        public string MySqlHost;
        public int MySqlPort;
        public string MySqlUserId;
        public string MySqlPassword;
        public string MySqlSchemaName;
        public string MySqlCharacter;
    }
    [GenerateAutoClass]
    public class ConfigServerLaunch : AConfig<ConfigServerLaunchElement>
    {
        [SerializeField]
        protected LaunchSettings m_launchSettings;

        private Dictionary<LaunchType, ConfigServerLaunchElement> m_keyValuePairs = new Dictionary<LaunchType, ConfigServerLaunchElement>();
        public string MysqlConnection
        {
            get
            {
                if(!TryGetValue(m_launchSettings.LaunchType, out ConfigServerLaunchElement element))
                {
                    return "";
                }
                string connectionInfo = $"host={element.MySqlHost};port={element.MySqlPort};user id={element.MySqlUserId};password={element.MySqlPassword};database={element.MySqlSchemaName}; character set={element.MySqlCharacter}; ConnectionLifeTime=10; SslMode=none;";
                return connectionInfo;

            }
        }
        public int ListenerPort
        {
            get
            {
                if (!TryGetValue(m_launchSettings.LaunchType, out ConfigServerLaunchElement element))
                {
                    return 0;
                }
                return element.ListenerPort;
            }
        }
        protected override void InitElementData()
        {
            m_keyValuePairs.Clear();
            for (int index = 0; index < m_datas.Count; index++)
            {
                ConfigServerLaunchElement element = m_datas[index];
                if (element == null)
                {
                    continue;
                }
                m_keyValuePairs.Add(element.LaunchType, element);
            }
        }
        public bool TryGetValue(LaunchType type, out ConfigServerLaunchElement value)
        {
            return m_keyValuePairs.TryGetValue(type, out value);
        }
    }
}
