using GameCommon;
using System.Collections.Generic;

namespace GameServer
{
    [GenerateAutoClass]
    public class MessageNoticeFightResultSender : AMessageNoticeSender<MessageNoticeFightResult>
    {
        public void SendMessage(List<long> userIds, GameCommon.FightCamp winCamp,GameCommon.FightResultType fightResultType)
        {
			m_notice.WinCamp = winCamp;
			m_notice.fightResultType = fightResultType;

            SendMessageCore(userIds);
        }
    }
}