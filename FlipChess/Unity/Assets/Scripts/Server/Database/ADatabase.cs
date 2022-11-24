using ET;
using GameCommon;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
#if !UNITY_WEBGL
using System.Data.SqlClient;
#endif
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    public abstract class ADatabase : ScriptableObject
    {
        public abstract void Init();
        public abstract void UnInit();
    }
    public abstract class ADatabaseElement
    {

    }
    public abstract class ADatabase<TElement,TKey> : ADatabase where TElement : ADatabaseElement,new()
    {
        private string m_tableName;
        private FieldInfo[] m_fields;
        private MethodInfo m_getFieldValueMethod;
        public override void Init()
        {
            m_tableName = GetType().Name;
            Type typeElement = typeof(TElement);
            m_fields = typeElement.GetFields();

            Type type = this.GetType();
            m_getFieldValueMethod = type.GetMethod(nameof(GetFieldValue), BindingFlags.NonPublic | BindingFlags.Instance);
        }
        public override void UnInit()
        {
            m_fields = null;
            m_getFieldValueMethod = null;
        }
        public async Task<long> TrySelectTableCount()
        {
            MySqlConnection connection = await SyncName.FetchMysqlConnection.BroadcastSyncEvent<ETTask<MySqlConnection>>();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 60;
                cmd.SelectTableCountCmd(m_tableName);
                DbDataReader dbReader = await cmd.ExecuteReaderAsync();
                long count = 0;
                if(await dbReader.ReadAsync())
                {
                    count = await dbReader.GetFieldValueAsync<long>(0);
                }
                await dbReader.CloseAsync();
                SyncName.RecycleMysqlConnection.BroadcastSyncEvent(connection);
                return count;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                SyncName.RecycleMysqlConnection.BroadcastSyncEvent(connection);
                return 0;
            }
        }
        public async Task<long> TrySelectCount(TKey keyValue)
        {
            MySqlConnection connection = await SyncName.FetchMysqlConnection.BroadcastSyncEvent<ETTask<MySqlConnection>>();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 60;
                cmd.SelectCountCmd(m_tableName, keyValue, m_fields[0].Name);
                DbDataReader dbReader = await cmd.ExecuteReaderAsync();
                long count = 0;
                if (await dbReader.ReadAsync())
                {
                    count = await dbReader.GetFieldValueAsync<long>(0);
                }
                await dbReader.CloseAsync();
                SyncName.RecycleMysqlConnection.BroadcastSyncEvent(connection);
                return count;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                SyncName.RecycleMysqlConnection.BroadcastSyncEvent(connection);
                return 0;
            }
        }
        public async Task<bool> TrySelect(TElement element, TKey keyValue)
        {
            MySqlConnection connection = await SyncName.FetchMysqlConnection.BroadcastSyncEvent<ETTask<MySqlConnection>>();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 60;
                switch (m_fields.Length)
                {
                    case 2:
                        cmd.SelectCmd(m_tableName, keyValue, m_fields[0].Name, m_fields[1].Name);
                        break;
                    case 3:
                        cmd.SelectCmd(m_tableName, keyValue, m_fields[0].Name, m_fields[1].Name, m_fields[2].Name);
                        break;
                    case 4:
                        cmd.SelectCmd(m_tableName, keyValue, m_fields[0].Name, m_fields[1].Name, m_fields[2].Name, m_fields[3].Name);
                        break;
                }
                DbDataReader dbReader = await cmd.ExecuteReaderAsync();
                if (await dbReader.ReadAsync())
                {
                    for (int i = 0; i < m_fields.Length; i++)
                    {
                        MethodInfo userMethod = m_getFieldValueMethod.MakeGenericMethod(m_fields[i].FieldType);
                        object dbValue = userMethod.Invoke(this, new object[] { dbReader, i });
                        m_fields[i].SetValue(element, dbValue);
                    }
                }
                await dbReader.CloseAsync();
                SyncName.RecycleMysqlConnection.BroadcastSyncEvent(connection);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                SyncName.RecycleMysqlConnection.BroadcastSyncEvent(connection);
                return false;
            }
        }
        public async Task<bool> TryInsertInto(TElement element)
        {
            MySqlConnection connection = await SyncName.FetchMysqlConnection.BroadcastSyncEvent<ETTask<MySqlConnection>>();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 60;
                switch (m_fields.Length)
                {
                    case 2:
                        {
                            object value0 = m_fields[0].GetValue(element);
                            object value1 = m_fields[1].GetValue(element);
                            cmd.InsertIntoCmd(m_tableName, value0, m_fields[0].Name, value1, m_fields[1].Name);
                        }
                        break;
                    case 3:
                        {
                            object value0 = m_fields[0].GetValue(element);
                            object value1 = m_fields[1].GetValue(element);
                            object value2 = m_fields[2].GetValue(element);
                            cmd.InsertIntoCmd(m_tableName, value0, m_fields[0].Name, value1, m_fields[1].Name, value2, m_fields[2].Name);
                        }
                        break;
                    case 4:
                        {
                            object value0 = m_fields[0].GetValue(element);
                            object value1 = m_fields[1].GetValue(element);
                            object value2 = m_fields[2].GetValue(element);
                            object value3 = m_fields[3].GetValue(element);
                            cmd.InsertIntoCmd(m_tableName, value0, m_fields[0].Name, value1, m_fields[1].Name, value2, m_fields[2].Name, value3, m_fields[3].Name);
                        }
                        break;
                }
                await cmd.ExecuteNonQueryAsync();
                SyncName.RecycleMysqlConnection.BroadcastSyncEvent(connection);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                SyncName.RecycleMysqlConnection.BroadcastSyncEvent(connection);
            }
            return false;
        }
        public async Task<bool> TryUpdate(TElement element)
        {
            MySqlConnection connection = await SyncName.FetchMysqlConnection.BroadcastSyncEvent<ETTask<MySqlConnection>>();
            try
            {
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 60;
                switch (m_fields.Length)
                {
                    case 2:
                        {
                            object value0 = m_fields[0].GetValue(element);
                            object value1 = m_fields[1].GetValue(element);
                            cmd.UpdateCmd(m_tableName, value0, m_fields[0].Name, value1, m_fields[1].Name);
                        }
                        break;
                    case 3:
                        {
                            object value0 = m_fields[0].GetValue(element);
                            object value1 = m_fields[1].GetValue(element);
                            object value2 = m_fields[2].GetValue(element);
                            cmd.UpdateCmd(m_tableName, value0, m_fields[0].Name, value1, m_fields[1].Name, value2, m_fields[2].Name);
                        }
                        break;
                    case 4:
                        {
                            object value0 = m_fields[0].GetValue(element);
                            object value1 = m_fields[1].GetValue(element);
                            object value2 = m_fields[2].GetValue(element);
                            object value3 = m_fields[3].GetValue(element);
                            cmd.UpdateCmd(m_tableName, value0, m_fields[0].Name, value1, m_fields[1].Name, value2, m_fields[2].Name, value3, m_fields[3].Name);
                        }
                        break;
                }
                await cmd.ExecuteNonQueryAsync();
                SyncName.RecycleMysqlConnection.BroadcastSyncEvent(connection);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
                SyncName.RecycleMysqlConnection.BroadcastSyncEvent(connection);
            }
            return false;
        }
        protected T GetFieldValue<T>(DbDataReader dbReader, int index)
        {
            return dbReader.GetFieldValue<T>(index);
        }
    }
}
