using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace GameCommon
{
    class SyncRepository
    {
        private Dictionary<SyncName, ISyncAction> m_syncActionDict = new Dictionary<SyncName, ISyncAction>();
        private Dictionary<SyncName, ISyncFunc> m_syncFuncDict = new Dictionary<SyncName, ISyncFunc>();

        private MethodInfo m_registerSyncActionMethod0;
        private MethodInfo m_registerSyncActionMethod1;
        private MethodInfo m_registerSyncActionMethod2;

        private MethodInfo m_registerSyncFuncMethod0;
        private MethodInfo m_registerSyncFuncMethod1;
        private MethodInfo m_registerSyncFuncMethod2;

        private MethodInfo m_unRegisterSyncActionMethod0;
        private MethodInfo m_unRegisterSyncActionMethod1;
        private MethodInfo m_unRegisterSyncActionMethod2;

        private MethodInfo m_unRegisterSyncFuncMethod0;
        private MethodInfo m_unRegisterSyncFuncMethod1;
        private MethodInfo m_unRegisterSyncFuncMethod2;

        public SyncRepository()
        {
            Type type = this.GetType();

            m_registerSyncActionMethod0 = type.GetMethod(nameof(RegisterSyncAction0), BindingFlags.NonPublic | BindingFlags.Instance);
            m_registerSyncActionMethod1 = type.GetMethod(nameof(RegisterSyncAction1), BindingFlags.NonPublic | BindingFlags.Instance);
            m_registerSyncActionMethod2 = type.GetMethod(nameof(RegisterSyncAction2), BindingFlags.NonPublic | BindingFlags.Instance);

            m_registerSyncFuncMethod0 = type.GetMethod(nameof(RegisterSyncFunc0), BindingFlags.NonPublic | BindingFlags.Instance);
            m_registerSyncFuncMethod1 = type.GetMethod(nameof(RegisterSyncFunc1), BindingFlags.NonPublic | BindingFlags.Instance);
            m_registerSyncFuncMethod2 = type.GetMethod(nameof(RegisterSyncFunc2), BindingFlags.NonPublic | BindingFlags.Instance);

            m_unRegisterSyncActionMethod0 = type.GetMethod(nameof(UnRegisterSyncAction0), BindingFlags.NonPublic | BindingFlags.Instance);
            m_unRegisterSyncActionMethod1 = type.GetMethod(nameof(UnRegisterSyncAction1), BindingFlags.NonPublic | BindingFlags.Instance);
            m_unRegisterSyncActionMethod2 = type.GetMethod(nameof(UnRegisterSyncAction2), BindingFlags.NonPublic | BindingFlags.Instance);

            m_unRegisterSyncFuncMethod0 = type.GetMethod(nameof(UnRegisterSyncAction0), BindingFlags.NonPublic | BindingFlags.Instance);
            m_unRegisterSyncFuncMethod1 = type.GetMethod(nameof(UnRegisterSyncAction1), BindingFlags.NonPublic | BindingFlags.Instance);
            m_unRegisterSyncFuncMethod2 = type.GetMethod(nameof(UnRegisterSyncAction2), BindingFlags.NonPublic | BindingFlags.Instance);
        }
        #region register
        public void RegisterSyncClass(Type classType,object classInstance)
        {
            MethodInfo[] methods = classType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            MethodInfo method;
            MethodInfo userMethod = null;
            for (int i = 0; i < methods.Length; i++)
            {
                method = methods[i];
                var atts = method.GetCustomAttributes<AAttribute>();
                if (atts == null)
                {
                    continue;
                }
                if (atts.Any() == false)
                {
                    continue;
                }
                foreach (var att in atts)
                {
                    if (att is SynchronizeMethodAttribute)
                    {
                        ParameterInfo[] pars = method.GetParameters();
                        Type returnType = method.ReturnType;
                        if(returnType == typeof(void))
                        {
                            switch (pars.Length)
                            {
                                case 0:
                                    userMethod = m_registerSyncActionMethod0;
                                    break;
                                case 1:
                                    userMethod = m_registerSyncActionMethod1.MakeGenericMethod(pars[0].ParameterType);
                                    break;
                                case 2:
                                    userMethod = m_registerSyncActionMethod2.MakeGenericMethod(pars[0].ParameterType, pars[1].ParameterType);
                                    break;
                            }
                        }
                        else
                        {
                            switch (pars.Length)
                            {
                                case 0:
                                    userMethod = m_registerSyncFuncMethod0.MakeGenericMethod(returnType);
                                    break;
                                case 1:
                                    userMethod = m_registerSyncFuncMethod1.MakeGenericMethod(pars[0].ParameterType, returnType);
                                    break;
                                case 2:
                                    userMethod = m_registerSyncFuncMethod2.MakeGenericMethod(pars[0].ParameterType, pars[1].ParameterType, returnType);
                                    break;
                            }
                        }
                        if (userMethod == null)
                        {
                            continue;
                        }
                        userMethod.Invoke(this, new object[] { method, classInstance, (att as SynchronizeMethodAttribute).SyncName });

                    }
                }
            }
        }
        private void RegisterSyncAction0(MethodInfo methodInfo, object target, SyncName syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction actionEvnet))
            {
                actionEvnet = new SyncAction();
                m_syncActionDict.Add(syncName, actionEvnet);
            }
            if (!(actionEvnet is SyncAction))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Action), target);
            SyncAction tempSyncAction = actionEvnet as SyncAction;
            tempSyncAction += del as Action;
        }
        private void RegisterSyncAction1<T1>(MethodInfo methodInfo, object target, SyncName syncName)
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
        private void RegisterSyncAction2<T1, T2>(MethodInfo methodInfo, object target, SyncName syncName)
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
            tempSyncAction += del as Action<T1, T2>;
        }
        private void RegisterSyncFunc0<T1>(MethodInfo methodInfo, object target, SyncName syncName)
        {
            if (!m_syncFuncDict.TryGetValue(syncName, out ISyncFunc funcEvnet))
            {
                funcEvnet = new SyncFunc<T1>();
                m_syncFuncDict.Add(syncName, funcEvnet);
            }
            if (!(funcEvnet is SyncFunc<T1>))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Func<T1>), target);
            SyncFunc<T1> tempSyncFunc = funcEvnet as SyncFunc<T1>;
            tempSyncFunc += del as Func<T1>;
        }
        private void RegisterSyncFunc1<T1, T2>(MethodInfo methodInfo, object target, SyncName syncName)
        {
            if (!m_syncFuncDict.TryGetValue(syncName, out ISyncFunc funcEvnet))
            {
                funcEvnet = new SyncFunc<T1,T2>();
                m_syncFuncDict.Add(syncName, funcEvnet);
            }
            if (!(funcEvnet is SyncFunc<T1,T2>))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Func<T1,T2>), target);
            SyncFunc<T1,T2> tempSyncFunc = funcEvnet as SyncFunc<T1,T2>;
            tempSyncFunc += del as Func<T1,T2>;
        }
        private void RegisterSyncFunc2<T1, T2, T3>(MethodInfo methodInfo, object target, SyncName syncName)
        {
            if (!m_syncFuncDict.TryGetValue(syncName, out ISyncFunc funcEvnet))
            {
                funcEvnet = new SyncFunc<T1, T2, T3>();
                m_syncFuncDict.Add(syncName, funcEvnet);
            }
            if (!(funcEvnet is SyncFunc<T1, T2, T3>))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Func<T1, T2, T3>), target);
            SyncFunc<T1, T2, T3> tempSyncFunc = funcEvnet as SyncFunc<T1, T2, T3>;
            tempSyncFunc += del as Func<T1, T2, T3>;
        }
        #endregion

        #region UnRegister
        public void UnRegisterSyncClass(Type classType, object classInstance)
        {
            MethodInfo[] methods = classType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            MethodInfo method;
            MethodInfo userMethod = null;
            for (int i = 0; i < methods.Length; i++)
            {
                method = methods[i];
                var atts = method.GetCustomAttributes<AAttribute>();
                if (atts == null)
                {
                    continue;
                }
                if (atts.Any() == false)
                {
                    continue;
                }
                foreach (var att in atts)
                {
                    if (att is SynchronizeMethodAttribute)
                    {
                        ParameterInfo[] pars = method.GetParameters();
                        Type returnType = method.ReturnType;
                        if (returnType == typeof(void))
                        {
                            switch (pars.Length)
                            {
                                case 0:
                                    userMethod = m_unRegisterSyncActionMethod0;
                                    break;
                                case 1:
                                    userMethod = m_unRegisterSyncActionMethod1.MakeGenericMethod(pars[0].ParameterType);
                                    break;
                                case 2:
                                    userMethod = m_unRegisterSyncActionMethod2.MakeGenericMethod(pars[0].ParameterType, pars[1].ParameterType);
                                    break;
                            }
                        }
                        else
                        {
                            switch (pars.Length)
                            {
                                case 0:
                                    userMethod = m_unRegisterSyncFuncMethod0.MakeGenericMethod(returnType);
                                    break;
                                case 1:
                                    userMethod = m_unRegisterSyncFuncMethod1.MakeGenericMethod(pars[0].ParameterType, returnType);
                                    break;
                                case 2:
                                    userMethod = m_unRegisterSyncFuncMethod2.MakeGenericMethod(pars[0].ParameterType, pars[1].ParameterType, returnType);
                                    break;
                            }
                        }
                        if (userMethod == null)
                        {
                            continue;
                        }
                        userMethod.Invoke(this, new object[] { method, classInstance, (att as SynchronizeMethodAttribute).SyncName });

                    }
                }
            }
        }
        private void UnRegisterSyncAction0(MethodInfo methodInfo, object target, SyncName syncName)
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
        private void UnRegisterSyncAction1<T1>(MethodInfo methodInfo, object target, SyncName syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction actionEvnet))
            {
                return;
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
        private void UnRegisterSyncAction2<T1, T2>(MethodInfo methodInfo, object target, SyncName syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction actionEvnet))
            {
                return;
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
        private void UnRegisterSyncFunc0<T1>(MethodInfo methodInfo, object target, SyncName syncName)
        {
            if (!m_syncFuncDict.TryGetValue(syncName, out ISyncFunc funcEvnet))
            {
                return;
            }
            if (!(funcEvnet is SyncFunc<T1>))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Func<T1>), target);
            SyncFunc<T1> tempSyncFunc = funcEvnet as SyncFunc<T1>;
            tempSyncFunc -= del as Func<T1>;
            if (!tempSyncFunc.IsDispose())
            {
                return;
            }
            m_syncFuncDict.Remove(syncName);
        }
        private void UnRegisterSyncFunc1<T1, T2>(MethodInfo methodInfo, object target, SyncName syncName)
        {
            if (!m_syncFuncDict.TryGetValue(syncName, out ISyncFunc funcEvnet))
            {
                return;
            }
            if (!(funcEvnet is SyncFunc<T1,T2>))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Func<T1,T2>), target);
            SyncFunc<T1,T2> tempSyncFunc = funcEvnet as SyncFunc<T1,T2>;
            tempSyncFunc -= del as Func<T1,T2>;
            if (!tempSyncFunc.IsDispose())
            {
                return;
            }
            m_syncFuncDict.Remove(syncName);
        }
        private void UnRegisterSyncFunc2<T1, T2, T3>(MethodInfo methodInfo, object target, SyncName syncName)
        {
            if (!m_syncFuncDict.TryGetValue(syncName, out ISyncFunc funcEvnet))
            {
                return;
            }
            if (!(funcEvnet is SyncFunc<T1,T2,T3>))
            {
                return;
            }
            Delegate del = methodInfo.CreateDelegate(typeof(Func<T1,T2,T3>), target);
            SyncFunc<T1,T2,T3> tempSyncFunc = funcEvnet as SyncFunc<T1,T2,T3>;
            tempSyncFunc -= del as Func<T1,T2,T3>;
            if (!tempSyncFunc.IsDispose())
            {
                return;
            }
            m_syncFuncDict.Remove(syncName);
        }
        #endregion

        #region Broadcast
        public void BroadcastSyncEvent(SyncName syncName)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction syncAction))
            {
                return;
            }
            if (!(syncAction is SyncAction))
            {
                Debug.LogError($"广播类型错误: {syncName}");
                return;
            }
            (syncAction as SyncAction).Invoke();
        }
        public void BroadcastSyncEvent<T1>(SyncName syncName, T1 value1)
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
        public void BroadcastSyncEvent<T1, T2>(SyncName syncName, T1 value1, T2 value2)
        {
            if (!m_syncActionDict.TryGetValue(syncName, out ISyncAction syncAction))
            {
                return;
            }
            if (!(syncAction is SyncAction<T1, T2>))
            {
                Debug.LogError($"广播类型错误: {syncName}");
                return;
            }
            (syncAction as SyncAction<T1, T2>).Invoke(value1, value2);
        }

        public T1 BroadcastSyncEvent<T1>(SyncName syncName)
        {
            if (!m_syncFuncDict.TryGetValue(syncName, out ISyncFunc syncFunc))
            {
                return default(T1);
            }
            if (!(syncFunc is SyncFunc<T1>))
            {
                Debug.LogError($"广播类型错误: {syncName}");
                return default(T1);
            }
            return (syncFunc as SyncFunc<T1>).Invoke();
        }
        public T2 BroadcastSyncEvent<T1,T2>(SyncName syncName,T1 value1)
        {
            if (!m_syncFuncDict.TryGetValue(syncName, out ISyncFunc syncFunc))
            {
                return default(T2);
            }
            if (!(syncFunc is SyncFunc<T1,T2>))
            {
                Debug.LogError($"广播类型错误: {syncName}");
                return default(T2);
            }
            return (syncFunc as SyncFunc<T1,T2>).Invoke(value1);
        }
        public T3 BroadcastSyncEvent<T1,T2,T3>(SyncName syncName, T1 value1, T2 value2)
        {
            if (!m_syncFuncDict.TryGetValue(syncName, out ISyncFunc syncFunc))
            {
                return default(T3);
            }
            if (!(syncFunc is SyncFunc<T1,T2,T3>))
            {
                Debug.LogError($"广播类型错误: {syncName}");
                return default(T3);
            }
            return (syncFunc as SyncFunc<T1,T2,T3>).Invoke(value1,value2);
        }
        #endregion
    }

    public static class SyncHelper
    {
        private static SyncRepository m_repository;

        static SyncHelper()
        {
            m_repository = new SyncRepository();
        }
        public static void RegisterSyncClass(this Type classType, object classObject = null)
        {
            m_repository.RegisterSyncClass(classType, classObject);
        }
        public static void UnRegisterSyncClass(this Type classType, object classObject = null)
        {
            m_repository.RegisterSyncClass(classType, classObject);
        }
        public static void BroadcastSyncEvent(this SyncName syncName)
        {
            m_repository.BroadcastSyncEvent(syncName);
        }
        public static void BroadcastSyncEvent<T1>(this SyncName syncName, T1 value1)
        {
            m_repository.BroadcastSyncEvent(syncName, value1);
        }
        public static void BroadcastSyncEvent<T1, T2>(this SyncName syncName, T1 value1, T2 value2)
        {
            m_repository.BroadcastSyncEvent(syncName, value1, value2);
        }
        public static T1 BroadcastSyncEvent<T1>(this SyncName syncName)
        {
            return m_repository.BroadcastSyncEvent<T1>(syncName);
        }
        public static T2 BroadcastSyncEvent<T1,T2>(this SyncName syncName,T1 value1)
        {
            return m_repository.BroadcastSyncEvent<T1,T2>(syncName, value1);
        }
        public static T3 BroadcastSyncEvent<T1,T2,T3>(this SyncName syncName, T1 value1, T2 value2)
        {
            return m_repository.BroadcastSyncEvent<T1,T2,T3>(syncName, value1,value2);
        }
    }
}

