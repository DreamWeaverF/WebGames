using GameCommon;
using System.Collections.Generic;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageNoticeActionChessManSender : AMessageNoticeSender<MessageNoticeActionChessMan>
    {
        public void SendMessage(List<long> userIds, GameCommon.Vector2I curPosition,GameCommon.Vector2I targetPosition)
        {
			m_notice.CurPosition = curPosition;
			m_notice.TargetPosition = targetPosition;

            SendMessageCore(userIds);
        }
    }
}