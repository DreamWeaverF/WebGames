using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestActionChessManSender : AMessageRequestSender<MessageRequestActionChessMan,MessageResponseActionChessMan>
    {
        [SerializeField]
        private FightStorage m_fightStorage;
        [SerializeField]
        private UserStorage m_userStorage;
        public async Task<bool> SendMessage(GameCommon.Vector2I curPosition,GameCommon.Vector2I targetPosition)
        {
            if (m_fightStorage.FightData.CheckActionCheesMan(m_userStorage.UserData.UserId, curPosition, targetPosition))
            {
                return false;
            }
            m_request.CurPosition = curPosition;
			m_request.TargetPosition = targetPosition;
            bool success = await BroadMessage();
            if (!success)
            {
                return false;
            }
            return true;
        }
    }
}