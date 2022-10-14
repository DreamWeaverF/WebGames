using GameCommon;
using MessagePack;
using System;
using System.Xml.Linq;
using UnityEngine;

namespace GameServer
{
    [MessagePackObject]
    public class ConfigServerLaunchElement : ConfigBaseElement
    {
        [Key(2)]
        public int ListenerPort { get; set; }
        [Key(3)]
        public string MySqlHost { get; set; }
        [Key(4)]
        public int MySqlPort { get; set; }
        [Key(5)]
        public string MySqlUserId { get; set; }
        [Key(6)]
        public string MySqlPassword { get; set; }
        [Key(7)]
        public string MySqlSchemaName { get; set; }
        [Key(7)]
        public string MySqlCharacter { get; set; }
    }

    public class ConfigServerLaunch : ConfigBase<ConfigServerLaunchElement>
    {
        [SerializeField]
        public LaunchType m_launchType;
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
