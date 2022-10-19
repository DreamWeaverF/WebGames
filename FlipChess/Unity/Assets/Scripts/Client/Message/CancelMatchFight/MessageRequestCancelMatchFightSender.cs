using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestCancelMatchFightSender : AMessageRequestSender<MessageRequestCancelMatchFight,MessageResponseCancelMatchFight>
    {
        [SerializeField]
        private UserStorage m_userStorage;
        public async Task<bool> SendMessage()
        {
            bool success = await BroadMessage();
            if (!success)
            {
                return false;
            }
            m_userStorage.UserData.UserState = UserState.None;
            return true;
        }
    }
}