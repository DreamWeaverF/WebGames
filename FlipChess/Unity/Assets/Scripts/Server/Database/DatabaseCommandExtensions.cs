using MessagePack;
using MySql.Data.MySqlClient;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace GameServer
{
    public static class DatabaseCommandExtensions
    {
        private static MySqlDbType CoverTypeToDbType<T1>(T1 value1)
        {
            MySqlDbType _dbType;
            switch (value1.GetType().Name)
            {
                case "String":
                    _dbType = MySqlDbType.VarChar;
                    break;
                case "Int32":
                    _dbType = MySqlDbType.Int32;
                    break;
                case "Int64":
                    _dbType = MySqlDbType.Int64;
                    break;
                case "Byte[]":
                    _dbType = MySqlDbType.Blob;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return _dbType;
        }
        public static void SelectTableCountCmd(this MySqlCommand cmd, string tableName)
        {
            cmd.CommandText = $"SELECT COUNT(1) FROM {tableName};";
        }
        public static void SelectCountCmd<T1>(this MySqlCommand cmd, string tableName, T1 keyValue, string keyName)
        {
            cmd.CommandText = $"SELECT COUNT({keyName}) AS {keyName} FROM {tableName} WHERE {keyName}= @KeyValue;";
            cmd.Parameters.Add("@KeyValue", CoverTypeToDbType(keyValue)).Value = keyValue;
        }

        public static void SelectCmd<T1>(this MySqlCommand cmd, string tableName, T1 keyValue, string keyName1, string keyName2)
        {
            cmd.CommandText = $"SELECT {keyName1},{keyName2} FROM {tableName} WHERE {keyName1}= @KeyValue;";
            cmd.Parameters.Add("@KeyValue", CoverTypeToDbType(keyValue)).Value = keyValue;
        }
        public static void SelectCmd<T1>(this MySqlCommand cmd, string tableName, T1 keyValue, string keyName1, string keyName2,string keyName3)
        {
            cmd.CommandText = $"SELECT {keyName1},{keyName2},{keyName3} FROM {tableName} WHERE {keyName1}= @KeyValue;";
            cmd.Parameters.Add("@KeyValue", CoverTypeToDbType(keyValue)).Value = keyValue;
        }
        public static void SelectCmd<T1>(this MySqlCommand cmd, string tableName, T1 keyValue, string keyName1, string keyName2, string keyName3, string keyName4)
        {
            cmd.CommandText = $"SELECT {keyName1},{keyName2},{keyName3},{keyName4} FROM {tableName} WHERE {keyName1}= @KeyValue;";
            cmd.Parameters.Add("@KeyValue", CoverTypeToDbType(keyValue)).Value = keyValue;
        }
        public static void SelectCmdLimitRange<T1>(this MySqlCommand cmd, string tableName, T1 keyMinValue, T1 keyMaxValue, string keyName1, string keyName2, string limitKeyName)
        {
            cmd.CommandText = $"SELECT {keyName1},{keyName2} FROM {tableName} WHERE {limitKeyName}> @KeyMinValue and {limitKeyName}< @KeyMaxValue;";
            cmd.Parameters.Add("@KeyMinValue", CoverTypeToDbType(keyMinValue)).Value = keyMinValue;
            cmd.Parameters.Add("@KeyMaxValue", CoverTypeToDbType(keyMaxValue)).Value = keyMaxValue;
        }
        public static void SelectCmdLimitRange<T1>(this MySqlCommand cmd,string tableName,T1 keyMinValue, T1 keyMaxValue, string keyName1, string keyName2, string keyName3, string limitKeyName)
        {
            cmd.CommandText = $"SELECT {keyName1},{keyName2},{keyName3} FROM {tableName} WHERE {limitKeyName}> @KeyMinValue and {limitKeyName} < @KeyMaxValue;";
            cmd.Parameters.Add("@KeyMinValue", CoverTypeToDbType(keyMinValue)).Value = keyMinValue;
            cmd.Parameters.Add("@KeyMaxValue", CoverTypeToDbType(keyMaxValue)).Value = keyMaxValue;
        }

        public static void SelectCmdLimitRange<T1>(this MySqlCommand cmd, string tableName, T1 keyMinValue, T1 keyMaxValue, string keyName1, string keyName2, string keyName3, string keyName4, string limitKeyName)
        {
            cmd.CommandText = $"SELECT {keyName1},{keyName2},{keyName3},{keyName4} FROM {tableName} where {limitKeyName}> @KeyMinValue and {limitKeyName}< @KeyMaxValue;";
            cmd.Parameters.Add("@KeyMinValue", CoverTypeToDbType(keyMinValue)).Value = keyMinValue;
            cmd.Parameters.Add("@KeyMaxValue", CoverTypeToDbType(keyMaxValue)).Value = keyMaxValue;
        }

        public static void InsertIntoCmd<T1,T2>(this MySqlCommand cmd, string tableName, T1 keyValue1, string keyName1, T2 keyValue2, string keyName2)
        {
            cmd.CommandText = $"INSERT INTO {tableName} ({keyName1},{keyName2}) VALUES (@{keyName1}, @{keyName2});";
            cmd.Parameters.Add($"@{keyName1}", CoverTypeToDbType(keyValue1)).Value = keyValue1;
            cmd.Parameters.Add($"@{keyName2}", CoverTypeToDbType(keyValue2)).Value = keyValue2;
        }
        public static void InsertIntoCmd<T1, T2, T3>(this MySqlCommand cmd, string tableName, T1 keyValue1, string keyName1, T2 keyValue2, string keyName2, T3 keyValue3, string keyName3)
        {
            cmd.CommandText = $"INSERT INTO {tableName} ({keyName1},{keyName2},{keyName3}) VALUES (@{keyName1}, @{keyName2},@{keyName3});";
            cmd.Parameters.Add($"@{keyName1}", CoverTypeToDbType(keyValue1)).Value = keyValue1;
            cmd.Parameters.Add($"@{keyName2}", CoverTypeToDbType(keyValue2)).Value = keyValue2;
            cmd.Parameters.Add($"@{keyName3}", CoverTypeToDbType(keyValue3)).Value = keyValue3;
        }
        public static void InsertIntoCmd<T1, T2, T3, T4>(this MySqlCommand cmd, string tableName, T1 keyValue1, string keyName1, T2 keyValue2, string keyName2, T3 keyValue3, string keyName3, T4 keyValue4, string keyName4)
        {
            cmd.CommandText = $"INSERT INTO {tableName} ({keyName1},{keyName2},{keyName3},{keyName4}) VALUES (@{keyName1}, @{keyName2},@{keyName3},@{keyName4});";
            cmd.Parameters.Add($"@{keyName1}", CoverTypeToDbType(keyValue1)).Value = keyValue1;
            cmd.Parameters.Add($"@{keyName2}", CoverTypeToDbType(keyValue2)).Value = keyValue2;
            cmd.Parameters.Add($"@{keyName3}", CoverTypeToDbType(keyValue3)).Value = keyValue3;
            cmd.Parameters.Add($"@{keyName4}", CoverTypeToDbType(keyValue4)).Value = keyValue4;
        }

        public static void UpdateCmd<T1,T2>(this MySqlCommand cmd,string tableName, T1 keyValue1,string keyName1, T2 keyValue2, string keyName2)
        {
            cmd.CommandText = $"UPDATE {tableName} set {keyName1} = @{keyName1}, {keyName2} = @{keyName2} WHERE {keyName1} = @{keyName1};";
            cmd.Parameters.Add($"@{keyName1}", CoverTypeToDbType(keyValue1)).Value = keyValue1;
            cmd.Parameters.Add($"@{keyName2}", CoverTypeToDbType(keyValue2)).Value = keyValue2;
        }
        public static void UpdateCmd<T1, T2, T3>(this MySqlCommand cmd, string tableName, T1 keyValue1, string keyName1, T2 keyValue2, string keyName2, T3 keyValue3, string keyName3)
        {
            cmd.CommandText = $"UPDATE {tableName} set {keyName1} = @{keyName1}, {keyName2} = @{keyName2}, {keyName3} = @{keyName3} WHERE {keyName1} = @{keyName1};";
            cmd.Parameters.Add($"@{keyName1}", CoverTypeToDbType(keyValue1)).Value = keyValue1;
            cmd.Parameters.Add($"@{keyName2}", CoverTypeToDbType(keyValue2)).Value = keyValue2;
            cmd.Parameters.Add($"@{keyName3}", CoverTypeToDbType(keyValue3)).Value = keyValue3;
        }
        public static void UpdateCmd<T1, T2, T3, T4>(this MySqlCommand cmd, string tableName, T1 keyValue1, string keyName1, T2 keyValue2, string keyName2, T3 keyValue3, string keyName3, T4 keyValue4, string keyName4)
        {
            cmd.CommandText = $"UPDATE {tableName} set {keyName1} = @{keyName1}, {keyName2} = @{keyName2}, {keyName3} = @{keyName3}, {keyName4} = @{keyName4} WHERE {keyName1} = @{keyName1};";
            cmd.Parameters.Add($"@{keyName1}", CoverTypeToDbType(keyValue1)).Value = keyValue1;
            cmd.Parameters.Add($"@{keyName2}", CoverTypeToDbType(keyValue2)).Value = keyValue2;
            cmd.Parameters.Add($"@{keyName3}", CoverTypeToDbType(keyValue3)).Value = keyValue3;
            cmd.Parameters.Add($"@{keyName4}", CoverTypeToDbType(keyValue4)).Value = keyValue4;
        }

        public static async Task<T1> GetByteFileValueAsync<T1>(this DbDataReader dbDataReader, int index)
        {
            byte[] _byteData = await dbDataReader.GetFieldValueAsync<byte[]>(index);
            return MessagePackSerializer.Deserialize<T1>(_byteData);
        }

        public static void TruncateTable(this MySqlCommand cmd, string tableName)
        {
            cmd.CommandText = $"TRUNCATE Table {tableName}";
        }
    }
}
