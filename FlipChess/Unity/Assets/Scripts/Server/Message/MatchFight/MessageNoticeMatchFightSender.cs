using GameCommon;
using System.Collections.Generic;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageNoticeMatchFightSender : AMessageNoticeSender<MessageNoticeMatchFight>
    {
        public void SendMessage(List<long> userIds, int fightId,long userId1,long userId2)
        {
			m_notice.FightId = fightId;
            m_notice.UserId1 = userId1;
            m_notice.UserId2 = userId2;

            SendMessageCore(userIds);
        }
    }
}