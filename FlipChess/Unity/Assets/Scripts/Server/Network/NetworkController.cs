using GameCommon;
using System;
using WebSocketSharp;
using MessagePack;
using WebSocketSharp.Server;
using UnityEngine;
using System.Collections.Generic;

namespace GameServer
{
    public class NetworkController : AMonoBehaviour
    {
        [SerializeField]
        private ConfigServerLaunch m_configServerLaunch;
        [SerializeField]
        private MessageRequestLoginHander m_loginHander;
        [SerializeField]
        private SerializationDictionary<Type, AMessageRequestHander> m_requestHanders;

        private Dictionary<long, UserNetBehavior> m_userNets = new Dictionary<long, UserNetBehavior>();
        private MessageNoticeError m_noticeError = new MessageNoticeError();

        private WebSocketServer m_webScoketServer;
        protected override void OnInit()
        {
            m_webScoketServer = new WebSocketServer(m_configServerLaunch.ListenerPort);
            m_webScoketServer.AddWebSocketService<UserNetBehavior>("/connect", OnConnectService);
            m_webScoketServer.Start();
        }
        protected override void UnInit()
        {
            if(m_webScoketServer != null)
            {
                m_webScoketServer.RemoveWebSocketService("/connect");
                m_webScoketServer.Stop();
                m_webScoketServer = null;
            }
        }
        [SynchronizeMethod(SyncName = SyncName.MessageNoticeSender)]
        private void OnCallbackMessageNoticeSender(List<long> userIds, bool bCloseSocket, AMessageNotice notice)
        {
            if(userIds.Count <= 0)
            {
                Debug.Log($"广播消息没有推送接收玩家 {notice.GetType().Name}");
                return;
            }
            byte[] bytes = MessagePackSerializer.Serialize<IMessage>(notice, MessagePackSerializerOptions.Standard);
            for(int i = 0; i < userIds.Count; i++)
            {
                if(!m_userNets.TryGetValue(userIds[i], out UserNetBehavior userNetBehavior))
                {
                    continue;
                }
                userNetBehavior.SendMessage(bytes,bCloseSocket);
            }
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
                    m_userNets.Remove(userId);
                    userId = 0;
                }
                MessageResponseLogin responseLogin = await m_loginHander.OnMessage(userId, request) as MessageResponseLogin;
                if (responseLogin.ErrorCode == MessageErrorCode.Success)
                {
                    net.UserId = responseLogin.UserId;
                    if (m_userNets.TryGetValue(responseLogin.UserId,out UserNetBehavior rNet))
                    {
                        m_noticeError.ErrorCode = MessageErrorCode.OtherLogin;
                        bytes = MessagePackSerializer.Serialize<IMessage>(m_noticeError);
                        rNet.SendMessage(bytes, true);
                        m_userNets.Remove(responseLogin.UserId);
                    }
                    m_userNets.Add(responseLogin.UserId, net);
                }
                bytes = MessagePackSerializer.Serialize<IMessage>(responseLogin);
                net.SendMessage(bytes, true);
                return;
            }
            if (!m_requestHanders.TryGetValue(type, out AMessageRequestHander hander))
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
