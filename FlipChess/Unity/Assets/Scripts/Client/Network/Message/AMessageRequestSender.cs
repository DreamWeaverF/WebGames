using GameCommon;
using ET;
using System.Threading.Tasks;
using UnityEngine;
using System.Reflection;

namespace GameClient
{
    public abstract class AMessageRequestSender<T1,T2> : ScriptableObject where T1 : AMessageRequest,new() where T2 : AMessageResponse
    {
        protected FieldInfo[] m_fields;
        protected T1 m_request;
        void OnEnable()
        {
            m_fields = typeof(T1).GetFields();
        }
        public async Task<T2> SendMessage(params object[] pars)
        {
            if(m_request == null)
            {
                m_request = new T1();
            }
            if(m_fields.Length != pars.Length)
            {
                Debug.LogError($"消息参数错误:{m_request.GetType().Name}");
                return null;
            }
            for(int i = 0; i < pars.Length; i++)
            {
                m_fields[i].SetValue(m_request,pars[i]);
            }
            ETTask<AMessageResponse> task = SyncName.MessageRequestSender.BroadcastSyncEvent<AMessageRequest, ETTask<AMessageResponse>>(m_request);
            return await task as T2;
        }
    }
}
