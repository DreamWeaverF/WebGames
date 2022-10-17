using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCommon
{
    public interface ISyncAction
    {
        bool IsDispose();
    }
    public class SyncAction : ISyncAction
    {
        private Action m_action;
        public void Invoke()
        {
            m_action?.Invoke();
        }
        public static SyncAction operator +(SyncAction a, Action b)
        {
            a.m_action -= b;
            a.m_action += b;
            return a;
        }
        public static SyncAction operator -(SyncAction a, Action b)
        {
            a.m_action -= b;
            return a;
        }
        public bool IsDispose()
        {
            return m_action == null;
        }
    }
    public class SyncAction<T1> : ISyncAction
    {
        private Action<T1> m_action;
        public void Invoke(T1 para1)
        {
            m_action?.Invoke(para1);
        }
        public static SyncAction<T1> operator +(SyncAction<T1> a, Action<T1> b)
        {
            a.m_action -= b;
            a.m_action += b;
            return a;
        }
        public static SyncAction<T1> operator -(SyncAction<T1> a, Action<T1> b)
        {
            a.m_action -= b;
            return a;
        }
        public bool IsDispose()
        {
            return m_action == null;
        }
    }
    public class SyncAction<T1,T2> : ISyncAction
    {
        private Action<T1,T2> m_action;
        public void Invoke(T1 para1,T2 para2)
        {
            m_action?.Invoke(para1, para2);
        }
        public static SyncAction<T1,T2> operator +(SyncAction<T1,T2> a, Action<T1,T2> b)
        {
            a.m_action -= b;
            a.m_action += b;
            return a;
        }
        public static SyncAction<T1,T2> operator -(SyncAction<T1,T2> a, Action<T1,T2> b)
        {
            a.m_action -= b;
            return a;
        }
        public bool IsDispose()
        {
            return m_action == null;
        }
    }
    public class SyncAction<T1, T2, T3> : ISyncAction
    {
        private Action<T1, T2, T3> m_action;
        public void Invoke(T1 para1, T2 para2, T3 para3)
        {
            m_action?.Invoke(para1, para2, para3);
        }
        public static SyncAction<T1, T2, T3> operator +(SyncAction<T1, T2, T3> a, Action<T1, T2, T3> b)
        {
            a.m_action -= b;
            a.m_action += b;
            return a;
        }
        public static SyncAction<T1, T2, T3> operator -(SyncAction<T1, T2, T3> a, Action<T1, T2, T3> b)
        {
            a.m_action -= b;
            return a;
        }
        public bool IsDispose()
        {
            return m_action == null;
        }
    }
}
