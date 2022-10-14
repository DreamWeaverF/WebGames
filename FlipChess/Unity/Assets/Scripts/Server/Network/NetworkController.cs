using GameCommon;
using System;
using WebSocketSharp;
using MessagePack;
using WebSocketSharp.Server;
using UnityEngine;
using System.Collections.Generic;

namespace GameServer
{
    public class NetworkController : MonoBehaviourEx
    {
        [SerializeField]
        private ConfigServerLaunch m_configServerLaunch;

        private Dictionary<long, UserNetBehavior> m_userNets = new Dictionary<long, UserNetBehavior>();
        private Dictionary<Type, AMessageRequestHander> m_requestHanders = new Dictionary<Type, AMessageRequestHander>();
        private MessageNoticeError m_noticeError = new MessageNoticeError();

        [SerializeField]
        private MessageRequestLoginHander m_loginHander;

        void Start()
        {
            WebSocketServer wss = new WebSocketServer(m_configServerLaunch.ListenerPort);
            wss.AddWebSocketService<UserNetBehavior>("/connect", OnConnectService);
            wss.Start();
        }
        private void OnConnectService(UserNetBehavior webSocketBehavior)
        {
            Debug.Log("Service Connect");
            webSocketBehavior.OnActionMessage = OnMessage;
            webSocketBehavior.OnActionClose = OnClose;
        }
        private async void OnMessage(long userId, MessageEventArgs e, UserNetBehavior net)
        {
            byte[] bytes;
            IMessage message = MessagePackSerializer.Deserialize<IMessage>(e.RawData);
            if(message == null)
            {
                m_noticeError.ErrorCode = MessageErrorCode.MessageError;
                bytes = MessagePackSerializer.Serialize<IMessage>(m_noticeError);
                net.SendMessage(bytes, true);
                return;
            }
            if(!(message is AMessageRequest))
            {
                m_noticeError.ErrorCode = MessageErrorCode.MessageError;
                bytes = MessagePackSerializer.Serialize<IMessage>(m_noticeError);
                net.SendMessage(bytes, true);
                return;
            }
            AMessageRequest request = message as AMessageRequest;
            Type type = message.GetType();
            if(type == typeof(MessageRequestLogin))
            {
                if(userId != 0)
                {
                    //
                }
                userId = 0;
                MessageResponseLogin responseLogin = await m_loginHander.OnMessage(userId, request) as MessageResponseLogin;
                if(responseLogin.ErrorCode == MessageErrorCode.Success)
                {
                    net.UserId = responseLogin.UserId;
                    m_userNets.Add(responseLogin.UserId, net);
                }
                bytes = MessagePackSerializer.Serialize<IMessage>(responseLogin);
                net.SendMessage(bytes, true);
                return;
            }
            if(!m_requestHanders.TryGetValue(type, out AMessageRequestHander hander))
            {
                m_noticeError.ErrorCode = MessageErrorCode.MessageError;
                bytes = MessagePackSerializer.Serialize<IMessage>(m_noticeError);
                net.SendMessage(bytes, true);
                return;
            }
            AMessageResponse response = await hander.OnMessage(userId, request);
            bytes = MessagePackSerializer.Serialize<IMessage>(response);
            net.SendMessage(bytes);
        }
        private void OnClose(long useId)
        {
            m_userNets.Remove(useId);
        }
    }
}
