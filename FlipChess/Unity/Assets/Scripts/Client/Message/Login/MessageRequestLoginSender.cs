using GameCommon;
using System.Threading.Tasks;
using UnityEngine;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestLoginSender : AMessageRequestSender<MessageRequestLogin,MessageResponseLogin>
    {
        [SerializeField]
        private MessageRequestUserDataSender m_sender;
        public async Task<bool> SendMessage(System.String userName,System.String password)
        {
			m_request.UserName = userName;
			m_request.Password = password;
            bool success = await BroadMessage();
            if (!success)
            {
                return false;
            }
            success = await m_sender.SendMessage();
            if (!success)
            {
                return false;
            }
            return true;
        }
    }
}