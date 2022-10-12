using GameCommon;
using System;
using System.Threading.Tasks;
using System.Threading;
using WebSocketSharp.Net;
using WebSocketSharp.Net.WebSockets;
using WebSocketSharp;
using MessagePack;

namespace GameServer
{
    public class NetworkController : MonoBehaviourEx
    {
        private string m_ipAddress = "http://127.0.0.1:50001/connect/";

        private string m_wsAddress = "ws://127.0.0.1:50011/connect";

        private HttpListener m_listener;
        private Thread m_listenerThread;
        async void Start()
        {
            m_listener = new HttpListener();
            m_listener.Prefixes.Add(m_ipAddress);
            m_listener.Start();

            m_listenerThread = new Thread(StartListener);
            m_listenerThread.Start();

            //Debug.Log("StartListen");

            await Task.Delay(1000);

            //var uri = new Uri(m_wsAddress);
            //System.Net.WebSockets.ClientWebSocket socket = new System.Net.WebSockets.ClientWebSocket();
            //await socket.ConnectAsync(uri, CancellationToken.None);
            ////
            //Debug.Log("222");
        }

        private void StartListener()
        {
            while (true)
            {
                var result = m_listener.BeginGetContext(ListenerCallback, m_listener);
                result.AsyncWaitHandle.WaitOne();
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            var context = m_listener.EndGetContext(result);

            if (!context.Request.IsWebSocketRequest)
            {
                context.Response.StatusCode = 400;
                context.Response.Close();
                return;
            }
            HttpListenerWebSocketContext webSocketContext = context.AcceptWebSocket("binary");
            WebSocket webSocket = webSocketContext.WebSocket;

            webSocket.OnMessage += OnMessage;


            //byte[] buffer = new byte[1024];
            //收到登录消息
            //var result = await webSocket.OnMessage(new ArraySegment<byte>(buffer), CancellationToken.None);

        }

        public void OnMessage(object sender, MessageEventArgs args)
        {
            MessageRequestLogin requestLogin = MessagePackSerializer.Deserialize<IMessage>(args.RawData) as MessageRequestLogin;

            MessageResponseLogin response = new MessageResponseLogin();
            response.RpcId = requestLogin.RpcId;
            response.UserId = 100;
            var buffer = MessagePackSerializer.Serialize<IMessage>(response);
            (sender as WebSocket).SendAsync(buffer, null);
        }

        //private async void StartListen()
        //{
        //    m_listener = new HttpListener();
        //    m_listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
        //    m_listener.Prefixes.Add(m_ipAddress);
        //    m_listener.Start();

        //    Debug.Log("StartListen");

        //    while (true)
        //    {
        //        HttpListenerContext context = await m_listener.GetContextAsync();
        //        if (!context.Request.IsWebSocketRequest)
        //        {
        //            context.Response.StatusCode = 400;
        //            context.Response.Close();
        //            Debug.LogWarning("连接错误");
        //            continue;
        //        }
        //        Debug.Log("Accepted");
        //        var wsContext = await context.AcceptWebSocketAsync(null);
        //        var webSocket = wsContext.WebSocket;
        //        byte[] buffer = new byte[1024];
        //        WebSocketReceiveResult received = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //        while (received.MessageType != WebSocketMessageType.Close)
        //        {
        //            await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, received.Count), received.MessageType, received.EndOfMessage, CancellationToken.None);
        //            received = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        //        }
        //        await webSocket.CloseAsync(received.CloseStatus.Value, received.CloseStatusDescription, CancellationToken.None);
        //        webSocket.Dispose();
        //        Console.WriteLine("Finished");
        //    }
        //}
    }
}
