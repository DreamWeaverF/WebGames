using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestCancelMatchFightSender : AMessageRequestSender<MessageRequestCancelMatchFight,MessageResponseCancelMatchFight>
    {
        public async ETTask<MessageResponseCancelMatchFight> SendMessage()
        {

            return await SendMessageCore();
        }
    }
}