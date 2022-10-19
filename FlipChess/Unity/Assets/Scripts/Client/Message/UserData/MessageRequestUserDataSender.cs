using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestUserDataSender : AMessageRequestSender<MessageRequestUserData,MessageResponseUserData>
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
            m_userStorage.UserData = m_response.UserData;
            m_userStorage.UserData.UserState = m_response.UserState;
            m_userStorage.UserData.FightID = m_response.FightID;
            switch (m_response.UserState)
            {
                case UserState.Matching:

                    break;
                case UserState.Fight:

                    break;
            }
            return true;
        }
    }
}