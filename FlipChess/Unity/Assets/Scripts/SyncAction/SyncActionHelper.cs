using Codice.Client.BaseCommands.Fileinfo;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

namespace Dreamwear
{
    public class SyncFiledInfo
    {
        private FieldInfo m_fieldInfo;
        private object m_target;
        public object Value;

        public SyncFiledInfo(FieldInfo fieldInfo, object target)
        {
            m_fieldInfo = fieldInfo;
            m_target = target;
            Value = fieldInfo.GetValue(target);
        }

        public bool IsDirty
        {
            get
            {
                object newValue = m_fieldInfo.GetValue(m_target);
                if(newValue.Equals(Value))
                {
                    return false;
                }
                Value = newValue;
                return true;
            }
        }
    }

    public static class SyncActionHelper
    {
        private static Dictionary<string, ISyncAction> m_syncActionDict = new Dictionary<string, ISyncAction>();

        private static MethodInfo m_registerMethodInfo0;
        private static MethodInfo m_registerMethodInfo1;
        private static MethodInfo m_registerMethodInfo2;

        private static MethodInfo m_unRegisterMethodInfo0;
        private static MethodInfo m_unRegisterMethodInfo1;
        private static MethodInfo m_unRegisterMethodInfo2;

        private static Dictionary<string, SyncFiledInfo> m_syncFieldDict = new Dictionary<string, SyncFiledInfo>();
        static SyncActionHelper()
        {
            Type type = typeof(SyncActionHelper);

            m_registerMethodInfo0 = type.GetMethod(nameof(RegisterSyncMethod0), BindingFlags.NonPublic | BindingFlags.Static);
            m_registerMethodInfo1 = type.GetMethod(nameof(RegisterSyncMethod1), BindingFlags.NonPublic | BindingFlags.Static);
            m_registerMethodInfo2 = type.GetMethod(nameof(RegisterSyncMethod2), BindingFlags.NonPublic | BindingFlags.Static);

            m_unRegisterMethodInfo0 = type.GetMethod(nameof(UnRegisterSyncMethod0), BindingFlags.NonPublic | BindingFlags.Static);
            m_unRegisterMethodInfo1 = type.GetMethod(nameof(UnRegisterSyncMethod1), BindingFlags.NonPublic | BindingFlags.Static);
            m_unRegisterMethodInfo2 = type.GetMethod(nameof(UnRegisterSyncMethod2), BindingFlags.NonPublic | BindingFlags.Static);

            SyncAction fixUpdateAction = new SyncAction();
            fixUpdateAction += FixedUpdate;
            m_syncActionDict.Add("FixedUpdate", fixUpdateAction);
        }

