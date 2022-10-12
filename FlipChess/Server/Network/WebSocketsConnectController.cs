using MessagePack;
using Microsoft.AspNetCore.Mvc;

namespace Dreamwear
{
    public class WebSocketsConnectController : ControllerBase
    {
        private ILogger<WebSocketsConnectController> m_logger;
        private UserController m_userController;

        private long m_userId;
        public WebSocketsConnectController(ILogger<WebSocketsConnectController> logger, UserController userController)
        {
            m_logger = logger;
            m_userController = userController;
        }
        [HttpGet("/connect")]
        public async Task Connect()
        {
            if (!HttpContext.WebSockets.IsWebSocketRequest)
            {
                HttpContext.Response.StatusCode = 400;
                return;
            }
            using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            m_logger.Log(LogLevel.Information, "WebSocket connection established");

            byte[] buffer = new byte[1024];
            //收到登录消息
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            MessageRequestLogin requestLogin = MessagePackSerializer.Deserialize<MessageRequestLogin>(buffer);
            
            //todolist checkSQL
            MessageResponseLogin response = new MessageResponseLogin();
            response.RpcId = requestLogin.RpcId;
            response.UserId = 100;
            buffer = MessagePackSerializer.Serialize(response);
            await webSocket.SendAsync(buffer, result.MessageType, result.EndOfMessage, CancellationToken.None);
            //
            m_userId = 100;
            m_userController.Connect(m_userId,HttpContext.Connection.Id);
            IMessage request;
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                request = MessagePackSerializer.Deserialize<IMessage>(buffer);
            }
            //m_userController.UserDisConnect(m_userId);

        }
    }
}
