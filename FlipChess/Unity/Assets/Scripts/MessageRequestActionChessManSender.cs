using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestActionChessManSender : AMessageRequestSender<MessageRequestActionChessMan,MessageResponseActionChessMan>
    {
        public async ETTask<MessageResponseActionChessMan> SendMessage(GameCommon.Vector2I curPosition,GameCommon.Vector2I targetPosition,System.Int32 rpcId)
        {
			m_request.CurPosition = curPosition;
			m_request.TargetPosition = targetPosition;
			m_request.RpcId = rpcId;

            return await SendMessageCore();
        }
    }
}