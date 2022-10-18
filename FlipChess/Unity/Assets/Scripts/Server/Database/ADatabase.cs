using Codice.Client.Common;
using Mono.Cecil.Mdb;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    public abstract class ADatabaseElement
    {

    }

    public abstract class ADatabase<TElement,TKey> : ScriptableObject where TElement : ADatabaseElement
    {
        [SerializeField]
        private DatabaseConnect m_connectPool;

        private string m_tableName;
        private FieldInfo[] m_fields;
        private MethodInfo m_getFieldValueAsyncMethod;

        void OnEnable()
        {
            if (Application.isPlaying)
            {
                m_tableName = GetType().Name;
                Type typeElement = typeof(TElement);
                m_fields = typeElement.GetFields();

                Type type = this.GetType();
                m_getFieldValueAsyncMethod = type.GetMethod(nameof(GetFieldValueAsync), BindingFlags.NonPublic | BindingFlags.Instance);
            }
        }
        void OnDisable()
        {
            if (Application.isPlaying)
            {
                m_fields = null;
                m_getFieldValueAsyncMethod = null;
            }
        }
        public async Task<long> TrySelectTableCount()
        {
            try
            {
                MySqlConnection connection = await TryGetMySqlConnection();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 60;
                cmd.SelectTableCountCmd(m_tableName);
                using (DbDataReader dbReader = await cmd.ExecuteReaderAsync())
                {
                    if (await dbReader.ReadAsync())
                    {
                        long count = await dbReader.GetFieldValueAsync<long>(0);
                        m_connectPool.Recycle(connection);
                        return count;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return 0;
        }
        public async Task<long> TrySelectCount(TKey keyValue)
        {
            try
            {
                MySqlConnection connection = await TryGetMySqlConnection();
                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandTimeout = 60;
                cmd.SelectCountCmd(m_tableName, keyValue, m_fields[0].Name);
                using (DbDataReader dbReader = await cmd.ExecuteReaderAsync())
                {
                    if (await dbReader.ReadAsync())
                    {
                        long _count = await dbReader.GetFieldValueAsync<long>(0);
                        m_connectPool.Recycle(connection);
                        return _count;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return 0;
        }
        public async Task<bool> TrySelect(TElement element, TKey keyValue)
        {
            try
            {
                MySqlConnection connection = await TryGetMySqlConnection();
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
                using (DbDataReader dbReader = await cmd.ExecuteReaderAsync())
                {
                    if (await dbReader.ReadAsync())
                    {
                        for (int i = 0; i < m_fields.Length; i++)
                        {
                            MethodInfo userMethod = m_getFieldValueAsyncMethod.MakeGenericMethod(m_fields[i].FieldType);
                            object dbValue = userMethod.Invoke(this, new object[] { dbReader, i });
                            m_fields[i].SetValue(element, dbValue);
                        }
                        m_connectPool.Recycle(connection);
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return false;
        }
        public async Task<bool> TryInsertInto(TElement element)
        {
            try
            {
                MySqlConnection connection = await TryGetMySqlConnection();
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
                m_connectPool.Recycle(connection);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return false;
        }
        public async Task<bool> TryUpdate(TElement element)
        {
            try
            {
                MySqlConnection connection = await TryGetMySqlConnection();
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
                m_connectPool.Recycle(connection);
                return true;
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
            return false;
        }
        protected async Task<T> GetFieldValueAsync<T>(DbDataReader dbReader, int index)
        {
            return await dbReader.GetFieldValueAsync<T>(index);
        }
        private async Task<MySqlConnection> TryGetMySqlConnection()
        {
            while (!m_connectPool.CheckConnect())
            {
                await Task.Delay(1000);
            }
            return m_connectPool.Fetch();
        }

    }
}
