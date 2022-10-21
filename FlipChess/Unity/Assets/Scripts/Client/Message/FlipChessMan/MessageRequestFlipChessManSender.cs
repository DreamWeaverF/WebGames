using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestFlipChessManSender : AMessageRequestSender<MessageRequestFlipChessMan,MessageResponseFlipChessMan>
    {
        [SerializeField]
        private FightStorage m_fightStorage;
        [SerializeField]
        private UserStorage m_userStorage;
        public async Task<bool> SendMessage(GameCommon.Vector2I targetPosition)
        {
            if(!m_fightStorage.FightData.CheckFlipChessMan(m_userStorage.UserData.UserId, targetPosition))
            {
                return false;
            }
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