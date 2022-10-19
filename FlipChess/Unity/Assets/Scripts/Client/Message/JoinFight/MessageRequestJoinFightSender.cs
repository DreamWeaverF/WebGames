using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestJoinFightSender : AMessageRequestSender<MessageRequestJoinFight,MessageResponseJoinFight>
    {
        [SerializeField]
        private FightStorage m_fightStorage;
        public async Task<bool> SendMessage(System.Int32 fightId)
        {
			m_request.FightId = fightId;
            bool success = await BroadMessage();
            if (!success)
            {
                return false;
            }
            return true;
        }
    }
}