using GameCommon;
using MySql.Data.MySqlClient;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameServer
{
    [AutoGenSOClass]
    public class DatabaseConnect : ScriptableObject
    {
        [SerializeField]
        private ConfigServerLaunch m_configServerLaunch;
        //最大同时连接数
        private const int maxConnectCount = 100;
        //线程锁
        private object lockObject = new object();

        private Queue<MySqlConnection> mySqlConnectionList = default;

        void OnEnable()
        {
            mySqlConnectionList = new Queue<MySqlConnection>();
            for (int i = 0; i < maxConnectCount; i++)
            {
                try
                {
                    MySqlConnection connection = new MySqlConnection(m_configServerLaunch.MysqlConnection);
                    mySqlConnectionList.Enqueue(connection);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }
        public MySqlConnection Fetch()
        {
            lock (lockObject)
            {
                if (mySqlConnectionList.Count <= 0)
                {
                    return null;
                }
                MySqlConnection _connection = mySqlConnectionList.Dequeue();
                _connection.Open();
                return _connection;
            }
        }
        public void Recycle(MySqlConnection connection)
        {
            lock (lockObject)
            {
                connection.Close();
                mySqlConnectionList.Enqueue(connection);
            }
        }

        public bool CheckConnect()
        {
            lock (lockObject)
            {
                return mySqlConnectionList.Count > 0;
            }
        }

        public void OnBeforeSerialize()
        {
            mySqlConnectionList = new Queue<MySqlConnection>();
            for (int i = 0; i < maxConnectCount; i++)
            {
                try
                {
                    MySqlConnection connection = new MySqlConnection(m_configServerLaunch.MysqlConnection);
                    mySqlConnectionList.Enqueue(connection);
                }
                catch (Exception e)
                {
                    Debug.LogError(e.ToString());
                }
            }
        }
    }
}
