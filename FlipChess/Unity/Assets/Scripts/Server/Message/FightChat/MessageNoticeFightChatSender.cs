using GameCommon;
using System.Collections.Generic;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageNoticeFightChatSender : AMessageNoticeSender<MessageNoticeFightChat>
    {
        public void SendMessage(List<long> userIds, System.String context,System.Int64 userId)
        {
			m_notice.Context = context;
            m_notice.UserId = userId;

            SendMessageCore(userIds);
        }
    }
}