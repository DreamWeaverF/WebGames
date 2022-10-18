using GameCommon;
using ET;

namespace GameClient
{
    [GenerateAutoClass]
    public class MessageRequestMatchFightSender : AMessageRequestSender<MessageRequestMatchFight,MessageResponseMatchFight>
    {
        public async ETTask<MessageResponseMatchFight> SendMessage()
        {

            return await SendMessageCore();
        }
    }
}