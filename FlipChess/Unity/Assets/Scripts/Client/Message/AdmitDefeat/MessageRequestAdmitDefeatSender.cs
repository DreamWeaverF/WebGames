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
            if (!m_fightStorage.FightData.CheckAdmitDefeat())
            {
                return false;
            }
            bool success = await BroadMessage();
            if (!success)
            {
                return false;
            }
            return true;
        }
    }
}