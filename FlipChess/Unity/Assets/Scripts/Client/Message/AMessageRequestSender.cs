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
        protected T2 m_response;
        protected async Task<bool> BroadMessage()
        {
            ETTask<AMessageResponse> task = SyncName.MessageRequestSender.BroadcastSyncEvent<AMessageRequest, ETTask<AMessageResponse>>(m_message);
            AMessageResponse response = await task;
            m_response = response as T2;
            if(m_response.ErrorCode != MessageErrorCode.Success)
            {
                Debug.Log($"MessageError {m_response.GetType().Name} Error {m_response.ErrorCode}");
                return false;
            }
            return true;
        }
    }
}
