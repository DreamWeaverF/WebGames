using UnityEngine;
using UnityWebSocket;
using System.Collections.Generic;
using ET;
using MessagePack;
using GameCommon;
using System;

namespace GameClient
{
    public class NetworkController : AMonoBehaviour
    {
        [SerializeField]
        private ConfigClientLaunch m_configClientLanch;
        [SerializeField]
        private MessageRequestLoginSender m_loginSender;
        [SerializeField]
        private SerializationDictionary<Type, AMessageNoticeHander> m_noticeHanders;

        private IWebSocket m_socket;
        private Dictionary<int, ETTask<AMessageResponse>> m_requestCallbacks = new Dictionary<int, ETTask<AMessageResponse>>();

        private int m_rpcId;
        protected override void OnInit()
        {
            m_socket = new WebSocket(m_configClientLanch.ServerAddress);
            m_socket.OnOpen += OnSocketOpen;
            m_socket.OnMessage += OnSocketMessage;
            m_socket.OnClose += OnSocketClose;
            m_socket.OnError += OnSocketError;
            m_socket.ConnectAsync();
        }
        protected override void UnInit()
        {
            m_socket.OnOpen -= OnSocketOpen;
            m_socket.OnMessage -= OnSocketMessage;
            m_socket.OnClose -= OnSocketClose;
            m_socket.OnError -= OnSocketError;
            m_socket.CloseAsync();
            m_socket = null;
        }
        [SynchronizeMethod(SyncName = SyncName.MessageRequestSender)]
        private ETTask<AMessageResponse> OnCallbackMessageRequestSender(AMessageRequest request)
        {
            request.RpcId = ++m_rpcId;
            byte[] bytes = MessagePackSerializer.Serialize<IMessage>(request, MessagePackSerializerOptions.Standard);
            m_socket.SendAsync(bytes);
            ETTask<AMessageResponse> task = ETTask<AMessageResponse>.Create(true);
            m_requestCallbacks.Add(request.RpcId, task);
            return task;
        }
        private void OnSocketMessage(object sender, MessageEventArgs e)
        {
            IMessage message = MessagePackSerializer.Deserialize<IMessage>(e.RawData);
            switch (message)
            {
                case AMessageResponse response:
                    if (!m_requestCallbacks.TryGetValue(response.RpcId, out ETTask<AMessageResponse> task))
                    {
                        return;
                    }
                    task.SetResult(response);
                    m_requestCallbacks.Remove(response.RpcId);
                    break;
                case AMessageNotice notice:
                    if(!m_noticeHanders.TryGetValue(notice.GetType(),out AMessageNoticeHander noticeHander))
                    {
                        return;
                    }
                    noticeHander.OnMessage(notice);
                    break;
            }
        }
        private async void OnSocketOpen(object sender, OpenEventArgs e)
        {
            Debug.Log("Client SocketOpen");
            await m_loginSender.SendMessage("1234", "4321");
        }
        private void OnSocketClose(object sender, CloseEventArgs e)
        {
            Debug.Log($"Client SocketClose: {e.Reason}");
        }
        private void OnSocketError(object sender, ErrorEventArgs e)
        {
            Debug.Log($"Client SocketClose: {e.Message}");
        }
    }
}
