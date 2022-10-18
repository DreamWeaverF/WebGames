using GameCommon;
using System.Collections.Generic;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageNoticeMatchFightSender : AMessageNoticeSender<MessageNoticeMatchFight>
    {
        public void SendMessage(List<long> userIds, System.Int32 fightId)
        {
			m_notice.FightId = fightId;

            SendMessageCore(userIds);
        }
    }
}