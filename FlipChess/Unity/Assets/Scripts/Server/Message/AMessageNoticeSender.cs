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
        [SerializeField]
        private bool m_bCloseSocket;

        private T1 m_message;
        protected T1 m_notice
        {
            get
            {
                if(m_message == null)
                {
                    m_message = new T1();
                }
                return m_message;
            }
        }
        protected void SendMessageCore(List<long> userIds)
        {
            SyncName.MessageNoticeSender.BroadcastSyncEvent(userIds, m_bCloseSocket, m_message);
        }
    }
}
