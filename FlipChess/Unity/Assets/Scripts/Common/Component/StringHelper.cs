using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Numerics;
using System.Reflection;
using UnityEngine;

namespace GameCommon
{
    public static class StringHelper
    {
        private static string m_ignoreString = "xx";
        private static MethodInfo m_covertMethod;
        static StringHelper()
        {
            Type type = typeof(StringHelper);
            m_covertMethod = type.GetMethod(nameof(ConvertT), BindingFlags.NonPublic | BindingFlags.Static);

        }
        public static object Convert(this string str, Type type)
        {
            MethodInfo userMethod = m_covertMethod.MakeGenericMethod(type);
            return userMethod.Invoke(null, new object[] { str });
        }
        private static T ConvertT<T>(string str)
        {
            try
            {
                if(str == m_ignoreString)
                {
                    return default(T);
                }
                object result;
                switch (typeof(T).Name)
                {
                    case "string":
                        result = str;
                        break;
                    case "Vector2I":
                        {
                            string[] strs = str.Split("#");
                            result = new Vector2I();
                            (result as Vector2I).x = int.Parse(strs[0]);
                            (result as Vector2I).y = int.Parse(strs[1]);
                        }

                        break;
                    case "Vector3I":
                        {
                            string[] strs = str.Split("#");
                            result = new Vector3I();
                            (result as Vector3I).x = int.Parse(strs[0]);
                            (result as Vector3I).y = int.Parse(strs[1]);
                            (result as Vector3I).z = int.Parse(strs[2]);
                        }
                        break;
                    default:
                        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
                        result = converter.ConvertFromString(str);
                        break;
                }
                return (T)result;
            }
            catch (Exception)
            {
                return default(T);
            }
        }
    }
}
