using ET;
using GameCommon;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    public class AMessageNoticeSender<T1> : ScriptableObject where T1 : AMessageNotice,new()
    {
        protected T1 m_notice = new T1();
        protected FieldInfo[] m_fields;
        void OnEnable()
        {
            m_fields = typeof(T1).GetFields();
        }
        public void SendMessage(List<long> userIds, params object[] pars)
        {
            if (m_notice == null)
            {
                m_notice = new T1();
            }
            if (m_fields.Length != pars.Length)
            {
                Debug.LogError($"消息参数错误:{m_notice.GetType().Name}");
                return;
            }
            for (int i = 0; i < pars.Length; i++)
            {
                m_fields[i].SetValue(m_notice, pars[i]);
            }
            SyncName.MessageNoticeSender.BroadcastSyncEvent(userIds,m_notice);
        }
    }
}
