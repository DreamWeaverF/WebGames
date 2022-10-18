using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestLoginSender : AMessageRequestSender<MessageRequestLogin,MessageResponseLogin>
    {
        public async ETTask<MessageResponseLogin> SendMessage(System.String userName,System.String password)
        {
			m_request.UserName = userName;
			m_request.Password = password;

            return await SendMessageCore();
        }
    }
}