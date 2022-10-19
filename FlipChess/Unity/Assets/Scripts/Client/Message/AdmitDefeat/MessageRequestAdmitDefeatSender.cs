using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestAdmitDefeatSender : AMessageRequestSender<MessageRequestAdmitDefeat,MessageResponseAdmitDefeat>
    {
        [SerializeField]
        private FightStorage m_fightStorage;
        [SerializeField]
        private UserStorage m_userStorage;
        public async Task<bool> SendMessage()
        {
            m_fightStorage.FightData.CheckAdmitDefeat(m_userStorage.UserData.UserId);
            bool success = await BroadMessage();
            if (!success)
            {
                return false;
            }
            return true;
        }
    }
}