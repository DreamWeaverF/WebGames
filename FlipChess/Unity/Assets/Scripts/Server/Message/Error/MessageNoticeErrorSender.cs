using GameCommon;
using System.Collections.Generic;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageNoticeErrorSender : AMessageNoticeSender<MessageNoticeError>
    {
        public void SendMessage(List<long> userIds, GameCommon.MessageErrorCode errorCode)
        {
			m_notice.ErrorCode = errorCode;

            SendMessageCore(userIds);
        }
    }
}