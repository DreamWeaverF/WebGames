using GameCommon;
using System.Collections.Generic;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageNoticeJoinFightSender : AMessageNoticeSender<MessageNoticeJoinFight>
    {
        public void SendMessage(List<long> userIds, System.Int64 userId,System.String userNick,System.String userHeadIcon)
        {
			m_notice.UserId = userId;
			m_notice.UserNick = userNick;
			m_notice.UserHeadIcon = userHeadIcon;

            SendMessageCore(userIds);
        }
    }
}