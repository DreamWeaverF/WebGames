using GameCommon;
using System.Collections.Generic;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageNoticeFlipChessManSender : AMessageNoticeSender<MessageNoticeFlipChessMan>
    {
        public void SendMessage(List<long> userIds, GameCommon.Vector2I targetPosition)
        {
			m_notice.TargetPosition = targetPosition;

            SendMessageCore(userIds);
        }
    }
}