using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestFlipChessManSender : AMessageRequestSender<MessageRequestFlipChessMan,MessageResponseFlipChessMan>
    {
        public async ETTask<MessageResponseFlipChessMan> SendMessage(GameCommon.Vector2I targetPosition)
        {
			m_request.TargetPosition = targetPosition;

            return await SendMessageCore();
        }
    }
}