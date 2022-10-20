using GameCommon;
using System.Collections.Generic;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageNoticeFightResultSender : AMessageNoticeSender<MessageNoticeFightResult>
    {
        public void SendMessage(List<long> userIds, long winUserId,GameCommon.FightResultType fightResultType)
        {
			m_notice.WinUserId = winUserId;
			m_notice.fightResultType = fightResultType;

            SendMessageCore(userIds);
        }
    }
}