        public static void FixedUpdate()
        {
            foreach(var var in m_syncFieldDict)
            {
                if(!m_syncActionDict.TryGetValue(var.Key,out ISyncAction syncAction))
                {
                    continue;
                }
                if (!var.Value.IsDirty)
                {
                    continue;
                }
                syncAction.Invoke(var.Value.Value);
            }
        }
        public static void RegisterSyncMethod(this MethodInfo methodInfo,object target,string syncName)
        {
            ParameterInfo[] pars = methodInfo.GetParameters();
            MethodInfo userMethodInfo = null;
            switch (pars.Length)
            {
                case 0:
                    userMethodInfo = m_registerMethodInfo0.MakeGenericMethod();
                    break;
                case 1:
                    userMethodInfo = m_registerMethodInfo1.MakeGenericMethod(pars[0].ParameterType);
                    break;
                case 2:
                    userMethodInfo = m_registerMethodInfo2.MakeGenericMethod(pars[0].ParameterType, pars[1].ParameterType);
                    break;
            }
            if (userMethodInfo == null)
            {
                return;
            }
            userMethodInfo.Invoke(null, new object[] { methodInfo, target, syncName });
        }
        private static void RegisterSyncMethod0(MethodInfo methodInfo, object target, string syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction actionEvnet))
            {
                actionEvnet = new SyncAction();
                m_syncActionDict.Add(syncName, actionEvnet);
            }
            if(!(actionEvnet is SyncAction))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Action), target);
            SyncAction tempSyncAction = actionEvnet as SyncAction;
            tempSyncAction += del as Action;
        }
        private static void RegisterSyncMethod1<T1>(MethodInfo methodInfo, object target, string syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction actionEvnet))
            {
                actionEvnet = new SyncAction<T1>();
                m_syncActionDict.Add(syncName, actionEvnet);
            }
            if (!(actionEvnet is SyncAction<T1>))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Action<T1>), target);
            SyncAction<T1> tempSyncAction = actionEvnet as SyncAction<T1>;
            tempSyncAction += del as Action<T1>;
        }
        private static void RegisterSyncMethod2<T1,T2>(MethodInfo methodInfo, object target, string syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction actionEvnet))
            {
                actionEvnet = new SyncAction<T1,T2>();
                m_syncActionDict.Add(syncName, actionEvnet);
            }
            if (!(actionEvnet is SyncAction<T1,T2>))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Action<T1,T2>), target);
            SyncAction<T1,T2> tempSyncAction = actionEvnet as SyncAction<T1,T2>;
            tempSyncAction += del as Action<T1,T2>;
        }
        public static void UnRegisterSyncMethod(this MethodInfo methodInfo, object target, string syncName)
        {
            ParameterInfo[] pars = methodInfo.GetParameters();
            MethodInfo userMethodInfo = null;
            switch (pars.Length)
            {
                case 0:
                    userMethodInfo = m_unRegisterMethodInfo0.MakeGenericMethod();
                    break;
                case 1:
                    userMethodInfo = m_unRegisterMethodInfo1.MakeGenericMethod(pars[0].ParameterType);
                    break;
                case 2:
                    userMethodInfo = m_unRegisterMethodInfo2.MakeGenericMethod(pars[0].ParameterType, pars[1].ParameterType);
                    break;
            }
            if (userMethodInfo == null)
            {
                return;
            }
            userMethodInfo.Invoke(null, new object[] { methodInfo, target, syncName });
        }
        private static void UnRegisterSyncMethod0(MethodInfo methodInfo, object target, string syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction actionEvnet))
            {
                return;
            }
            if (!(actionEvnet is SyncAction))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Action), target);
            SyncAction tempSyncAction = actionEvnet as SyncAction;
            tempSyncAction -= del as Action;
            if (!tempSyncAction.IsDispose())
            {
                return;
            }
            m_syncActionDict.Remove(syncName);
        }
        private static void UnRegisterSyncMethod1<T1>(MethodInfo methodInfo, object target, string syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction actionEvnet))
            {
                actionEvnet = new SyncAction<T1>();
                m_syncActionDict.Add(syncName, actionEvnet);
            }
            if (!(actionEvnet is SyncAction<T1>))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Action<T1>), target);
            SyncAction<T1> tempSyncAction = actionEvnet as SyncAction<T1>;
            tempSyncAction -= del as Action<T1>;
            if (!tempSyncAction.IsDispose())
            {
                return;
            }
            m_syncActionDict.Remove(syncName);
        }
        private static void UnRegisterSyncMethod2<T1, T2>(MethodInfo methodInfo, object target, string syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction actionEvnet))
            {
                actionEvnet = new SyncAction<T1, T2>();
                m_syncActionDict.Add(syncName, actionEvnet);
            }
            if (!(actionEvnet is SyncAction<T1, T2>))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Action<T1, T2>), target);
            SyncAction<T1, T2> tempSyncAction = actionEvnet as SyncAction<T1, T2>;
            tempSyncAction -= del as Action<T1, T2>;
            if (!tempSyncAction.IsDispose())
            {
                return;
            }
            m_syncActionDict.Remove(syncName);
        }
        public static void RegisterSyncField(this FieldInfo fieldInfo, object target,string syncName)
        {
            if (m_syncFieldDict.ContainsKey(syncName))
            {
                Debug.LogError($"重复注册事件:{syncName} Field:{fieldInfo.Name}");
                return;
            }
            m_syncFieldDict.Add(syncName, new SyncFiledInfo(fieldInfo, target));
        }
        public static void UnRegisterSyncField(this FieldInfo fieldInfo, object target, string syncName)
        {
            if (!m_syncFieldDict.ContainsKey(syncName))
            {
                return;
            }
            m_syncFieldDict.Remove(syncName);
        }
        public static void BroadcastSyncEvent(this string syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction syncAction))
            {
                return;
            }
            if(!(syncAction is SyncAction))
            {
                Debug.LogError($"广播类型错误: {syncName}");
                return;
            }
            (syncAction as SyncAction).Invoke();
        }
        public static void BroadcastSyncEvent<T1>(this string syncName,T1 value1)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction syncAction))
            {
                return;
            }
            if (!(syncAction is SyncAction<T1>))
            {
                Debug.LogError($"广播类型错误: {syncName}");
                return;
            }
            (syncAction as SyncAction<T1>).Invoke(value1);
        }
        public static void BroadcastSyncEvent<T1,T2>(this string syncName,T1 value1,T2 value2)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction syncAction))
            {
                return;
            }
            if (!(syncAction is SyncAction<T1,T2>))
            {
                Debug.LogError($"广播类型错误: {syncName}");
                return;
            }
            (syncAction as SyncAction<T1,T2>).Invoke(value1,value2);
        }

    }
}

