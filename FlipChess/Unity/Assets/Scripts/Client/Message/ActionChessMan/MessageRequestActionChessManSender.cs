using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestActionChessManSender : AMessageRequestSender<MessageRequestActionChessMan,MessageResponseActionChessMan>
    {
        public async ETTask<MessageResponseActionChessMan> SendMessage(GameCommon.Vector2I curPosition,GameCommon.Vector2I targetPosition)
        {
			m_request.CurPosition = curPosition;
			m_request.TargetPosition = targetPosition;

            return await SendMessageCore();
        }
    }
}