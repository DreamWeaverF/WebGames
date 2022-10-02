using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Dreamwear
{
    public interface ISyncAction
    {
        void Invoke(params object[] kPara);

        bool IsDispose();
    }
    public class SyncAction : ISyncAction
    {
        private Action m_action;
        public void Invoke(params object[] kPara)
        {
            if (kPara.Length != 0)
            {
                return;
            }
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
        public void Invoke(params object[] kPara)
        {
            if (kPara.Length != 1)
            {
                return;
            }
            m_action?.Invoke((T1)kPara[0]);
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
        public void Invoke(params object[] kPara)
        {
            if (kPara.Length != 2)
            {
                return;
            }
            m_action?.Invoke((T1)kPara[0], (T2)kPara[1]);
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
}
