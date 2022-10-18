using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestJoinFightSender : AMessageRequestSender<MessageRequestJoinFight,MessageResponseJoinFight>
    {
        public async ETTask<MessageResponseJoinFight> SendMessage(System.Int32 fightId)
        {
			m_request.FightId = fightId;

            return await SendMessageCore();
        }
    }
}