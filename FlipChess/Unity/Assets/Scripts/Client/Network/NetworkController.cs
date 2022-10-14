using UnityEngine;
using UnityWebSocket;
using System.Collections.Generic;
using ET;
using MessagePack;
using GameCommon;

namespace GameClient
{
    public class NetworkController : MonoBehaviourEx
    {
        //[SerializeField]
        private string m_address = "ws://127.0.0.1:50001/connect";

        [SerializeField]
        private MessageRequestLoginSender m_loginSenderSO;

        private IWebSocket m_socket;
        private readonly Dictionary<int, ETTask<AMessageResponse>> m_requestCallbacks = new Dictionary<int, ETTask<AMessageResponse>>();
        void Start()
        {
            m_socket = new WebSocket(m_address);
            m_socket.OnOpen += OnSocketOpen;
            m_socket.OnMessage += OnSocketMessage;
            m_socket.OnClose += OnSocketClose;
            m_socket.OnError += OnSocketError;
            m_socket.ConnectAsync();
        }
        [SynchronizeMethod(SyncName = SyncName.MessageRequestSender)]
        private ETTask<AMessageResponse> OnSendMessage(AMessageRequest request)
        {
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

                    break;
            }
        }
        private void OnSocketOpen(object sender, OpenEventArgs e)
        {
            Debug.Log("Client SocketOpen");
            TestCode();
        }
        private void OnSocketClose(object sender, CloseEventArgs e)
        {

        }

        private void OnSocketError(object sender, ErrorEventArgs e)
        {

        }

        public async void TestCode()
        {
            MessageRequestLogin request = new MessageRequestLogin()
            {
                UserName = "1234",
                Password = "4321",
            };

            MessageResponseLogin response = await m_loginSenderSO.SendMessage(request);
            Debug.Log(response.UserId);
        }
    }
}
