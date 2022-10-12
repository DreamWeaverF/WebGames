using System;

namespace GameCommon
{
    public interface ISyncFunc
    {
        bool IsDispose();
    }
    public class SyncFunc<T1> : ISyncFunc
    {
        private Func<T1> m_func;
        public T1 Invoke()
        {
            if (m_func == null)
            {
                return default(T1);
            }
            return m_func.Invoke();
        }
        public static SyncFunc<T1> operator +(SyncFunc<T1> a, Func<T1> b)
        {
            a.m_func -= b;
            a.m_func += b;
            return a;
        }
        public static SyncFunc<T1> operator -(SyncFunc<T1> a, Func<T1> b)
        {
            a.m_func -= b;
            return a;
        }
        public bool IsDispose()
        {
            return m_func == null;
        }
    }
    public class SyncFunc<T1,T2> : ISyncFunc
    {
        private Func<T1,T2> m_func;
        public T2 Invoke(T1 para1)
        {
            if (m_func == null)
            {
                return default(T2);
            }
            return m_func.Invoke(para1);
        }
        public static SyncFunc<T1, T2> operator +(SyncFunc<T1, T2> a, Func<T1,T2> b)
        {
            a.m_func -= b;
            a.m_func += b;
            return a;
        }
        public static SyncFunc<T1, T2> operator -(SyncFunc<T1, T2> a, Func<T1,T2> b)
        {
            a.m_func -= b;
            return a;
        }
        public bool IsDispose()
        {
            return m_func == null;
        }
    }
    public class SyncFunc<T1, T2, T3> : ISyncFunc
    {
        private Func<T1, T2, T3> m_func;
        public T3 Invoke(T1 para1,T2 para2)
        {
            if (m_func == null)
            {
                return default(T3);
            }
            return m_func.Invoke(para1, para2);
        }
        public static SyncFunc<T1, T2, T3> operator +(SyncFunc<T1, T2, T3> a, Func<T1, T2, T3> b)
        {
            a.m_func -= b;
            a.m_func += b;
            return a;
        }
        public static SyncFunc<T1, T2, T3> operator -(SyncFunc<T1, T2, T3> a, Func<T1, T2, T3> b)
        {
            a.m_func -= b;
            return a;
        }
        public bool IsDispose()
        {
            return m_func == null;
        }
    }
}
