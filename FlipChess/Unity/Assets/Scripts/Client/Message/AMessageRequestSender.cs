using GameCommon;
using ET;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    public abstract class AMessageRequestSender<T1,T2> : ScriptableObject where T1 : AMessageRequest,new() where T2 : AMessageResponse
    {
        private T1 m_message;
        protected T1 m_request
        {
            get
            {
                if (m_message == null)
                {
                    m_message = new T1();
                }
                return m_message;
            }
        }
        protected async Task<T2> SendMessageCore()
        {
            ETTask<AMessageResponse> task = SyncName.MessageRequestSender.BroadcastSyncEvent<AMessageRequest, ETTask<AMessageResponse>>(m_message);
            return await task as T2;
        }
    }
}
