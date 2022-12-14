using GameCommon;
using System;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    public abstract class AMessageRequestHander<T1,T2> : AMessageRequestHander where T1 : AMessageRequest where T2 : AMessageResponse, new()
    {
        [SerializeField]
        protected UserRespository m_userRespository;
        [SerializeField]
        protected TimerStorage m_timerStorage;

        private T2 m_message;

        protected T2 m_response
        {
            get
            {
                if(m_message == null)
                {
                    m_message = new T2();
                }
                return m_message;
            }
        }
        public override Type GetRequestType()
        {
            return typeof(T1);
        }
        public override Type GetResponseType()
        {
            return typeof(T2);
        }
        public override async Task<AMessageResponse> OnMessage(long userId,AMessageRequest request)
        {
            m_response.ErrorCode = MessageErrorCode.Success;
            if (userId == 0)
            {
                m_message.ErrorCode = MessageErrorCode.UserNotLogged;
                return m_message;
            }
            if (!m_userRespository.UserDatas.TryGetValue(userId,out UserData userData))
            {
                m_message.ErrorCode = MessageErrorCode.UserNotLogged;
                return m_message;
            }
            m_message.RpcId = request.RpcId;
            userData.LastMessageTime = m_timerStorage.MilliSecond;
            await OnMessage(userData, request as T1);
            return m_message;
        }
        protected virtual async Task OnMessage(UserData userData, T1 request)
        {
            await Task.CompletedTask;
        }
    }
    public abstract class AMessageRequestHander : ScriptableObject
    {
        public abstract Type GetRequestType();
        public abstract Type GetResponseType();

        public abstract Task<AMessageResponse> OnMessage(long userId, AMessageRequest request);
    }
}
