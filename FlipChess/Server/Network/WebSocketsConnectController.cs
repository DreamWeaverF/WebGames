using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace Server.Network
{
    public class WebSocketsConnectController : ControllerBase
    {
        private readonly ILogger<WebSocketsConnectController> m_logger;
        public WebSocketsConnectController(ILogger<WebSocketsConnectController> logger)
        {
            m_logger = logger;
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
            var buffer = new byte[1024 * 4];
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            m_logger.Log(LogLevel.Information, "Message received from Client");
            //todolist 连接成功消息
            while (!result.CloseStatus.HasValue)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                var serverMsg = Encoding.UTF8.GetBytes($"Server: Hello. You said: {Encoding.UTF8.GetString(buffer)}");
                await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
                m_logger.Log(LogLevel.Information, "Message sent to Client");
            }
        }
    }
}
