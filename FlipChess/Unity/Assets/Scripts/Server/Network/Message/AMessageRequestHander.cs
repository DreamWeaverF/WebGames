using GameCommon;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

namespace GameServer
{
    public abstract class AMessageRequestHander<T1,T2> : AMessageRequestHander where T1 : AMessageRequest where T2 : AMessageResponse, new()
    {
        protected T2 m_response = new T2();
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
            if(userId == 0)
            {
                m_response.ErrorCode = MessageErrorCode.UserNotLogged;
                return m_response;
            }
            m_response.RpcId = request.RpcId;
            await OnMessage(userId,request as T1);
            return m_response;
        }
        protected virtual async Task OnMessage(long userId, T1 request)
        {
            await Task.CompletedTask;
        }
    }

    public abstract class AMessageRequestHander : ScriptableObject
    {
        public abstract Type GetRequestType();
        public abstract Type GetResponseType();

        public abstract Task<AMessageResponse> OnMessage(long userId,AMessageRequest request);
    }
}
