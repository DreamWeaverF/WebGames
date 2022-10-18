using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestFightChatSender : AMessageRequestSender<MessageRequestFightChat,MessageResponseFightChat>
    {
        public async ETTask<MessageResponseFightChat> SendMessage(System.String context)
        {
			m_request.Context = context;

            return await SendMessageCore();
        }
    }
}