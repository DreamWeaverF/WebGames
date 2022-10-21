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
        [SerializeField]
        private UserStorage m_userStorage;
        public async Task<bool> SendMessage(System.Int32 fightId)
        {
            bool success = await BroadMessage();
            if (!success)
            {
                return false;
            }
            if (!m_fightStorage.FightData.CheckEnterUser(m_userStorage.UserData.UserId))
            {
                return false;
            }
			m_request.FightId = fightId;
            return true;
        }
    }
}