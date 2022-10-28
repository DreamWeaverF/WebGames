using ET;
using GameCommon;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    public class DatabaseController : AMonoBehaviour
    {
        [SerializeField]
        private ConfigServerLaunch m_configServerLaunch;
        [SerializeField]
        private List<ADatabase> m_databases;

        private const int m_maxConnectCount = 100;
        private Queue<MySqlConnection> m_connections = default;
        private List<ETTask<MySqlConnection>> m_reqConnectTasks = new List<ETTask<MySqlConnection>>();
        protected override void OnInit()
        {
            m_connections = new Queue<MySqlConnection>();
            for (int i = 0; i < m_maxConnectCount; i++)
            {
                MySqlConnection connection = new MySqlConnection(m_configServerLaunch.MysqlConnection);
                m_connections.Enqueue(connection);
            }
            for(int i = 0; i < m_databases.Count; i++)
            {
                m_databases[i].Init();
            }
        }
        protected override void UnInit()
        {
            for (int i = 0; i < m_databases.Count; i++)
            {
                m_databases[i].UnInit();
            }
        }
        void Update()
        {
            if (m_connections.Count <= 0)
            {
                return;
            }
            if(m_reqConnectTasks.Count <= 0)
            {
                return;
            }
            m_reqConnectTasks[0].SetResult(m_connections.Dequeue());
        }
        [SynchronizeMethod(SyncName = SyncName.FetchMysqlConnection)]
        private ETTask<MySqlConnection> OnCallbackFetchMysqlConnection()
        {
            ETTask<MySqlConnection> connectionTask = ETTask<MySqlConnection>.Create(true);
            m_reqConnectTasks.Add(connectionTask);
            return connectionTask;
        }
        [SynchronizeMethod(SyncName = SyncName.RecycleMysqlConnection)]
        private void OnCallbackRecycleMysqlConnection(MySqlConnection connection)
        {
            connection.Close();
            m_connections.Enqueue(connection);
        }
    }
}
