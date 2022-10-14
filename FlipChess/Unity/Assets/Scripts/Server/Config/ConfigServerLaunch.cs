using GameCommon;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    [Serializable]
    public class ConfigServerLaunchElement : ConfigBaseElement
    {
        public int ListenerPort;
        public string MySqlHost;
        public int MySqlPort;
        public string MySqlUserId;
        public string MySqlPassword;
        public string MySqlSchemaName;
        public string MySqlCharacter;
    }
    [GenConfigClass]
    public class ConfigServerLaunch : ConfigBase<ConfigServerLaunchElement>
    {
        [SerializeField]
        protected LaunchType m_launchType;
        public string MysqlConnection
        {
            get
            {
                if(!TryGetValue((int)m_launchType, out ConfigServerLaunchElement element))
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
                if (!TryGetValue((int)m_launchType, out ConfigServerLaunchElement element))
                {
                    return 0;
                }
                return element.ListenerPort;
            }
        }
    }
}
